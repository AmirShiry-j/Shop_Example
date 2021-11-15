using Microsoft.EntityFrameworkCore;
using Shop_Example.Entities.Products;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shop_Example.DataLayer.Configurations.Discounts
{
    class DiscountConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            var discountTypeConversion = new EnumToStringConverter<DiscountType>();
            var discountLimitationConversion = new EnumToStringConverter<DiscountLimitationType>();

            builder.Property(p => p.CouponCode).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.DiscountType).HasConversion(discountTypeConversion);
            builder.Property(p => p.DiscountLimitation).HasConversion(discountLimitationConversion);
        }
    }
}
