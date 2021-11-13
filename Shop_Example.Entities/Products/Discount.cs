using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products
{
    public class Discount
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool UsePercentage { get; set; }
        public int DiscountPercentage { get; set; }
        public int DiscountAmount { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool RequiresCouponCode { get; set; }
        public string CouponCode { get; set; }

        public DiscountType DiscountType { get; set; }

        public int LimitationTimes { get; set; }
        public DiscountLimitationType DiscountLimitation { get;set; }


        public ICollection<Product> ProductItems { get; set; }
    }

    public enum DiscountType
    {
        [Display(Name = "تخفیف برای محصولات")]
        AssignedProduct = 1,
        [Display(Name = "تخفیف برای دسته بندی")]
        AssignedToCategories = 2
    }

    ///  محدودیت تعداد استفاده
    public enum DiscountLimitationType
    {

        /// بدونه محدودیت تعداد
        [Display(Name = "بدونه محدودیت تعداد")]
        Unlimited = 0,

        /// فقط N بار
        [Display(Name = "فقط N بار")]
        NTimesOnly = 1,
    }
}
