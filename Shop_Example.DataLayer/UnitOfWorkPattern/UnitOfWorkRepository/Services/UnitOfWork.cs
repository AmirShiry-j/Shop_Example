using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop_Example.DataLayer.Context;
using Shop_Example.DataLayer.Repositorys.GenericRepository.Interfaces;
using Shop_Example.DataLayer.Repositorys.GenericRepository.Services;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Billboard;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products;
using Shop_Example.Entities.Products.Comments;

namespace Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataBaseContext _context;

        public UnitOfWork(DataBaseContext context)
        {
            _context = context;

            CategoryRepository = new GenerecRepositorys<Category>(_context);
            ProductRepository = new GenerecRepositorys<Product>(_context);
            TagesRepository = new GenerecRepositorys<ProductTages>(_context);
            ProductImagesRepository = new GenerecRepositorys<ProductImages>(_context);
            ProductFeatureRepository = new GenerecRepositorys<ProductFeature>(_context);
            UnitedRepository = new GenerecRepositorys<United>(_context);
            CityRepository = new GenerecRepositorys<City>(_context);
            AddressRepository = new GenerecRepositorys<Address>(_context);
            SliderRepository = new GenerecRepositorys<Slider>(_context);
            WarrantyRepository = new GenerecRepositorys<Warranty>(_context);
            CommentRepository = new GenerecRepositorys<Comment>(_context);
            StartRepository = new GenerecRepositorys<Stars>(_context);
            PointRepository = new GenerecRepositorys<Point>(_context);
            HelpfulRepository = new GenerecRepositorys<Helpful>(_context);
            FavoriteRepository = new GenerecRepositorys<Favorite>(_context);
            DiscountRepository = new GenerecRepositorys<Discount>(_context);
        }

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