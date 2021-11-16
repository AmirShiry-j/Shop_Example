using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Discount
{
    public class DiscountDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string ValueDiscount { get; set; }
        public string CouponCode { get; set; }

    }
}
