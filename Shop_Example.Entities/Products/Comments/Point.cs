using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products.Comments
{
    public class Point
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public TypePoint TypePoint { get; set; }

        //Nav and Rel
        public long CommentId { get; set; }
    }
    public enum TypePoint
    {
        Strength,
        Weak
    }
}
