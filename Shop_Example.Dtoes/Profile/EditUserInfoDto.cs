using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Profile
{
    public class EditUserInfoDto
    {
        [Required(ErrorMessage = "لطفا نام و نام خانوادگی خود را وارد کنید")]
        [MaxLength(50, ErrorMessage = "نام و نام خانوادگی شما حد اکثر میتواند 50 کاراکتر باشد")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "قالب ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }
        [RegularExpression("(09)[0-9]{9}",ErrorMessage ="قالب شماره همراه وارد شده صحیح نیست")]
        public string PhoneNumber { get; set; }
        public bool TwoFactorLogin { get; set; }

    }
}
