using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shop_Example.Entities.Products.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.DataLayer.Configurations.Comments
{
    class PointConfig : IEntityTypeConfiguration<Point>
    {
        public void Configure(EntityTypeBuilder<Point> builder)
        {
            var enumToString= new EnumToStringConverter<TypePoint>();

            builder.Property(p => p.TypePoint)
                .IsRequired()
                .HasConversion(enumToString);

            builder.Property(p => p.Text)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
