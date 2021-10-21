using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.ProductTages
{
    class AddNewTagsDto
    {
        public int TagId { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "نمیتوانید بیشتر از 150 حرف وارد کنید")]
        [MinLength(3, ErrorMessage = "نمیتوانید کمتر از 3 حرف وارد کنید")]
        public string Value { get; set; }
        public int ProductId { get; set; }
    }
}
