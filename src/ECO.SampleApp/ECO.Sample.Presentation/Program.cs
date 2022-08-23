using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ECO.Configuration;
using ECO.Integrations.Microsoft.DependencyInjection;
using MediatR;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration(config =>
{
#if INMEMORY
    config.AddJsonFile("ecosettings.inmemory.json");
#elif EFSQL
    config.AddJsonFile("ecosettings.efcore.sqlserver.json");
#elif EFMEMORY
    config.AddJsonFile("ecosettings.efcore.memory.json");
#elif NH
    config.AddJsonFile("ecosettings.nhibernate.json");
#endif
});

builder.Services.AddControllersWithViews();
//ECO            
builder.Services.AddDataContext(options =>
{
    options.UsingConfiguration(builder.Configuration);
});
//MediatR
builder.Services.AddMediatR(typeof(ECO.Sample.Application.AssemblyMarker));
//Automapper
builder.Services.AddAutoMapper(
    typeof(ECO.Sample.Application.AssemblyMarker),
    typeof(Program)
);
#if INMEMORY
builder.Services.AddScoped<IEventRepository, EventMemoryRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerMemoryRepository>();
#elif EFSQL || EFMEMORY
builder.Services.AddScoped<IEventRepository, EventEFRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerEFRepository>();
#elif NH
builder.Services.AddScoped<IEventRepository, EventNHRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerNHRepository>();
#endif

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();