using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Final_E_Commerce.MiddlewareExtensions
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using(var scope = httpContext.RequestServices.CreateScope())
            {
                AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var routeData = httpContext.GetRouteData();

                var log = new AuditLog();
                log.CreatedDate = log.RequestTime = DateTime.Now.AddHours(12);
                log.IsHttps=httpContext.Request.IsHttps;
                log.Path = httpContext.Request.Path;
                
                if (routeData.Values.TryGetValue("area", out object area)){
                    log.Area = area.ToString();
                }
                if (routeData.Values.TryGetValue("controller", out object controller))
                {
                    log.Controller = controller.ToString();
                }
                if (routeData.Values.TryGetValue("action", out object action))
                {
                    log.Action = action.ToString();
                }
                if (!string.IsNullOrWhiteSpace(httpContext.Request.QueryString.Value))
                {
                    log.QueryString = httpContext.Request.QueryString.Value;
                }
                await _next(httpContext);
                log.StatusCode=httpContext.Response.StatusCode;
                log.RespondTime= DateTime.Now.AddHours(12);

                await db.AuditLog.AddAsync(log);
                db.SaveChangesAsync();
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuditMiddlewareExtensions
    {
        public static IApplicationBuilder UseAudit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddleware>();
        }
    }
}
