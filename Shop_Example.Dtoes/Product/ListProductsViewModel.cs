using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Product
{
    public class ListProductsViewModel
    {
        public int TotalRecords { get; set; }
        public int Page { get; set; }

        public string CategoryName { get; set; }
        public string SearchKeyName { get; set; }

        public List<ProductDto> ProductsOrderByViews { get; set; }
        public List<ProductDto> ProductsOrderByCheaper { get; set; }
        public List<ProductDto> ProductsOrderByExpensive { get; set; }
        public List<ProductDto> ProductsOrderByDate { get; set; }
        public List<ProductDto> ProductsOrderByFavorite { get; set; }
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Stars { get; set; }
        public bool HasDiscount { get; set; }
        public byte Discount { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
    }
}
