using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Product
{
    public class ProductsCateogryDto
    {
        public int Page { get; set; }
        public int CounInPage { get; set; }
        public int CountAllItems { get; set; }
        public CateogryDto Cateogry { get; set; }
        public List<ProductDto> Products { get; set; }
    }
    public class CateogryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
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