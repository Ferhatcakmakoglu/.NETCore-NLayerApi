using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    //Burada hem GenericRepo yu hemde Product a ozel interfaceyi implement ettik
    //GenericRepoyu diger islemlere erisebilmek icin ve context nesnesine ulasabilmek icin cektik
    //Bu durum bize sadece Product Icın olusturdugumuz methodun gelmesine yaradı 
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<Product>> GetProductsWithCategory()
        {
            //Eager Loading islemi yapıldı
            //Yani data cekilirken aynı zamanda kategorilerin de alınması istendi
            
            //Context nesnesi GenericRepo da protected old. icin kullanabiliriz!
            return _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
