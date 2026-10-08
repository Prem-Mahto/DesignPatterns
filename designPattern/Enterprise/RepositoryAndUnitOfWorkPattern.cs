namespace designPattern.Enterprise;

// 1. Domain Entities
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal LoyaltyPoints { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
}

// 2. Domain Repository Interfaces
public interface ICustomerRepository
{
    Customer? GetById(int id);
    void Update(Customer customer);
}

public interface IOrderRepository
{
    void Add(Order order);
    IEnumerable<Order> GetByCustomerId(int customerId);
}

// 3. Unit of Work Interface (Atomic Transaction Management)
public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    int SaveChanges();
    void Rollback();
}

// 4. Concrete Implementation (Simulating an ORM / DbContext)
public class MockDbContext
{
    public List<Customer> Customers { get; } = new()
    {
        new Customer { Id = 1, Name = "Alice", LoyaltyPoints = 250 },
        new Customer { Id = 2, Name = "Bob", LoyaltyPoints = 50 }
    };
    public List<Order> Orders { get; } = new();

    public int SaveChanges()
    {
        Console.WriteLine("  [DB-COMMIT] Flushed all changes to database in a single atomic transaction.");
        return 1;
    }
}

public class CustomerRepository : ICustomerRepository
{
    private readonly MockDbContext _db;
    public CustomerRepository(MockDbContext db) => _db = db;

    public Customer? GetById(int id) => _db.Customers.FirstOrDefault(c => c.Id == id);
    public void Update(Customer customer) => Console.WriteLine($"  [REPO-CUSTOMER] Updated customer {customer.Name} loyalty points: {customer.LoyaltyPoints}");
}

public class OrderRepository : IOrderRepository
{
    private readonly MockDbContext _db;
    public OrderRepository(MockDbContext db) => _db = db;

    public void Add(Order order)
    {
        _db.Orders.Add(order);
        Console.WriteLine($"  [REPO-ORDER] Staged new order for customer {order.CustomerId}, Amount: ${order.TotalAmount}");
    }

    public IEnumerable<Order> GetByCustomerId(int customerId) => _db.Orders.Where(o => o.CustomerId == customerId);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly MockDbContext _db = new();
    private ICustomerRepository? _customers;
    private IOrderRepository? _orders;

    public ICustomerRepository Customers => _customers ??= new CustomerRepository(_db);
    public IOrderRepository Orders => _orders ??= new OrderRepository(_db);

    public int SaveChanges() => _db.SaveChanges();
    public void Rollback() => Console.WriteLine("  [DB-ROLLBACK] Reverted staged changes due to an error.");
    public void Dispose() => GC.SuppressFinalize(this);
}

// 5. Business Service (Decoupled from direct EF Core DbContext)
public class OrderService
{
    private readonly IUnitOfWork _uow;
    public OrderService(IUnitOfWork uow) => _uow = uow;

    public bool PlaceOrder(int customerId, decimal orderAmount)
    {
        Console.WriteLine($"\n[SERVICE] Processing order for Customer ID: {customerId} (Amount: ${orderAmount})");

        var customer = _uow.Customers.GetById(customerId);
        if (customer == null)
        {
            Console.WriteLine("  [ERROR] Customer not found!");
            return false;
        }

        // Deduct points
        customer.LoyaltyPoints -= 20;
        _uow.Customers.Update(customer);

        // Add order
        var order = new Order { CustomerId = customerId, TotalAmount = orderAmount };
        _uow.Orders.Add(order);

        // Commit atomically via Unit of Work
        _uow.SaveChanges();
        return true;
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class RepositoryAndUnitOfWorkExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 1: Repository & Unit of Work ---");

        using var uow = new UnitOfWork();
        var orderService = new OrderService(uow);

        orderService.PlaceOrder(1, 149.99m);
        orderService.PlaceOrder(99, 50.00m); // Non-existent customer
    }
}
