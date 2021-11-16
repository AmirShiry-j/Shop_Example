using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Discount
{
    public class DetailDiscountDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ValueDiscount { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public bool RequiresCouponCode { get; set; }
        public string CouponCode { get; set; }
        public string DiscountType { get; set; }
        public string Limitation { get; set; }

        public List<ProductDto> Products { get; set; }
    }
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }



}
