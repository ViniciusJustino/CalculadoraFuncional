using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using CalculadoraFuncional.Properties;
using Google.Cloud.Firestore;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await Database.CreateTableAsync<Bill>();
            //await Database.CreateTableAsync<Calculator>();
            /*var result = await Database.CreateTableAsync<Bill>().Wait();
            var result2 = await Database.CreateTableAsync<Calculator>();*/
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
        public async ValueTask<int> Add(ModelExample bill)
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
    }
}
