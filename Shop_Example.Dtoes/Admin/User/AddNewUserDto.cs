using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.User
{
    public class AddNewUserDto
    {
        [Required(ErrorMessage = "لطفا نام و نام خانوادگی خود را وارد کنید")]
        [MaxLength(50, ErrorMessage = "نام و نام خانوادگی شما حد اکثر میتواند 50 کاراکتر باشد")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "قالب ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }


        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
        [MinLength(6, ErrorMessage = "رمز عبور شما حداقل باید 6 کاراکتر باشد")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        public string Password { get; set; }


        [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن با هم مطابقت ندارند")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
