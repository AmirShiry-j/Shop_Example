using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products
{
    public class Favorite 
    {
        public long Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
