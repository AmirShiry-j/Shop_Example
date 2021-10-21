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
        public long CardId { get; set; }
        public DateTime TimeCreate { get; set; }
        public bool IsFinished { get; set; }
        public Guid BrowserId { get; set; }
        //
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem> cartItems { get; set; }
    }
    public class CartItem
    {
        public int CartItemId { get; set; } 
        public long Price { get; set; }
        public long Count { get; set; }
        //
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
    }
}
