using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.HomeCategory
{
    public class CreateHomeCategoryDto
    {
        [Required(ErrorMessage = "شما دسته بندی رو انتخاب نکردید")]
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
