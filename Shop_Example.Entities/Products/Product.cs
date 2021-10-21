using Shop_Example.Entities.Products;
using Shop_Example.Entities.Products.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long Count { get; set; }
        public bool Displayed { get; set; }
        public string Image { get; set; }
        public byte? Discount { get; set; }

        public DateTime TimeCreate { get; set; } = DateTime.Now;
        //Relations
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductTages> ProductTages { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Warranty Warranty { get; set; }



    }
}
