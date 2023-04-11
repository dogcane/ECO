using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ECO.Configuration;
using ECO.Integrations.Microsoft.DependencyInjection;
using MediatR;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ECO.Providers.Marten.Configuration;
using ECO.Providers.InMemory.Configuration;
using Weasel.Core;
using Marten;
using Newtonsoft.Json;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration(config =>
{
#if INMEMORY
    //config.AddJsonFile("ecosettings.inmemory.json"); => MOVED TO FLUENT "WAY"
#elif EFSQL
    config.AddJsonFile("ecosettings.efcore.sqlserver.json");
#elif EFMEMORY
    config.AddJsonFile("ecosettings.efcore.memory.json");
#elif EFPOSTGRE
    config.AddJsonFile("ecosettings.efcore.postgresql.json");
#elif NHSQL
    config.AddJsonFile("ecosettings.nhibernate.sqlserver.json");
#elif NHPOSTGRE
    config.AddJsonFile("ecosettings.nhibernate.postgresql.json");
#elif MONGODB
    config.AddJsonFile("ecosettings.mongodb.json");
#elif MARTEN
    //config.AddJsonFile("ecosettings.marten.json");  => NOT YET SUPPORTED!
#endif
});

builder.Services.AddControllersWithViews();

//ECO            
#if INMEMORY
builder.Services.AddDataContext(options =>
{
    options.UseInMemory(opt =>
    {
        opt.Name = "ecosampleapp.efcore.memory";
        opt.Classes = new[]
        {
            typeof(Event),
            typeof(Speaker)
        };
    }, builder.Configuration);
});
#elif MARTEN
builder.Services.AddDataContext(options =>
{
    options.UseMarten(opt =>
    {
        opt.Name = "ecosampleapp.marten";
        opt.Classes = new[]
        {
            typeof(Event),
            typeof(Speaker)
        };
        opt.StoreOptions.Connection(builder.Configuration.GetConnectionString("marten"));        
        opt.StoreOptions.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
        opt.StoreOptions.Schema.For<Event>().Identity(ent => ent.Identity);
        opt.StoreOptions.Schema.For<Speaker>().Identity(ent => ent.Identity);
        var serializer = new Marten.Services.JsonNetSerializer();        
        serializer.EnumStorage = EnumStorage.AsString;
        serializer.Customize(_ =>
        {
            _.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            _.TypeNameHandling = TypeNameHandling.None;
            _.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        serializer.NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters;        
        opt.StoreOptions.Serializer(serializer);
    }, builder.Configuration);
});
#else
builder.Services.AddDataContext(options =>
{
    options.UsingConfiguration(builder.Configuration);
});
#endif
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
#elif EFSQL || EFMEMORY || EFPOSTGRE
builder.Services.AddScoped<IEventRepository, EventEFRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerEFRepository>();
#elif NHSQL || NHPOSTGRE
builder.Services.AddScoped<IEventRepository, EventNHRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerNHRepository>();
#elif MONGODB
builder.Services.AddScoped<IEventRepository, EventMongoRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerMongoRepository>();
#elif MARTEN
builder.Services.AddScoped<IEventRepository, EventMartenRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerMartenRepository>();
//builder.Services.AddRepository<Event, Guid>(); => WIP
//builder.Services.AddRepository<Speaker, Guid>(); => WIP
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