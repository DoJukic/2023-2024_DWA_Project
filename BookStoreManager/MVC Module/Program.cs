using DBScaffold;
using DBScaffold.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVC_Module.Systems;

namespace MVC_Module
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // WATCH FOR DOUBLE "\"!
            // dotnet ef dbcontext scaffold "name=ConnectionStrings:DB" Microsoft.EntityFrameworkCore.SqlServer -o Models --force
            builder.Services.AddDbContext<DwaContext>(options => {
                options.UseSqlServer("name=ConnectionStrings:DB");
            });

            BookReservationSystem.Configure(
                builder.Configuration.GetValue<string>("ConnectionStrings:DB"),
                builder.Configuration.GetValue<int>("StandardExpirationTimeDays")
                );

            // Ask prof. what this is for? Commenting out in exercise solution does nothing.
            builder.Services.AddDistributedMemoryCache();

            // Cookie based logins. Much prefer session storage stuff tbh
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.LogoutPath = "/User/Logout";
                    options.AccessDeniedPath = "/User/Forbidden";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
