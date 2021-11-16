using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Discount
{
    public class CreateDiscountDto
    {

        [Display(Name = "عنوان تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(60, ErrorMessage = "نمیتوانید بیشتر از 60 حرف وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "استفاده از تخفیف درصدی؟")]


        public bool UsePercentage { get; set; }
        [Display(Name = "درصد تخفیف")]


        [Range(0, 100,ErrorMessage ="عدد وارد شده باید بین 0 تا 100  باشد")]
        public int DiscountPercentage { get; set; }


        [Display(Name = "مبلغ تخفیف")]
        public int DiscountAmount { get; set; }


        [Display(Name = "زمان شروع")]
        public DateTime? StartDate { get; set; }


        [Display(Name = "زمان پایان")]
        public DateTime EndDate { get; set; }


        [Display(Name = "اعمال خودکار؟")]
        public bool RequiresCouponCode { get; set; }


        [Required(ErrorMessage = "کد تخفیف را وارد کنید")]
        [Display(Name = "کد تخفیف")]
        public string CouponCode { get; set; }


        [Display(Name = "کد تخفیف برای کدوم دسته هست؟")]
        public DiscountType DiscountType { get; set; }


        [Display(Name = "محدودیت استفاده")]
        public DiscountLimitationType DiscountLimitation { get; set; }


        [Display(Name = "تعداد دفعات")]
        public int LimitationTimes { get; set; }

        public List<int> ApplidToProductItem { get; set; }
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
