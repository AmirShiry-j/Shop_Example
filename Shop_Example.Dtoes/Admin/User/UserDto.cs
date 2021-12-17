using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string ImageProfileName { get; set; }
        public bool IsBlocked { get; set; }

        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }


        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }


        public virtual bool TwoFactorEnabled { get; set; }


        public virtual bool LockoutEnabled { get; set; }

        public AddressDto Address { get; set; }
        public CartDto Cart { get; set; }
        public List<string> Roles { get; set; }
        
    }
    public class CartDto
    {
        public long Id { get; set; }
        public DateTime TimeCreate { get; set; }
        public bool Finished { get; set; }
        public Guid BrowserId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
    public class CartItemDto
    {
        public long Id { get; set; }
        public long Price { get; set; }
        public long Count { get; set; }

        public string TimeCreate { get; set; }
        
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
    public class AddressDto
    {
        public string UnitedAndCity { get; set; }
        public string FullAddress { get; set; }
        public string PostalCode { get; set; }
        public string RecipientName { set; get; }
    }
}
