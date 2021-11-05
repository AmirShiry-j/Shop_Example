using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop_Example.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.DataLayer.Configurations
{
    public class FavoriteConfig : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.ProductId).IsRequired();
        }
    }
}
