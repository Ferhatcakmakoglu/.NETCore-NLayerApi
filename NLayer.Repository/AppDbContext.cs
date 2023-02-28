using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Repository.Configurations;
using System.Reflection;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products{ get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            //Ustte yazdıgımız sadece ProductConfigurations u derler
            //Bu method ile yazdıgımızda ise tum Configuration lar derlenecek (hepsi)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Seed's islemlerini burdan da yapabiliriz. Fakat bu AppDbContext dosyası kirlenmemesi icin
            //Burada yapmak BestPracties e uymaz. Fakat yazdıgımız bu deneme ProductFeature ozellikleri de calisir
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature
            {
                Id = 1,
                Color="Kırmızı",
                Width = 200,
                Height= 100,
                ProductId= 1
            },
            new ProductFeature
            {
                Id = 2,
                Color = "Mavi",
                Width = 500,
                Height = 300,
                ProductId = 2
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
