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
    class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p => p.FullAddress).IsRequired();
            builder.Property(p => p.RecipientName).IsRequired().HasMaxLength(70);
            builder.Property(p => p.PostalCode).IsRequired().HasMaxLength(50);

            builder.Property(p => p.UserId).IsRequired();
        }
    }
}
