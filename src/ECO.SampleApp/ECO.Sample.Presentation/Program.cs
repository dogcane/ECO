using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ECO.Integrations.Microsoft.DependencyInjection;
using MediatR;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ECO.Providers.InMemory.Configuration;
using ECO.Providers.Marten.Configuration;
using Weasel.Core;
using Marten;
using Newtonsoft.Json;
using System.Reflection;
using JasperFx.Core;
using Marten.Services.Json;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Text.Json.Serialization;
using ECO.Providers.Marten.Utils;
using ECO.Configuration;
using ECO.Providers.EntityFramework.Configuration;
using Weasel.Core.Migrations;
using ECO.Sample.Infrastructure.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

#if INMEMORY
    //builder.Configuration.AddJsonFile("ecosettings.inmemory.json"); => MOVED TO FLUENT "WAY"    
#elif EFSQL
    //builder.Configuration.AddJsonFile("ecosettings.efcore.sqlserver.json"); => MOVED TO FLUENT "WAY"    
#elif EFMEMORY
    //builder.Configuration.AddJsonFile("ecosettings.efcore.memory.json"); => MOVED TO FLUENT "WAY"    
#elif EFPOSTGRE
    builder.Configuration.AddJsonFile("ecosettings.efcore.postgresql.json");
#elif NHSQL
    builder.Configuration.AddJsonFile("ecosettings.nhibernate.sqlserver.json");
#elif NHPOSTGRE
    builder.Configuration.AddJsonFile("ecosettings.nhibernate.postgresql.json");
#elif MONGODB
    builder.Configuration.AddJsonFile("ecosettings.mongodb.json");
#elif MARTEN
//builder.Configuration.AddJsonFile("ecosettings.marten.json");  => NOT YET SUPPORTED!
#endif


builder.Services.AddControllersWithViews().AddJsonOptions(jopt =>
{
    jopt.JsonSerializerOptions.TypeInfoResolver = new NonPublicContractResolver() { UseParameterlessCtor = false };
});

//ECO            
#if INMEMORY
builder.Services.AddDataContext(options =>
{
    options.UseInMemory(opt =>
    {
        opt.Name = "ecosampleapp.efcore.memory";
        opt.Assemblies = new[] { typeof(ECO.Sample.Domain.AssemblyMarker).Assembly };        
    });
});
#elif EFSQL
builder.Services.AddDataContext(options =>
{
    options.UseEntityFramework<ECOSampleContext>(opt =>
    {
        opt.Name = "ecosampleapp.efcore.sqlserver";
        opt.DbContextOptions
            .UseSqlServer(builder.Configuration.GetConnectionString("sqlserver"));
    });
});
#elif EFMEMORY
builder.Services.AddDataContext(option =>
{
    option.UseEntityFramework<ECOSampleContext>(opt =>
    {
        opt.Name = "ecosampleapp.efcore.memory";
        opt.DbContextOptions
            .UseInMemoryDatabase("ecosampleapp.efcore.memory")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    });
});

#elif MARTEN
builder.Services.AddDataContext(options =>
{
    options.UseMarten(opt =>
    {
        opt.Name = "ecosampleapp.marten";
        opt.Assemblies = new[] { typeof(ECO.Sample.Domain.AssemblyMarker).Assembly };
        opt.StoreOptions.Connection(builder.Configuration.GetConnectionString("marten"));        
        opt.StoreOptions.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
        var serializer = new Marten.Services.SystemTextJsonSerializer
        {
            EnumStorage = EnumStorage.AsString
        };
        serializer.Customize(_ =>
        {
            _.TypeInfoResolver = new NonPublicContractResolver();
            _.ReferenceHandler = ReferenceHandler.IgnoreCycles;            
        });
        opt.StoreOptions.Serializer(serializer);
        /*
        var serializer = new Marten.Services.JsonNetSerializer
        {
            EnumStorage = EnumStorage.AsString
        };
        serializer.Customize(_ =>
        {
            _.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            _.TypeNameHandling = TypeNameHandling.None;
            _.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        serializer.NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters;        
        opt.StoreOptions.Serializer(serializer);
        */
    });
});
#else
builder.Services.AddDataContext(options =>
{
    options.UseConfiguration(builder.Configuration);
});
#endif
//MediatR
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblyContaining<ECO.Sample.Application.AssemblyMarker>();
});
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