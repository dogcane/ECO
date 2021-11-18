using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.Impl;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
using ECO.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECO.Sample.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //ECO
            services.AddECODataContext(Configuration, options =>
            {
                options.RequireTransaction = true;
            });
            //MediatR
            services.AddMediatR(typeof(ECO.Sample.Application.Events.Queries.SearchEvents));
            //Sample App Services
            services.AddScoped<IEventRepository, EventMemoryRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerMemoryRepository>();
        }
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "events",
                    pattern: "{area:exists}/{controller=event}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
