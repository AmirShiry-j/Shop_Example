using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required(ErrorMessage ="لطفا عنوان نظر خود را وارد کنید")]
        [MaxLength(70,ErrorMessage ="عنوان نظر شما حداکثر میتواند 70 کاراکتر باشد")]
        public string Title { get; set; }
        [Required(ErrorMessage ="لطفا متن نظر خود را وارد کنید")]
        [MaxLength(400,ErrorMessage ="متن نظر شما حداکثر میتواند 400 کاراکتر باشد")]
        public string Text { get; set; }

        [Range(1,5)]
        [Required]
        public byte EasyUse { get; set; }//راحتی در استفاده
        [Range(1,5)]
        [Required]
        public byte Beauty { get; set; }//زیبایی
        [Range(1,5)]
        [Required]
        public byte QualityBuild { get; set; }//کیفیت ساخت
        [Range(1,5)]
        [Required]
        public byte Affordable { get; set; }//قیمت نسبت به کیفیت
        [Range(1,5)]
        [Required]
        public byte Innovation { get; set; }//نوآوری
        [Range(1,5)]
        [Required]
        public byte Ability { get; set; }//قابلیت ها

        public bool Suggestion { get; set; } = true;
    }
}