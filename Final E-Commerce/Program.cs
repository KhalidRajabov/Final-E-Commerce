using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Mapper;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;


// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IgnoreNullValues = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile(new MapperProfile());
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
