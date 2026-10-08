namespace designPattern.Structural;

// 1. The Target Contract (What our modern code expects)
public interface INotificationService
{
    Task SendAsync(string recipient, string message);
}

// 2. The Adaptee (Incompatible 3rd-party vendor SDK)
public class SpeedySmsLegacyApi
{
    public bool DispatchMessageV2(long internationalPhoneNumber, string text, int priorityFlag, bool isFlash)
    {
        Console.WriteLine($"[SpeedySms Gateway] Sent to +{internationalPhoneNumber}: \"{text}\" (Priority: {priorityFlag}, Flash: {isFlash})");
        return true;
    }
}

// 3. The Adapter (Translates between our interface and vendor API)
public class SpeedySmsAdapter : INotificationService
{
    private readonly SpeedySmsLegacyApi _legacyApi;

    public SpeedySmsAdapter(SpeedySmsLegacyApi legacyApi)
    {
        _legacyApi = legacyApi;
    }

    public Task SendAsync(string recipient, string message)
    {
        // 1. Adapt and translate inputs: strip out non-digits
        string cleanPhone = new string(recipient.Where(char.IsDigit).ToArray());

        if (!long.TryParse(cleanPhone, out long parsedPhone))
        {
            throw new ArgumentException($"Invalid phone number format: {recipient}");
        }

        // 2. Map standard call to vendor parameters
        int normalPriority = 1;
        bool isFlash = false;

        bool success = _legacyApi.DispatchMessageV2(parsedPhone, message, normalPriority, isFlash);

        if (!success)
        {
            throw new InvalidOperationException("Failed to dispatch SMS through vendor gateway.");
        }

        return Task.CompletedTask;
    }
}

// Consumer
public class OrderService
{
    private readonly INotificationService _notifier;

    public OrderService(INotificationService notifier)
    {
        _notifier = notifier;
    }

    public async Task CompleteOrderAsync(string customerPhone, decimal amount)
    {
        Console.WriteLine($"Order of ${amount} completed.");
        await _notifier.SendAsync(customerPhone, $"Your order of ${amount} is confirmed!");
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class AdapterExample
{
    public static void Run()
    {
        Console.WriteLine("=== ADAPTER PATTERN ===");

        var legacyApi = new SpeedySmsLegacyApi();
        INotificationService adapter = new SpeedySmsAdapter(legacyApi);

        var orderService = new OrderService(adapter);
        orderService.CompleteOrderAsync("+1 (555) 019-2834", 199.99m).GetAwaiter().GetResult();

        Console.WriteLine();
    }
}
