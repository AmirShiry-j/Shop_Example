using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products
{
    public class ProductInformation
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }

        //Nav
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
