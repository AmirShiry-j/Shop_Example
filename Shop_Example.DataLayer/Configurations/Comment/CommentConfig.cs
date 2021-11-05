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
    class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.UserId)
               .IsRequired();

            builder.Property(p => p.Title)
                .HasMaxLength(70)
                .IsRequired();

            builder.Property(p => p.Text)
                .HasMaxLength(400)
                .IsRequired();



            /////////////////////////////////////
            ///
        }
    }
}
