using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Profile
{
    public class CommentsViewModel
    {
        public List<CommentDto> Comments { get; set; }
    }
    public class CommentDto
    {        
        public long CommentId { get; set; }
        public bool IsConfirmed { get; set; }
        public string CommentText { get; set; }
        public byte CommentStars { get; set; }

        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
    }
}
