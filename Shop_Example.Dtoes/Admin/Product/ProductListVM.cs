using System.Collections.Generic;

namespace Shop_Example.Dtoes.Admin.Product
{
    public class ProductListVM
    {
        public int Page { get; set; }
        public int CounInPage { get; set; }
        public int CountAllItems { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}