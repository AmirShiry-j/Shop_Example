using Microsoft.AspNetCore.Mvc.Rendering;
using Shop_Example.Entities.Billboard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.Slider
{
    public class CreateSliderDto
    {
        [DataType(DataType.Url,ErrorMessage ="قالب وارد شده برای یک لینک مناسب نیست")]
        [Required(ErrorMessage ="لطفا یه لینک برای اسلایدر خود وارد کنید")]
        public string Link { get; set; }
        public SliderLocation Location { get; set; }
        public bool Displayed { get; set; } = true;
    }
}
