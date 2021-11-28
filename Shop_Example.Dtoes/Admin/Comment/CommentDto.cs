using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Comment
{
    public class CommentDto
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public byte AvgStars { get; set; }
        public string StreaghtPoint { get; set; }
        public string WeakPoint { get; set; }

        public string UserId { get; set; }
        public string FullName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool IsConfirm { get; set; }

        public string TimeCrate { get; set; }
    }
}
