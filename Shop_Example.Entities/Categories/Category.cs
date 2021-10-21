using System;
using System.Collections.Generic;
namespace Shop_Example.Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public ICollection<Category> SubCategories { get; set; }
        //Nav Prop
        public ICollection<Product> Products { get; set; }

    }
}
