using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Profile
{
    public class FavoritesViewModel
    {
        public List<FavoriteDto> Favorites { get; set; }
    }
    public class FavoriteDto
    {
        public long FavoriteId { get; set; }

        public byte ProductAvgStars { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public long ProductPrice { get; set; }
    }
}
