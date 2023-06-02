using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.Web.Modules;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using NLayer.Service.Mapping;
using System.Reflection;
using FluentValidation.AspNetCore;
using NLayer.Service.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); ;


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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
