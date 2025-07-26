using System.Reflection;
using System.Text.Json.Serialization;
using ECO.Configuration;
using ECO.Integrations.Microsoft.DependencyInjection;
using ECO.Providers.EntityFramework.Configuration;
using ECO.Providers.InMemory.Configuration;
using ECO.Providers.Marten.Configuration;
using ECO.Providers.MongoDB.Configuration;
using ECO.Providers.NHibernate.Configuration;
using ECO.Providers.NHibernate.Utils;
using ECO.Providers.Marten.Utils;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.DAL.EntityFramework;
using ECO.Sample.Infrastructure.Repositories;
using JasperFx.Core;
using Marten;
using Marten.Services.Json;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Weasel.Core;
using Weasel.Core.Migrations;
using NHibernate.Properties;

var builder = WebApplication.CreateBuilder(args);

 

builder.Services.AddControllersWithViews().AddJsonOptions(jopt =>
{
    jopt.JsonSerializerOptions.TypeInfoResolver = new NonPublicContractResolver() { UseParameterlessCtor = false };
});

//ECO            
#if INMEMORY
builder.Services.AddDataContext(options =>
{
    options.UseInMemory("ecosampleapp.efcore.memory", opt => opt.AddAssemblyFromType<ECO.Sample.Domain.AssemblyMarker>());            
});
#elif EFSQL
builder.Services.AddDataContext(options =>
{
    options.UseEntityFramework<ECOSampleContext>("ecosampleapp.efcore.sqlserver", opt => opt.DbContextOptions.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")));
});
#elif EFPOSTGRE
builder.Services.AddDataContext(options =>
{
    options.UseEntityFramework<ECOSampleContext>("ecosampleapp.efcore.postgresql", opt => opt.DbContextOptions.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));
});
#elif EFMEMORY
builder.Services.AddDataContext(option =>
{
    option.UseEntityFramework<ECOSampleContext>("ecosampleapp.efcore.memory", opt =>
    {
        opt.DbContextOptions
            .UseInMemoryDatabase("ecosampleapp.efcore.memory")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    });
});
#elif NHSQL
builder.Services.AddDataContext(options =>
{
    options.UseNHibernate("ecosampleapp.nh.sqlserver", opt =>
    {
        opt.Configuration.DataBaseIntegration(config =>
        {
            config.ConnectionString = builder.Configuration.GetConnectionString("sqlserver");
            config.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
            config.Driver<NHibernate.Driver.SqlClientDriver>();
        }).AddAssemblyExtended(typeof(ECO.Sample.Infrastructure.DAL.NHibernate.AssemblyMarker).Assembly);
    });
});
#elif NHPOSTGRE
builder.Services.AddDataContext(options =>
{
    options.UseNHibernate("ecosampleapp.nh.postgresql", opt =>
    {
        opt.Configuration.DataBaseIntegration(config =>
        {
            config.ConnectionString = builder.Configuration.GetConnectionString("postgres");
            config.Dialect<NHibernate.Dialect.PostgreSQL83Dialect>();
            config.Driver<NHibernate.Driver.NpgsqlDriver>();
        })
        .SetNamingStrategy(new ECO.Sample.Infrastructure.DAL.NHibernate.Utils.PostgreSQLNamingStrategy())
        .AddAssemblyExtended(typeof(ECO.Sample.Infrastructure.DAL.NHibernate.AssemblyMarker).Assembly)
        .SetProperty("default_schema", "public");
    });
});
#elif MARTEN
builder.Services.AddDataContext(options =>
{
    options.UseMarten("ecosampleapp.marten", opt =>
    {
        opt.AddAssemblyFromType<ECO.Sample.Domain.AssemblyMarker>();
        opt.StoreOptions.Connection(builder.Configuration.GetConnectionString("marten"));        
        opt.StoreOptions.AutoCreateSchemaObjects = JasperFx.AutoCreate.CreateOrUpdate;
        var serializer = new Marten.Services.SystemTextJsonSerializer
        {
            EnumStorage = EnumStorage.AsString
        };
        serializer.Configure(_ =>
        {
            _.TypeInfoResolver = new NonPublicContractResolver();
            _.ReferenceHandler = ReferenceHandler.IgnoreCycles;            
        });
        opt.StoreOptions.Serializer(serializer);
    });
});
#elif MONGODB
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
    new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)
);
builder.Services.AddDataContext(options =>
{
    options.UseMongoDB("ecosampleapp.mongodb", opt =>
    {
        opt.ConnectionString = builder.Configuration.GetConnectionString("mongo");
        opt.DatabaseName = "ECOSampleApp";
        opt.AddAssemblyFromType<ECO.Sample.Domain.AssemblyMarker>();
        opt.MappingAssemblies = "ECO.Sample.Infrastructure.DAL.MongoDB";
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