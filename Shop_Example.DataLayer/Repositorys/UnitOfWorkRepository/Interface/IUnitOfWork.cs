using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop_Example.DataLayer.Repositorys.GenericRepository.Interfaces;
using Shop_Example.Entities.Billboard;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products;
using Shop_Example.Entities.Products.Comments;

namespace Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Category> CategoryRepository { get; }
        public IGenericRepository<Product> ProductRepository { get; set; }
        public IGenericRepository<ProductTages> TagesRepository { get; set; }
        public IGenericRepository<ProductImages> ProductImagesRepository { get; set; }
        public IGenericRepository<ProductFeature> ProductFeatureRepository { get; set; }
        public IGenericRepository<United> UnitedRepository { get; set; }
        public IGenericRepository<City> CityRepository { get; set; }
        public IGenericRepository<Address> AddressRepository { get; set; }
        public IGenericRepository<Slider> SliderRepository { get; set; }
        public IGenericRepository<Warranty> WarrantyRepository { get; set; }
        public IGenericRepository<Comment> CommentRepository { get; set; }
        public IGenericRepository<Stars> StartRepository { get; set; }
        public IGenericRepository<Point> PointRepository { get; set; }
        public IGenericRepository<Helpful> HelpfulRepository { get; set; }
        public IGenericRepository<Favorite> FavoriteRepository { get; set; }
        public IGenericRepository<Discount> DiscountRepository { get; set; }
    }
}
