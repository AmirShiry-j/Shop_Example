using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products
{
    public class Warranty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //Nav
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
