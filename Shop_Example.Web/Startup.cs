using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop_Example.DataLayer.Context;
using Shop_Example.Web.Tools.PersianErrors;
using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Services;
using Shop_Example.Entities.Models;
using Shop_Example.DataLayer;
using Shop_Example.Tools.EmailService;
using Shop_Example.Web.Utilities;
using Shop_Example.DataLayer.Services.Card;
using ExceptionHandling;

namespace Shop_Example.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddControllersWithViews();

#if DEBUG
            builder.AddRazorRuntimeCompilation();
#endif
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DataBaseContext"));
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataBaseContext>()
                .AddDefaultTokenProviders()
                .AddRoles<Role>()
                .AddErrorDescriber<PersianIdentityErrors>();

            services.AddTransient<HandlerOptions>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequiredUniqueChars = 6;
                options.Password.RequireUppercase = false;


                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);


                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            });

            services.ConfigureApplicationCookie(option =>
            {

                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/Login";
                option.ExpireTimeSpan = TimeSpan.FromDays(14);
                option.SlidingExpiration = true;

            });

            services.AddAuthorization(configure =>
            {
                configure.AddPolicy("IsAdminUser", policy =>
                {
                    policy.RequireRole("Admin");
                });
                configure.AddPolicy("IsAdminOrManagerUser", policy =>
                {
                    policy.RequireRole("Admin","Manager");
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICookiesManeger, CookiesManeger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");

                app.UseHsts();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images/ProductImages")
                ),
                RequestPath = "/ProductImages"
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images/ProfileImages")
                    ),
                RequestPath = "/ProfileImages"
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images/SliderImages")
                    ),
                RequestPath = "/SliderImages"
            });

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}");

            });
        }
    }
}
