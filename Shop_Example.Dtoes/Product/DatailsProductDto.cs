using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Product
{
    public class DatailsProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        //public string Description { get; set; }

        public string Image { get; set; }

        public long ShowedPrice { get; set; }
        public long? LinedPrice { get; set; }

        //Relations
        public List<CategoryDto> Categories { get; set; }
        public List<string> Tages { get; set; }
        public List<string> SrcImages { get; set; }
        public List<FeatureDto> ProductFeatures { get; set; }
        public WarrantyDto Warranty { get; set; }
    }

    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class WarrantyDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class FeatureDto
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
}
