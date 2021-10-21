using System;

namespace Shop_Example.Entities.Models
{
    public class ProductTages
    {
        public int ProductTagesId { get; set; }

        public string Value { get; set; }


        //Nav
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
