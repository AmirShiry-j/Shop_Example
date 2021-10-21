using System;

namespace Shop_Example.Entities.Models
{
    public class ProductImages
    {
        public int ProductImagesId { get; set; }

        public string Image { get; set; }


        //Nav
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
