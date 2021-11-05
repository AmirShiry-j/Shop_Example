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
    class WarrantyConfig : IEntityTypeConfiguration<Warranty>
    {
        public void Configure(EntityTypeBuilder<Warranty> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(40);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(150);
            builder.Property(p => p.ProductId).IsRequired();
        }
    }
}
