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
        public Startup(IConfiguration configuration) => this.configuration = configuration;

        // !!! Note that appsettings.json will be registered by default in .NET Core 2.0.
        private IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddSessionStateTempDataProvider();

            // EF context objects should be scoped for a per-request lifetime.
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            var serverVersion = new MySqlServerVersion(new Version(5, 7, 0));

            services.AddDbContext<NoteDbContext>(option =>
            {
                    option
                        .UseMySql(connectionString, serverVersion)
                        .UseSnakeCaseNamingConvention();

                // if (env.IsDevelopment())
                // {
                //     option
                //         .UseMySql(connectionString, serverVersion)
                //         .UseSnakeCaseNamingConvention();
                // }
                // else
                // {
                //     option.UseInMemoryDatabase(databaseName: "ef-note");
                // }
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
                // Auto run database migration when start a website
                if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    context.Database.Migrate();
                }
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
                // Default to Note/index
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Note}/{action=Index}/{id?}"
                );
            });
        }
    }
}
