using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string FullAddress { get; set; }
        public string PostalCode { get; set; }
        public string RecipientName { set; get; }

        //
        public City City { get; set; }
        public int CityId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
