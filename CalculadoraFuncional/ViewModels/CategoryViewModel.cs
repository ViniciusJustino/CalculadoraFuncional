using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class CategoryViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _nameCategory;
        [ObservableProperty]
        private string _colorHex;

        [ObservableProperty]
        private bool _isEdited;
        public Models.Category Category { get; set; }
        public ICommand SaveCategoryCommand { get; private set; }
        public ICommand CancelCategoryCommand { get; private set; }

        public CategoryViewModel()
        {
            this.Category = new();
            SaveCategoryCommand = new AsyncRelayCommand(SaveCategoryAsync);
            CancelCategoryCommand = new AsyncRelayCommand(CancelCategoryAsync);

            PropertyChanged += CategoryViewModel_PropertyChanged;
        }

        public CategoryViewModel(Models.Category category)
        {
            SaveCategoryCommand = new AsyncRelayCommand(SaveCategoryAsync);
            CancelCategoryCommand = new AsyncRelayCommand(CancelCategoryAsync);

            PropertyChanged += CategoryViewModel_PropertyChanged;

            this.Category = category;
            this.NameCategory = category.NameCategory;
            this.ColorHex = category.ColorHex;
        }

        private void CategoryViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "NameCategory")
            { 
                CategoryViewModel categoryViewModel = (CategoryViewModel) sender;

                if (string.IsNullOrWhiteSpace(this.Category.NameCategory) &&
                   !string.IsNullOrWhiteSpace(categoryViewModel.NameCategory))
                {
                    this.IsEdited = true;
                    return;
                }

                if (!this.Category.NameCategory.Equals(categoryViewModel.NameCategory) && !IsEdited)
                {
                    this.IsEdited = true;
                }
                else if(IsEdited)
                {
                    this.IsEdited = false;
                }
            }

            if(e.PropertyName == "ColorHex")
            {
                CategoryViewModel categoryViewModel = (CategoryViewModel)sender;

                if(string.IsNullOrWhiteSpace( this.Category.ColorHex ) &&
                   !string.IsNullOrWhiteSpace( categoryViewModel.ColorHex ))
                {
                    this.IsEdited = true;
                    return;
                }

                if (!this.Category.ColorHex.Equals(categoryViewModel.ColorHex) && !IsEdited)
                {
                    this.IsEdited = true;
                }
                else if (IsEdited)
                {
                    this.IsEdited = false;
                }
            }
        }

        public async Task UpdateCategory()
        {
            if (!this.IsEdited)
                return;

            if (!this.NameCategory.Equals(Category.NameCategory))
                Category.NameCategory = this.NameCategory;

            if(!this.ColorHex.Equals(Category.ColorHex))
                Category.ColorHex = this.ColorHex;

            await App.localDatabase.UpdateCategoryAsync(Category);
        }

        private async Task SaveCategoryAsync()
        {
            if(string.IsNullOrEmpty(NameCategory))
            {
                await Shell.Current.CurrentPage.DisplayAlert("Atenção", "Para prosseguir, é necessário informar o nome da nova categoria.", "OK");
                return;
            }

            this.Category.NameCategory = this.NameCategory;
            this.Category.ColorHex = this.ColorHex;

            await App.localDatabase.AddCategory(this.Category);
            await Shell.Current.GoToAsync($"..?Refresh={null}");
        }

        private async Task CancelCategoryAsync()
        {
            this.NameCategory = string.Empty;
            this.ColorHex = string.Empty;
            await Shell.Current.GoToAsync($"..?Refresh={null}");
        }
    }
}
