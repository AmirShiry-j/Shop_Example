using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Tools.DiscountHelper
{
    public class Discount
    {
        public long GetShowedPrice(long MainPrice, byte? Discount)
        {
            if (Discount == null || Discount == 0)
            {
                return MainPrice;
            }
            else
            {
                return MainPrice - (MainPrice / 100) * (byte)Discount;
            }
        }
        public long? GetLinedPrice(long MainPrice, byte? Discount)
        {
            if (Discount == null || Discount == 0)
            {
                return null;
            }
            else
            {
                return MainPrice;
            }
        }
    }
}
