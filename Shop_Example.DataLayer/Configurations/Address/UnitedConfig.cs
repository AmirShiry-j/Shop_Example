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
    class UnitedConfig : IEntityTypeConfiguration<United>
    {
        public void Configure(EntityTypeBuilder<United> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}
