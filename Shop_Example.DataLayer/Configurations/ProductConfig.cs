using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop_Example.Entities.Models;

namespace Shop_Example.DataLayer.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);

            builder.Property(p => p.Model).IsRequired().HasMaxLength(150);

            builder.Property(p => p.Brand).IsRequired().HasMaxLength(50);

            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.Count).IsRequired();

            builder.Property(p => p.Displayed).IsRequired();

            builder.Property(p => p.Image).IsRequired().HasMaxLength(60);

        }
    }
}
