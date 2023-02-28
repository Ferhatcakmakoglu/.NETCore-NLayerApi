using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//IUnitOfWork u ve UnitOfWork Tanýmlandý
//Yani Sistem eger IUnitOfWork gorurse UnitOfWork'ten miras
//alacagýný bilicek
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Burda da IGenericRepository gordugunde GenericRepository e gitmesini sagladýk
//typeof ile yapma sebebimiz bunlar Generic <> oldugundan
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

   
//Bir tane MapProfile classý oldugu icin direkt typeof ile verdik
//Eger Profile sýnýfýndan tureyen daha fazla MapProfile olsaydý
//Assambly yontemi ile tk satýrda belirtebiliriz
builder.Services.AddAutoMapper(typeof(MapProfile));


//AppDbContext'i sisteme belirledik
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        /*
            Burayý yapma amacýmýz SqlConnection baglantýsýnýn nerde oldugunu sisteme
            anlatmamýz gerekmektedir. Options methodu ile bunu gerceklestirdik
         */

        //option.MigrationsAssembly("NLayer.Repository");
        //Üstte yazdýgýmýz kod ile benzer iþi yapar. Altta yazdýgýmýz daha dinamik.
        //Yani eðer NLayer.repository nin ismi degisirse otomatik olarak bulacaktýr ismi
        //(AppDbContext sayesinde)
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
