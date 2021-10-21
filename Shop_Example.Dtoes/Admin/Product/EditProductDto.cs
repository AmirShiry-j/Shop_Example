using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Product
{
    public class EditProductDto
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "نمیتوانید بیشتر از 150 حرف وارد کنید")]
        [MinLength(3, ErrorMessage = "نمیتوانید کمتر از 3 حرف وارد کنید")]
        public string Name { get; set; }


        [Display(Name = "مدل محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "نمیتوانید بیشتر از 150 حرف وارد کنید")]
        [MinLength(3, ErrorMessage = "نمیتوانید کمتر از 3 حرف وارد کنید")]
        public string Model { get; set; }


        [Display(Name = "برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "نمیتوانید بیشتر از 150 حرف وارد کنید")]
        [MinLength(3, ErrorMessage = "نمیتوانید کمتر از 3 حرف وارد کنید")]
        public string Brand { get; set; }


        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(50, ErrorMessage = "نمیتوانید کمتر از 50 حرف وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }



        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(0,int.MaxValue,ErrorMessage ="قیمت محصول نمیتواند زیر 0 تومان باشد")]
        public long Price { get; set; }


        [Display(Name = "تعداد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(0, long.MaxValue,ErrorMessage ="تعداد محصول نمیتواند کمتر از 0 عدد باشد")]
        public long Count { get; set; }

        [Display(Name = "قابلیت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public bool Displayed { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Tages { get; set; }

        [Required]
        public string Image { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Range(0, 100, ErrorMessage = "رنج تخفیف از 0 تا 100 درصد میتونه باشه")]
        public byte? Discount { get; set; }
    }
}
