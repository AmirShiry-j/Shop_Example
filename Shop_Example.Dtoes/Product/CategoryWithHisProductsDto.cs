using System.Collections.Generic;

namespace Shop_Example.Dtoes.Product
{
    public class CategoryWithHisProductsDto 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ShortInfoProductDto> Products { get; set; }
    
    }
}
