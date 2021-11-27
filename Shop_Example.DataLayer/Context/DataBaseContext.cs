using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop_Example.Entities.Models;
using Shop_Example.DataLayer.Configurations;
using Shop_Example.Entities.Carts;
using Shop_Example.Entities.Billboard;
using Shop_Example.Entities.Products;
using Shop_Example.Entities.Products.Comments;
using Shop_Example.DataLayer.Configurations.Comments;
using Shop_Example.DataLayer.Configurations.Discounts;
using Shop_Example.Entities.Home.HomeCategories;

namespace Shop_Example.DataLayer.Context
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTages> ProductTages { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<United> Uniteds { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Warranty> Warranties { get; set; }


        public DbSet<Comment> Comments { get; set; }
        public DbSet<Point> Points { get; set; }

        public DbSet<Stars> Stars { get; set; }

        public DbSet<Helpful> Helpfuls { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<HomeCategory> HomeCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            ApplyConfigurations(builder);

            //For Category

            builder.Entity<Category>()
                .HasOne<Category>()
                .WithMany(p => p.SubCategories)
                .HasForeignKey(p => p.ParentId);

            //

            //For Address

            builder.Entity<Address>()
                .HasOne(p => p.City)
                .WithMany();

            builder.Entity<User>()
                .HasOne(p => p.Address)
                .WithOne(p => p.User)
                .HasForeignKey<User>();

            builder.Entity<Address>()
                .HasOne(p => p.User)
                .WithOne(p => p.Address)
                .HasForeignKey<Address>(p => p.UserId);

            //

            //For Warranty

            builder.Entity<Product>()
                .HasOne(p => p.Warranty)
                .WithOne(p => p.Product)
                .HasForeignKey<Product>();

            builder.Entity<Warranty>()
                .HasOne(p => p.Product)
                .WithOne(p => p.Warranty)
                .HasForeignKey<Warranty>(p => p.ProductId);

            //

            builder.Entity<Comment>()
                .HasMany(p => p.Points)
                .WithOne()
                .HasForeignKey(p => p.CommentId);


            builder.Entity<Stars>()
                .HasOne(p => p.Comment)
                .WithOne(p => p.Stars)
                .HasForeignKey<Stars>(p => p.CommentId);

            //

            builder.Entity<Helpful>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            //
            base.OnModelCreating(builder);
        }

        private static void ApplyConfigurations(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());

            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new ProductFeatureConfig());
            builder.ApplyConfiguration(new ProductTagesConfig());
            builder.ApplyConfiguration(new ProductImagesConfig());
            builder.ApplyConfiguration(new AddressConfig());

            builder.ApplyConfiguration(new UnitedConfig());
            builder.ApplyConfiguration(new CityConfig());

            builder.ApplyConfiguration(new CategoryConfig());

            builder.ApplyConfiguration(new SliderConfig());

            builder.ApplyConfiguration(new WarrantyConfig());

            builder.ApplyConfiguration(new PointConfig());
            builder.ApplyConfiguration(new CommentConfig());

            builder.ApplyConfiguration(new StarsConfig());

            builder.ApplyConfiguration(new FavoriteConfig());

            builder.ApplyConfiguration(new DiscountConfig());
        }
    }
}
