namespace designPattern.Structural;

// 1. Subsystems
public class InventoryService
{
    public bool CheckStock(string sku, int qty) => true;
    public void DeductStock(string sku, int qty) =>
        Console.WriteLine($"[Warehouse] {qty} units of {sku} reserved.");
}

public class PaymentService
{
    public bool ProcessPayment(string cardToken, decimal amount)
    {
        Console.WriteLine($"[Stripe] Successfully charged ${amount}.");
        return true;
    }
}

public class ShippingService
{
    public string GenerateLabel(string address)
    {
        Console.WriteLine($"[Logistics] Shipping label generated for: {address}");
        return "FEDEX-TRACK-99482";
    }
}

public class NotificationService
{
    public void SendReceipt(string email, string tracking) =>
        Console.WriteLine($"[Email] Receipt & Tracking #{tracking} sent to {email}");
}

// 2. Request & Result Models
public class OrderRequest
{
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
}

public class OrderResult
{
    public bool IsSuccess { get; set; }
    public string? TrackingNumber { get; set; }
    public string? ErrorMessage { get; set; }

    public static OrderResult Success(string tracking) =>
        new OrderResult { IsSuccess = true, TrackingNumber = tracking };

    public static OrderResult Failed(string error) =>
        new OrderResult { IsSuccess = false, ErrorMessage = error };
}

// 3. The Facade Interface & Implementation
public interface IOrderProcessingFacade
{
    Task<OrderResult> PlaceOrderAsync(OrderRequest request);
}

public class OrderProcessingFacade : IOrderProcessingFacade
{
    private readonly InventoryService _inventory;
    private readonly PaymentService _payment;
    private readonly ShippingService _shipping;
    private readonly NotificationService _notification;

    public OrderProcessingFacade(
        InventoryService inventory,
        PaymentService payment,
        ShippingService shipping,
        NotificationService notification)
    {
        _inventory = inventory;
        _payment = payment;
        _shipping = shipping;
        _notification = notification;
    }

    public Task<OrderResult> PlaceOrderAsync(OrderRequest request)
    {
        Console.WriteLine("--> [Facade] Orchestrating order placement...");

        if (!_inventory.CheckStock(request.Sku, request.Quantity))
            return Task.FromResult(OrderResult.Failed("Item out of stock."));

        _inventory.DeductStock(request.Sku, request.Quantity);

        var paid = _payment.ProcessPayment(request.PaymentToken, request.TotalAmount);
        if (!paid)
            return Task.FromResult(OrderResult.Failed("Payment declined."));

        var tracking = _shipping.GenerateLabel(request.ShippingAddress);
        _notification.SendReceipt(request.CustomerEmail, tracking);

        return Task.FromResult(OrderResult.Success(tracking));
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class FacadeExample
{
    public static void Run()
    {
        Console.WriteLine("=== FACADE PATTERN ===");

        // Setup subsystems
        var inventory = new InventoryService();
        var payment = new PaymentService();
        var shipping = new ShippingService();
        var notification = new NotificationService();

        // Initialize Facade
        IOrderProcessingFacade facade = new OrderProcessingFacade(inventory, payment, shipping, notification);

        var request = new OrderRequest
        {
            Sku = "LAPTOP-X1",
            Quantity = 1,
            TotalAmount = 1299.99m,
            PaymentToken = "tok_visa_4242",
            ShippingAddress = "123 Tech Blvd, Austin, TX",
            CustomerEmail = "customer@example.com"
        };

        var result = facade.PlaceOrderAsync(request).GetAwaiter().GetResult();
        Console.WriteLine($"Order Processed Successfully? {result.IsSuccess}. Tracking: {result.TrackingNumber}");
        Console.WriteLine();
    }
}
