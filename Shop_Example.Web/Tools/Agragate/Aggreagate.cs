using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Tools.Agragate
{
    public class Aggregate
    {
        public string GetAggregatePoint(List<string> Point)
        {
            if (Point != null && Point.Any())
            {
                return Point.Aggregate((p1, p2) => p1 + " , " + p2);
            }

            return null;
        }
    }
}
