using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Carts
{
    public class Cart
    {
        public long Id { get; set; }
        public DateTime TimeCreate { get; set; }
        public bool Finished { get; set; }
        public Guid BrowserId { get; set; }
        //
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
    public class CartItem
    {
        public long Id { get; set; } 
        public long Price { get; set; }
        public long Count { get; set; }

        public DateTime TimeCreate { get; set; }
        //
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Cart Cart { get; set; }
        public long CartId { get; set; }
    }
}
