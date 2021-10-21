using Shop_Example.Entities.Models;

namespace Shop_Example.Entities.Products.Comments
{
    public class HelpfulCheck
    {
        public long Id { get; set; }
        public bool IsYes { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
