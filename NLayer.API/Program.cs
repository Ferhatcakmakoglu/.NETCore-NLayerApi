using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Validator olusturduk ve hatalar� tamn�mlad�k. Global olsun diye options ile tum validate leri gezip aktive ettik
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

//Framework ayarlar�n� degistirmek icin method
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //Framework un default degelen filter ini bask�lad�k. inaktive ettik
    //Bu sayede art�k bizim yazd�g�m�z ValidateFilterAttribute class�ndaki filter methodu cal�sacak
    options.SuppressModelStateInvalidFilter = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Catching islemini tan�mlad�g�m�z yer
builder.Services.AddMemoryCache();


builder.Services.AddScoped(typeof(NotFoundFilter<>));

//IUnitOfWork u ve UnitOfWork Tan�mland�
//Yani Sistem eger IUnitOfWork gorurse UnitOfWork'ten miras
//alacag�n� bilicek

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Burda da IGenericRepository gordugunde GenericRepository e gitmesini saglad�k
//typeof ile yapma sebebimiz bunlar Generic <> oldugundan

//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));


/*
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
*/

/*
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
*/
   
//Bir tane MapProfile class� oldugu icin direkt typeof ile verdik
//Eger Profile s�n�f�ndan tureyen daha fazla MapProfile olsayd�
//Assambly yontemi ile tk sat�rda belirtebiliriz
builder.Services.AddAutoMapper(typeof(MapProfile));


//AppDbContext'i sisteme belirledik
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        /*
            Buray� yapma amac�m�z SqlConnection baglant�s�n�n nerde oldugunu sisteme
            anlatmam�z gerekmektedir. Options methodu ile bunu gerceklestirdik
         */

        //option.MigrationsAssembly("NLayer.Repository");
        //�stte yazd�g�m�z kod ile benzer i�i yapar. Altta yazd�g�m�z daha dinamik.
        //Yani e�er NLayer.repository nin ismi degisirse otomatik olarak bulacakt�r ismi
        //(AppDbContext sayesinde)
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});


//AutoFac etklenti yeri
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Bizim yazd�g�m�z middleware
//Hata class� old icin bu fonk'u koydugumuz sira onemli
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
