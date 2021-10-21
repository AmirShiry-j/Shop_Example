using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop_Example.Entities.Products.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.DataLayer.Configurations.Comments
{
    class HelpfulCheckConfig : IEntityTypeConfiguration<HelpfulCheck>
    {
        public void Configure(EntityTypeBuilder<HelpfulCheck> builder)
        {

            builder.Property(p => p.UserId).IsRequired();
        }
    }
}
