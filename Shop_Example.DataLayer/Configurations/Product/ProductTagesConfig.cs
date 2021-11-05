using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.DataLayer.Configurations
{
    class ProductTagesConfig : IEntityTypeConfiguration<ProductTages>
    {
        public void Configure(EntityTypeBuilder<ProductTages> builder)
        {
            builder.Property(p => p.Value).IsRequired().HasMaxLength(50);
        }
    }
}
