using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    internal class Bill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public Category Category { get; set; }
        private string File { get; set; }

        public Bill() 
        { 
            this.Date = DateTime.Now;
            this.Name = "New Bill";
            this.Category = Categories.GetCategory(1);
            this.Value = 0;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public Bill(string name)
        {
            this.Name = name;
            this.Date = DateTime.Now;
            this.Category =  Categories.GetCategory(1);
            this.Value = 0;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public Bill(Category category)
        {
            this.Name = "New Bill";
            this.Date = DateTime.Now;
            this.Category = category;
            this.Value = 0;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public Bill(string name, Category category)
        {
            this.Name = name;
            this.Date = DateTime.Now;
            this.Category = category;
            this.Value = 0;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public Bill(string name, double value)
        {
            this.Name = name;
            this.Date = DateTime.Now;
            this.Category = Categories.GetCategory(1);
            this.Value = value;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public Bill(string name, double value, DateTime date)
        {
            this.Name = name;
            this.Date = date;
            this.Category = Categories.GetCategory(1);
            this.Value = value;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public Bill(DateTime date, Category category)
        {
            this.Name = "New Bill";
            this.Date = date;
            this.Category = category;
            this.Value = 0;
            this.File = $"{this.Date.ToString()}-{this.Name}.Bill.xml";
        }

        public void Add()
        {

        }

        public static IEnumerable<Bill> LoadAll()
        {
            Random random = new Random();
            int countOfBills = random.Next(1, 25);

            List<Bill> _bills = new();
            int ano, mes, dia;
            ano = 2023;

            for (int i = 1; i <= countOfBills; i++)
            {
                
                mes = random.Next(1, 12);
                dia = random.Next(1, DateTime.DaysInMonth(ano, mes) + 1);

                _bills.Add(new Bill($"teste{Convert.ToString(i)}", 
                                      random.Next(5000),
                                      new DateTime(ano, mes, dia)));
            }

            return _bills;
        }

    }
}
