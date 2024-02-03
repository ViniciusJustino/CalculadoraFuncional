using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    public static class Categories
    {
        public static List<Category> ListCategories { get; private set; } 
        
        static Categories()
        {
            ListCategories = new List<Category>
            {
                new Category(1, "Geral"),
                new Category(2, "Hortifruti"),
                new Category(3, "Padaria"),
                new Category(4, "Carnes"),
                new Category(5, "Laticínios"),
                new Category(6, "Congelados"),
                new Category(7, "Bebidas"),
                new Category(8, "Molhos e Condimentos"),
                new Category(9, "Café, Chá e Bebidas Quentes"),
                new Category(10, "Limpeza e Higiene"),
                new Category(11, "Cuidados com o Lar"),
                new Category(12, "Cuidados Pessoais"),
                new Category(13, "Snacks e Doces"),
                new Category(14, "Alimentos Enlatados e Não Perecíveis")
            };
        }

        public static Category GetCategory(int id)
        {
            return ListCategories.Find(category => category.Id == id);
        }

        public static Category GetCategory(Category _category)
        {
            return ListCategories.Find(category => category.Id == _category.Id);
        }
    }
    public class Category
    {
        public int Id { get; }
        public string NameCategory { get; }

        public Category(int id, string nameCategory)
        {
            Id = id;
            NameCategory = nameCategory;
        }
    }
}
