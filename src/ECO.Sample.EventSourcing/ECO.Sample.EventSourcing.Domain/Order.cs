using ECO.EventSourcing;
using ECO.Sample.EventSourcing.Events;

namespace ECO.Sample.EventSourcing.Domain;

public class Order : ESAggregateRoot<string>
{
    #region Fields

    private List<OrderDetail> items = new List<OrderDetail>();

    #endregion

    #region Properties

    public string Id
    {
        get => Identity;
        set => Identity = value;
    }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Address { get; set; }

    public IEnumerable<OrderDetail> Items => items;        

    public float Total => Items.Sum(item => item.TotalAmount);

    public OrderStatus Status { get; set; }

    #endregion

    #region Ctor

    private Order() : base() { }

    #endregion

    #region Public Methods

    public static Order? CreateNew(string id, string name, string surname, string address)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(address))
            return null;

        var order = new Order();
        var @event = new OrderCreated(id, name, surname, address);
        order.OnApply(@event, order.Apply);
        return order;
    }

    public bool AddDetail(int sku, string description, int quantity, float amount)
    {
        if (sku <= 0 || string.IsNullOrEmpty(description) || quantity <= 0)
            return false;

        if (Status != OrderStatus.New)
            return false;

        var @event = new OrderDetailAdded(Id, sku, description, quantity, amount);
        OnApply(@event, Apply);
        return true;
    }

    public bool RemoveDetail(int sku, int quantity)
    {
        if (sku <= 0 || quantity <= 0)
            return false;

        if (Status != OrderStatus.New)
            return false;

        var @event = new OrderDetailRemoved(Id, sku, quantity);
        OnApply(@event, Apply);
        return true;
    }

    public bool Prepare()
    {
        if (Status != OrderStatus.New)
            return false;

        var @event = new OrderPrepared(Id);
        OnApply(@event, Apply);
        return true;
    }

    public bool Ship()
    {
        if (Status != OrderStatus.Prepared)
            return false;

        var @event = new OrderShipped(Id);
        OnApply(@event, Apply);
        return true;
    }

    public override string ToString() => $"Order {Id} - Status : {Status} - Items : {Items.Sum(item => item.Quantity)} - Total Amount : {Total} - For : {Name} {Surname}";

    #endregion

    #region ES_Management    

    private void Apply(OrderCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Surname = @event.Surname;
        Address = @event.Address;
        Status = OrderStatus.New;
    }

    protected void Apply(OrderDetailAdded @event)
    {
        var currentItem = items.Where(it => it.Sku == @event.Sku).FirstOrDefault();
        if (currentItem != null)
        {
            currentItem.Quantity += @event.Quantity;
        }
        else
        {
            var detail = new OrderDetail
            {
                Sku = @event.Sku,
                Amount = @event.Amount,
                Description = @event.Description,
                Quantity = @event.Quantity
            };
            items.Add(detail);
        }
    }

    private void Apply(OrderDetailRemoved @event)
    {
        var currentItem = items.Where(it => it.Sku == @event.Sku).FirstOrDefault();
        if (currentItem != null)
        {
            currentItem.Quantity -= @event.Quantity;
            if (currentItem.Quantity < 0)
                items.Remove(currentItem);
        }
    }

    private void Apply(OrderPrepared @event)
    {
        Status = OrderStatus.Prepared;
    }

    private void Apply(OrderShipped @event)
    {
        Status = OrderStatus.Shipped;
    }

    #endregion
    
}

