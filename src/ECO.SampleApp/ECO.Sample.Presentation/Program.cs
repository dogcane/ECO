using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ECO.Sample.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)  
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("ecosettings.inmemory.json");
                    //config.AddJsonFile("ecosettings.efcore.sqlserver.json");
                    //config.AddJsonFile("ecosettings.efcore.memory.json");
                    //config.AddJsonFile("ecosettings.nhibernate.json");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
