using CollectibleCardsTradingShopProject;
using CollectibleCardsTradingShopProject.Data;
using CollectibleCardsTradingShopProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CollectibleCardsTradingShopProject.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Session.Keys.Contains("starting"))
            {
                var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();
                var userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
                var roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

                DbInitializer.Initialize(dbContext, userManager, roleManager);

                context.Session.SetString("starting", "Yes");
            }

            await _next(context);
        }
    }
}
