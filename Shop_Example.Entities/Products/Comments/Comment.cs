using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products.Comments
{
    public class Comment
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreate { get; set; }
        public bool Suggestion { get; set; }
        public bool Confirmation { get; set; }

        //Navs and Rel
        public string UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Point> Points { get; set; }
        public Stars Stars { get; set; }



    }
}
