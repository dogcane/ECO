using ECO.Data;
using ECO.Providers.Marten;
using ECO.Sample.EventSourcing.Domain;
using ECO.Sample.EventSourcing.Infrastructure.Marten;
using Marten;
using Marten.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Weasel.Core;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

var documentStore = DocumentStore.For(opt =>
{
    opt.Connection("Server=127.0.0.1;Port=5432;Database=ECOSampleES_MT;User Id=ecosample;Password=ecosample;");
    opt.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
    opt.Projections.SelfAggregate<Order>();    
    opt.Events.StreamIdentity = StreamIdentity.AsString;
    var serializer = new Marten.Services.JsonNetSerializer()
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
    opt.Serializer(serializer);
});
var martenPersistenceUnit = new MartenPersistenceUnit("eco.samplees.marten", loggerFactory, documentStore);
martenPersistenceUnit.AddClass<Order, string>();
var persistenceUnitFactory = new PersistenceUnitFactory();
persistenceUnitFactory.AddPersistenceUnit(martenPersistenceUnit);


documentStore.Advanced.Clean.CompletelyRemoveAll();
var orderId = "ORD-1234";


using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = Order.CreateNew(orderId, "Test", "Akronimo", "Via Sesia 5");
    order?.AddDetail(1234, "My fabolous phone", 1, 300f);
    order?.AddDetail(5678, "My fabolous phone's cover", 2, 10f);
    repository.Save(order);
    ctx.SaveChanges();
    tx.Commit();
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    PrintOrder("Orders Created", order);
    ctx.SaveChanges();
    tx.Commit();
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    order?.RemoveDetail(5678, 1);
    repository.Save(order);
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    PrintOrder("Order item removed", order);
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    order?.Prepare();
    repository.Save(order);
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    PrintOrder("Order prepared", order);
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    order?.Ship();
    repository.Save(order);
}

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var order = repository.Load(orderId);
    PrintOrder("Order shipped", order);
}


Console.WriteLine("###########################################");
Console.WriteLine("Event Sourcing : Time Travelling...........");
Console.WriteLine("###########################################");

using (var ctx = persistenceUnitFactory.OpenDataContext())
using (var tx = ctx.BeginTransaction())
{
    var repository = new MartenOrderRepository(ctx);
    var events = repository.LoadEvents(orderId);
    PrintEvents("Event stream", events);
}


void PrintOrder(string message, Order order)
{
    Console.WriteLine("###########################################");
    Console.WriteLine($"{message}");
    Console.WriteLine("###########################################");
    Console.WriteLine($"{order}");
    foreach (var item in order!.Items)
    {
        Console.WriteLine($"{item}");
    }
}

void PrintEvents(string message, IEnumerable<dynamic> events)
{
    Console.WriteLine("###########################################");
    Console.WriteLine($"{message}");
    Console.WriteLine("###########################################");
    foreach (dynamic @event in events)
    {
        Console.WriteLine($"{@event}");
    }
}
