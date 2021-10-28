using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products.Comments
{
    public class Stars
    {
        public long Id { get; set; }
        public byte EasyUse { get; set; }//راحتی در استفاده
        public byte Beauty { get; set; }//زیبایی
        public byte QualityBuild { get; set; }//کیفیت ساخت
        public byte Affordable { get; set; }//قیمت نسبت به کیفیت
        public byte Innovation { get; set; }//نوآوری
        public byte Ability { get; set; }//قابلیت ها


        public decimal AverageStars { get; set; }
        //nav
        public long CommentId { get; set; }
        public Comment Comment { get; set; }

    }
}
