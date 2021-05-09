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
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // Note that appsettings.json will be registered by default in .NET Core 2.0. 
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddSessionStateTempDataProvider();

            // EF context objects should be scoped for a per-request lifetime.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NoteDbContext>(option =>
            {
                option
                    .UseNpgsql(connectionString)
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
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<NoteDbContext>();
                context.Database.Migrate();
            }

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
            // TODO temporary remove https://stackoverflow.com/questions/50935730/how-do-i-disable-https-in-asp-net-core-2-1-kestrel
            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();// IMPORTANT: This session call MUST go before UseMvc()

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Note}/{action=Index}/{id?}"
                );
            });
        }
    }
}
