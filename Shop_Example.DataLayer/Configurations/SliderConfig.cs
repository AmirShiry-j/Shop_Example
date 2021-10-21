using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop_Example.Entities.Billboard;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.DataLayer.Configurations
{
    class SliderConfig : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            var enumConversion = new EnumToStringConverter<SliderLocation>();

            builder.Property(p => p.Src).IsRequired();
            builder.Property(p => p.Link).IsRequired();
            builder.Property(p => p.Location).HasConversion(enumConversion);

        }
    }
}
