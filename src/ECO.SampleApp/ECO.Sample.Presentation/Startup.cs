using ECO.Configuration;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
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
            services.AddDataContext(options =>
            {
                options.UsingConfiguration(Configuration);
            });

            //MediatR
            services.AddMediatR(typeof(ECO.Sample.Application.AssemblyMarker));
            //Automapper
            services.AddAutoMapper(
                typeof(ECO.Sample.Application.AssemblyMarker),
                typeof(Startup)
            );
            //Sample App Services - InMemory
            //services.AddScoped<IEventRepository, EventMemoryRepository>();
            //services.AddScoped<ISpeakerRepository, SpeakerMemoryRepository>();
            //Sample App Services - EF
            services.AddScoped<IEventRepository, EventEFRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerEFRepository>();
            //Sample App Services - NHibernate
            //services.AddScoped<IEventRepository, EventNHRepository>();
            //services.AddScoped<ISpeakerRepository, SpeakerNHRepository>();
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
                endpoints.MapControllers();
            });
        }
    }
}
