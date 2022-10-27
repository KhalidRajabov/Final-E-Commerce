using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Final_E_Commerce.DAL;
using Final_E_Commerce.MiddlewareExtensions;
using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.Identity;
using Final_E_Commerce.Helper;
using Final_E_Commerce.Hubs;
using Final_E_Commerce.SubscribeTableDependency;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddSignalR(c =>
{
    c.EnableDetailedErrors = true;
    c.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    c.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<SubscribeProductTableDependency>();
builder.Services.AddSingleton<PendingsHub>();
builder.Services.AddSingleton<SubscribePendingsTableDependency>();
builder.Services.AddSingleton<MessagesHub>();
builder.Services.AddSingleton<SubscribeMessagesTableDependency>();

builder.Services.AddAuthentication()
    .AddFacebook(opt =>
    {
        opt.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        opt.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    })
    .AddGoogle(opts =>
{
    opts.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    opts.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    
});
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn("UserId", System.Data.SqlDbType.VarChar)
    }
};
/*Logger log = new LoggerConfiguration()
    .
    .CreateLogger();
builder.Host.UseSerilog(log);*/
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IgnoreNullValues = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(20);
});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;

    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.Lockout.AllowedForNewUsers = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);



}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders().AddErrorDescriber<RegisterErrorMessages>();
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


app.MapHub<DashboardHub>("/dashboardHub");
app.MapHub<PendingsHub>("/pendingsHub");
app.MapHub<MessagesHub>("/messagesHub");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseProductTableDependency();
app.UsePendingsTableDependency();
app.UseMessagesTableDependency();
app.Run();
