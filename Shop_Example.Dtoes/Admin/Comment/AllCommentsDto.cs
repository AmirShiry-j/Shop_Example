using System.Collections.Generic;

namespace Shop_Example.Dtoes.Admin.Comment
{
    public class AllCommentsDto
    {
        public List<CommentDto> Comments { get; set; }
        public int Page { get; set; }
        public int CountComments { get; set; }
        public int CountInPage { get; set; }
    }
}
