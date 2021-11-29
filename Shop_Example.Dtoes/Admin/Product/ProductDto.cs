using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Displayed { get; set; }
        public long Price { get; set; }
        public string Image { get; set; }
        public bool HasWarranty { get; set; }
    }
}