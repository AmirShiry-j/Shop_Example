using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Entities.Billboard
{
    public class Slider
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public string Link { get; set; }
        public bool Displayed { get; set; } = true;
        public SliderLocation Location { get; set; } = SliderLocation.TopLeftMany;
    }
    public enum SliderLocation
    {
        TopRightOne,//بالا راست
        TopLeftMany,//اسلایدر بزرگه بالا چک
        Row2,//اولین دوم پایین
        Row3
    }
}
