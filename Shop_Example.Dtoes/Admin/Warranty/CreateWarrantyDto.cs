using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Warranty
{
    public class CreateWarrantyDto
    {
        [Required]
        [MaxLength(40,ErrorMessage ="نام گارانتی فقط میتواند 40 کاراکتر باشد")]
        public string Name { get; set; }
        [Required]
        [MaxLength(150,ErrorMessage = "توضیحات گارانتی فقط میتواند 150 کاراکتر باشد")]
        public string Description { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
