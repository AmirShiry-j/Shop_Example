using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Role
{
    public class AddNewRoleDto
    {
        [Required(ErrorMessage ="لطفا نام نقش رو وارد کنید")]
        [MaxLength(20,ErrorMessage ="نام نقش نهایتا میتواند 20 کاراکتر باشد")]
        public string Name { get; set; }
        [Required(ErrorMessage ="لطفا توضیحات نقش رو وارد کنید")]
        [MaxLength(50,ErrorMessage ="توضیحات نقش نهایتا میتواند 50 کاراکتر باشد")]
        public string Description { get; set; }
    }
}
