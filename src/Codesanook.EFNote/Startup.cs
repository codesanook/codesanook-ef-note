using System;
using Codesanook.EFNote.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Codesanook.EFNote
{
    public class Startup
    {
        // !!! Note that appsettings.json will be registered by default in .NET Core 2.0.
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddSessionStateTempDataProvider();

            // EF context objects should be scoped for a per-request lifetime.
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<NoteDbContext>(option =>
            {
                option
                    .UseSqlServer(connectionString)
                    .EnableSensitiveDataLogging()
                    .UseSnakeCaseNamingConvention();
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();// IMPORTANT: This session call MUST go before UseMvc()

            app.UseEndpoints(endpoints =>
            {
                // Default to Note/index
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Note}/{action=Index}/{id?}"
                );
            });
        }
    }
}
