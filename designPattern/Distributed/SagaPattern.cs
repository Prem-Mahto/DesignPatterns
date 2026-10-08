namespace designPattern.Distributed;

// 1. Saga Step Interfaces
public interface IPaymentService
{
    Guid Charge(Guid orderId, decimal amount);
    void Refund(Guid paymentId, decimal amount);
}

public interface IInventoryService
{
    bool ReserveStock(Guid orderId, string sku);
    void ReleaseStock(Guid orderId, string sku);
}

// 2. Concrete Services
public class MockPaymentService : IPaymentService
{
    public Guid Charge(Guid orderId, decimal amount)
    {
        var paymentId = Guid.NewGuid();
        Console.WriteLine($"  💳 [PAYMENT SERVICE] Successfully charged ${amount} (Payment ID: {paymentId.ToString()[..8]})");
        return paymentId;
    }

    public void Refund(Guid paymentId, decimal amount)
    {
        Console.WriteLine($"  🔄 [COMPENSATION - PAYMENT] Refunded ${amount} for Payment ID: {paymentId.ToString()[..8]}");
    }
}

public class MockInventoryService : IInventoryService
{
    private readonly bool _shouldStockFail;
    public MockInventoryService(bool shouldStockFail) => _shouldStockFail = shouldStockFail;

    public bool ReserveStock(Guid orderId, string sku)
    {
        if (_shouldStockFail)
        {
            Console.WriteLine($"  ❌ [INVENTORY SERVICE] Stock reservation FAILED for SKU: '{sku}' (Warehouse out of stock!)");
            return false;
        }

        Console.WriteLine($"  📦 [INVENTORY SERVICE] Stock reserved for SKU: '{sku}'");
        return true;
    }

    public void ReleaseStock(Guid orderId, string sku)
    {
        Console.WriteLine($"  🔄 [COMPENSATION - INVENTORY] Released reserved stock for SKU: '{sku}'");
    }
}

// 3. The Saga Orchestrator
public class OrderSagaOrchestrator
{
    private readonly IPaymentService _paymentService;
    private readonly IInventoryService _inventoryService;

    public OrderSagaOrchestrator(IPaymentService paymentService, IInventoryService inventoryService)
    {
        _paymentService = paymentService;
        _inventoryService = inventoryService;
    }

    public bool ExecuteOrderSaga(Guid orderId, decimal amount, string sku)
    {
        Console.WriteLine($"\n[SAGA-START] Beginning Distributed Saga for Order '{orderId.ToString()[..8]}'");

        Guid? paymentId = null;

        try
        {
            // Step 1: Charge customer
            paymentId = _paymentService.Charge(orderId, amount);

            // Step 2: Reserve inventory
            bool stockReserved = _inventoryService.ReserveStock(orderId, sku);
            if (!stockReserved)
            {
                throw new InvalidOperationException("StockReservationFailedException");
            }

            Console.WriteLine($"✅ [SAGA-COMPLETE] Distributed transaction succeeded across all microservices for Order '{orderId.ToString()[..8]}'\n");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n🚨 [SAGA-ABORT] Failure detected: '{ex.Message}'. Executing COMPENSATING TRANSACTIONS backwards...");

            // Compensate Step 1 if it had completed
            if (paymentId.HasValue)
            {
                _paymentService.Refund(paymentId.Value, amount);
            }

            Console.WriteLine($"❌ [SAGA-COMPENSATED] All intermediate state undone. Order marked as Cancelled.\n");
            return false;
        }
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class SagaExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- Tier 2 Pattern 1: The Saga Pattern (Distributed Transactions) ---");

        var paymentService = new MockPaymentService();

        // Scenario 1: Happy Path
        Console.WriteLine("Scenario 1: Happy path where all microservices succeed:");
        var happyInventory = new MockInventoryService(shouldStockFail: false);
        var happySaga = new OrderSagaOrchestrator(paymentService, happyInventory);
        happySaga.ExecuteOrderSaga(Guid.NewGuid(), 199.99m, "SKU-LAPTOP-X1");

        // Scenario 2: Failure with Compensating Transactions
        Console.WriteLine("Scenario 2: Inventory fails, triggering backwards compensation:");
        var failingInventory = new MockInventoryService(shouldStockFail: true);
        var failingSaga = new OrderSagaOrchestrator(paymentService, failingInventory);
        failingSaga.ExecuteOrderSaga(Guid.NewGuid(), 49.99m, "SKU-HEADPHONES");
    }
}
