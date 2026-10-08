using System.Text.Json;

namespace designPattern.Enterprise;

// 1. Entities
public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedOnUtc { get; set; }
}

public class OrderAggregate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public record OrderPlacedDomainEvent(Guid OrderId, string CustomerEmail, decimal Amount);

// 2. Simulated Transactional Database
public class TransactionalDatabase
{
    public List<OrderAggregate> Orders { get; } = new();
    public List<OutboxMessage> OutboxMessages { get; } = new();

    public void CommitOrderWithOutbox(OrderAggregate order, object domainEvent)
    {
        // ATOMIC LOCAL TRANSACTION: Both Order and Outbox Message written in the same transaction
        Orders.Add(order);

        var outboxMessage = new OutboxMessage
        {
            Type = domainEvent.GetType().Name,
            Content = JsonSerializer.Serialize(domainEvent)
        };
        OutboxMessages.Add(outboxMessage);

        Console.WriteLine($"  [SQL-TRANSACTION] Atomically committed Order '{order.Id.ToString()[..8]}' and OutboxMessage '{outboxMessage.Id.ToString()[..8]}'");
    }
}

// 3. Simulated Message Broker (RabbitMQ / Kafka)
public interface IMessageBus
{
    void Publish(string messageType, string payload);
}

public class ConsoleMessageBus : IMessageBus
{
    public void Publish(string messageType, string payload)
    {
        Console.WriteLine($"  🚀 [RABBITMQ-BUS] Broadcasted '{messageType}': {payload}");
    }
}

// 4. Background Outbox Worker (Poller)
public class OutboxProcessor
{
    private readonly TransactionalDatabase _db;
    private readonly IMessageBus _bus;

    public OutboxProcessor(TransactionalDatabase db, IMessageBus bus)
    {
        _db = db;
        _bus = bus;
    }

    public void ProcessUnpublishedMessages()
    {
        Console.WriteLine("\n[OUTBOX-WORKER] Polling database for unprocessed outbox messages...");

        var pendingMessages = _db.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.CreatedOnUtc)
            .ToList();

        if (pendingMessages.Count == 0)
        {
            Console.WriteLine("  [OUTBOX-WORKER] No pending messages.");
            return;
        }

        foreach (var msg in pendingMessages)
        {
            // Publish to external message broker
            _bus.Publish(msg.Type, msg.Content);

            // Mark as processed in local database
            msg.ProcessedOnUtc = DateTime.UtcNow;
            Console.WriteLine($"  [SQL-UPDATE] Outbox message '{msg.Id.ToString()[..8]}' marked as ProcessedOnUtc: {msg.ProcessedOnUtc:HH:mm:ss.fff}");
        }
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class TransactionalOutboxExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 7: The Transactional Outbox Pattern ---");

        var db = new TransactionalDatabase();
        var bus = new ConsoleMessageBus();
        var worker = new OutboxProcessor(db, bus);

        // Step 1: User places order (Atomic write in SQL, zero network call to RabbitMQ here!)
        Console.WriteLine("\n1. Business service places an order within a SQL Transaction:");
        var order = new OrderAggregate { CustomerEmail = "john.doe@enterprise.com", Amount = 299.99m };
        var domainEvent = new OrderPlacedDomainEvent(order.Id, order.CustomerEmail, order.Amount);

        db.CommitOrderWithOutbox(order, domainEvent);

        // Step 2: Background worker polls and publishes guaranteed messages
        Console.WriteLine("\n2. Background processor executes (guaranteed at-least-once delivery):");
        worker.ProcessUnpublishedMessages();

        // Step 3: Subsequent run finds nothing
        worker.ProcessUnpublishedMessages();
    }
}
