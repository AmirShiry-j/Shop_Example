using System.Collections.Generic;

namespace Shop_Example.Dtoes.Product
{
    public class CommentViewModel
    {
        public int ProductId { get; set; }
        public string ModelNameProduct { get; set; }
        public List<CommentDto> Comments { get; set; }
        public QualityAveragesDto QualityAverages { get; set; }
    }
    public class QualityAveragesDto
    {
        public byte EasyUse { get; set; }//راحتی در استفاده
        public byte Beauty { get; set; }//زیبایی
        public byte QualityBuild { get; set; }//کیفیت ساخت
        public byte Affordable { get; set; }//قیمت نسبت به کیفیت
        public byte Innovation { get; set; }//نوآوری
        public byte Ability { get; set; }//قابلیت ها
    }
    public class CommentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string DateCreateShamsi { get; set; }
        public bool Suggestion { get; set; }

        public string UserFullName { get; set; }

        public List<string> GoodPoints { get; set; }
        public List<string> BadsPoints { get; set; }
        public byte CountStars { get; set; }

        public int CountIsHelpful { get; set; }
        public bool IsHelpfulByUser { get; set; }
        public int CountNoHelpful { get; set; }
        public bool NotHelpfulByUser { get; set; }
    }
}
