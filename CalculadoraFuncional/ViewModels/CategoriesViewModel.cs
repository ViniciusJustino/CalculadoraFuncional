using CalculadoraFuncional.Models;
using CalculadoraFuncional.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class CategoriesViewModel: BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private bool _isRefreshing;
        [ObservableProperty]
        private CategoryViewModel _categorySelected;
        private bool _isCollectionEdited;
        public ObservableCollection<CategoryViewModel> listCategories { get; set; }
        
        public ICommand DeleteCategoryCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand NewCategoryCommand { get; private set; }
        public ICommand SaveCategoriesCommand { get; private set; }
        public ICommand SaveCategoryCommand { get; private set; }

        public CategoriesViewModel()
        {
            _= Init();
            PropertyChanged += CategoriesViewModel_PropertyChanged;

            DeleteCategoryCommand = new AsyncRelayCommand<CategoryViewModel>(DeleteCategoryAsync);
            NewCategoryCommand = new AsyncRelayCommand(NewCategoryAsync);
            RefreshCommand = new AsyncRelayCommand(RefreshCategoriesAsync);
            SaveCategoriesCommand = new AsyncRelayCommand(SaveCategoriesAsync);
            SaveCategoryCommand = new AsyncRelayCommand<CategoryViewModel>(SaveCategoryAsync);
        }

        private void CategoriesViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var ob = sender;
        }

        private async Task Init()
        {
            IEnumerable<Category> list = await Category.LoadAllAsync();

            this.listCategories = new ObservableCollection<CategoryViewModel>(list.Select(c => new CategoryViewModel(c)));

            OnPropertyChanged(nameof(this.listCategories));
        }

        private async Task RefreshCategoriesAsync()
        {
            this.IsRefreshing = true;

            listCategories.Clear();
            await Init();

            this.IsRefreshing = false;
        }

        private async Task DeleteCategoryAsync(CategoryViewModel category)
        {
            try 
            { 
                bool isDeleted = listCategories.Remove(category);

                if (isDeleted)
                {
                    await App.localDatabase.DeleteCategoryAsync(category.Category);
                    OnPropertyChanged(nameof(this.listCategories));
                }
            }catch(Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Notificação", "Erro ao excluir a categoria", "Ok");
                await RefreshCategoriesAsync();
            }
            
        }

        private async Task NewCategoryAsync()
        {
            await Shell.Current.GoToAsync(nameof(CategoryPage));
        }

        private async Task SaveCategoriesAsync()
        {
            foreach (var updateCategory in listCategories)
            { 
                try
                {
                    await updateCategory.UpdateCategory();
                }
                catch (Exception ex) 
                {
                    
                    continue;
                }
            }

            OnPropertyChanged(nameof(this.listCategories));
            _ = RefreshCategoriesAsync();
        }

        private async Task SaveCategoryAsync(CategoryViewModel categoriesViewModel)
        {
            try
            {

                await categoriesViewModel.UpdateCategory();
                categoriesViewModel.IsEdited = false;

                OnPropertyChanged(nameof(listCategories));

                _= RefreshCategoriesAsync();
            }
            catch(Exception ex) 
            {
                await Shell.Current.DisplayAlert("Atenção", "Não foi possível atualizar a categoria.", "Ok");
            }
            
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
                return;

            if(query.ContainsKey("Refresh"))
            {
                _=RefreshCategoriesAsync();
            }
        }

        
    }
}

