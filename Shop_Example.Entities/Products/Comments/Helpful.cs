using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Products.Comments
{
    public class Helpful
    {
        public long Id { get; set; }
        public bool WasHelpful { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Comment Comment { get; set; }
        public long CommentId { get; set; }
    }
}
