using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Product
{
    public class ShortInfoProductDto
    {

        public long ProductId { get; set; }
        public string Name { get; set; }
        public long? LinedPrice { get; set; }
        public string Image { get; set; }
        public long ShowedPrice { get; set; }

    }
}
