using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;
using ShopDemo.Infrastructure;
using ShopDemo.Infrastructure.Repositories;
using ShopDemo.Web.Extensions;
using ShopDemo.Web.Seeder;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddAppConfigurations();
builder.Logging.ClearProviders();
builder.Services.AddSingleton<ILoggerProvider>(sp =>
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Filter.ByExcluding(logEvent => logEvent.Exception != null)
        .CreateLogger();

    return new SerilogLoggerProvider(logger, dispose: true);
});

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Account/Login";
    });
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), 1024);
builder.Services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), typeof(Logger<Program>));

builder.Services.AddScoped<IDbFactory, DbFactoryData>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandlerCustom(app.Logger);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MigrateDatabase<ApplicationDbContext>((context, _) =>
{
    DataSeeder.InitData(context, app.Logger).Wait();
});

app.Run();
