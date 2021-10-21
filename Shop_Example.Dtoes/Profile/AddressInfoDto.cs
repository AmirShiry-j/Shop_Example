using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Profile
{
    public class AddressInfoDto
    {
        public long AddressId { get; set; }
        public string RecipientName { get; set; }
        public string PostalCode { get; set; }
        public string UnitedAndCityName { get; set; }
        public string FullAddress { get; set; }

    }
}
