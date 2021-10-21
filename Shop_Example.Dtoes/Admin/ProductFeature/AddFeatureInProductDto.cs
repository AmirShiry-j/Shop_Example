using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.ProductFeature
{
    public class AddFeatureInProductDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Value { get; set; }


        public string ProductName { get; set; }

    }
}
