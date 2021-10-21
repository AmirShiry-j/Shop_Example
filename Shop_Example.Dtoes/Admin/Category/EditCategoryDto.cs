using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Category
{
    public class EditCategoryDto
    {
        public int CategoryId { get; set; }

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(80, ErrorMessage = "نمیتوانید بیشتر از 150 حرف وارد کنید")]
        public string Title { get; set; }
        public int? ParentId { get; set; }
    }
}
