using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using CalculadoraFuncional.Properties;
using Google.Cloud.Firestore;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Services
{
    public partial class LocalDatabaseSQLite : IHandlerLocalDatabase
    {
        SQLiteAsyncConnection Database;

        

        public LocalDatabaseSQLite()
        {

        }

        public async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(SQLiteConstants.DatabasePath, SQLiteConstants.Flags);
            var b = await Database.CreateTableAsync<Bill>();
            var cy = await Database.CreateTableAsync<Category>();
            var c = await Database.CreateTableAsync<Calculator>();
            
            InitCategories();

        }

        private async void InitCategories()
        {
            List<Category> list = await Database.Table<Category>().ToListAsync();

            if(list.Count <= 0)
            {
                List<string> ListCategories = new List<string>
                {
                    "Geral",
                    "Hortifruti",
                    "Padaria",
                    "Carnes",
                    "Laticínios",
                    "Congelados",
                    "Bebidas",
                    "Molhos e Condimentos",
                    "Café, Chá e Bebidas Quentes",
                    "Limpeza e Higiene",
                    "Cuidados com o Lar",
                    "Cuidados Pessoais",
                    "Snacks e Doces",
                    "Alimentos Enlatados e Não Perecíveis"
                };

                await Database.InsertAllAsync(ListCategories.Select(x => new Category() { NameCategory = x}).ToList<Category>());
            }
        }

        public async ValueTask<IEnumerable<Bill>> GetAllBills()
        {
            await Init();

            return await Database.Table<Bill>().ToListAsync();
        }

        public async ValueTask<Bill> GetBillAsync(int Id)
        {
            await Init();

            return await Database.Table<Bill>().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async ValueTask<Bill> GetBillAsync(string Name)
        {
            await Init();

            return await Database.Table<Bill>().FirstOrDefaultAsync(x => x.Name == Name);
        }

        public async ValueTask<int> AddBill(Bill bill)
        {
            await Init();

            return await Database.InsertAsync(bill);
        }

        public async ValueTask<int> UpdateBillAsync(Bill bill)
        {
            await Init();

            return await Database.UpdateAsync(bill);
        }

        public async ValueTask<int> DeleteBillAsync(Bill bill)
        {
            await Init();

            return await Database.DeleteAsync(bill);
        }

        public async ValueTask<int> AddCalculator(Calculator calculator)
        {
            await Init();

            return await Database.InsertAsync(calculator);
        }

        public async ValueTask<IEnumerable<Calculator>> GetAllCalculators()
        {
            await Init();

            return await Database.Table<Calculator>().ToListAsync();
        }

        public async ValueTask<Calculator> GetCalculatorAsync(int Id)
        {
            await Init();

            return await Database.Table<Calculator>().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async ValueTask<IEnumerable<Calculator>> GetAllCalculatorOfBillAsync(Bill bill)
        {
            await Init();

            return await Database.Table<Calculator>().Where(x => x.IdBill == bill.Id).ToListAsync();
        }


        public async ValueTask<int> UpdateCalculatorAsync(Calculator calculator)
        {
            await Init();

            return await Database.UpdateAsync(calculator);
        }

        public async ValueTask<int> DeleteCalculatorAsync(Calculator calculator)
        {
            await Init();

            return await Database.DeleteAsync(calculator);
        }

        public async ValueTask<int> AddCategory(Category category)
        {
            await Init();

            return await Database.InsertAsync(category);
        }

        public async ValueTask<IEnumerable<Category>> GetAllCategories()
        {
            await Init();

            return await Database.Table<Category>().ToListAsync();
        }

        public async ValueTask<Category> GetCategoryAsync(int Id)
        {
            await Init();

            return await Database.Table<Category>().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async ValueTask<int> UpdateCategoryAsync(Category category)
        {
            await Init();

            return await Database.UpdateAsync(category);
        }

        public async ValueTask<int> DeleteCategoryAsync(Category category)
        {
            await Init();

            return await Database.DeleteAsync(category);
        }
    }
}
