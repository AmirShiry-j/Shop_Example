using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.ProductImage
{
    class AddProductImageDto
    {
        public int ProductImagesId { get; set; }
        public int ProductId { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "نمیتوانید بیشتر از 50 کاراکتر وارد کنید")]
        public string Image { get; set; }
    }
}
