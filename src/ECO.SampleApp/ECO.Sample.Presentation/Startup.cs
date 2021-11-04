using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.Impl;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
using ECO.Web.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddECODataContext(Configuration, options =>
            {
                options.RequireTransaction = true;
            });

            //Sample App Services
            services.AddScoped<IEventRepository, EventMemoryRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerMemoryRepository>();
            services.AddScoped<IShowEventsService, ShowEventsService>();
            services.AddScoped<ICreateEventService, CreateEventService>();
            services.AddScoped<IGetEventService, GetEventService>();
            services.AddScoped<IChangeEventService, ChangeEventService>();            
            services.AddScoped<IDeleteEventService, DeleteEventService>();
            services.AddScoped<IAddSessionToEventService, AddSessionToEventService>();
            services.AddScoped<IRemoveSessionFromEventService, RemoveSessionFromEventService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
