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
    public class StarsConfig : IEntityTypeConfiguration<Stars>
    {
        public void Configure(EntityTypeBuilder<Stars> builder)
        {
            builder.Property(p => p.Beauty).HasMaxLength(5);
            builder.Property(p => p.Ability).HasMaxLength(5);
            builder.Property(p => p.EasyUse).HasMaxLength(5);
            builder.Property(p => p.Innovation).HasMaxLength(5);
            builder.Property(p => p.QualityBuild).HasMaxLength(5);
            builder.Property(p => p.Affordable).HasMaxLength(5);
            
            //Take Average for Count Stars
            builder.Property(p => p.AverageStars).HasComputedColumnSql("CAST((([Beauty]+[Ability]+[EasyUse]+[Innovation]+[QualityBuild]+[Affordable])/6.0) AS decimal(5, 2))");
        }
    }
}
