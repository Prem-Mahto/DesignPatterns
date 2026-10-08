namespace designPattern.Behavioral;

// 1. Request Context
public class OrderPipelineRequest
{
    public string Token { get; set; } = string.Empty;
    public string UserIp { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

// 2. Base Handler (Link in the chain)
public abstract class OrderHandler
{
    private OrderHandler? _next;

    public OrderHandler SetNext(OrderHandler next)
    {
        _next = next;
        return next;
    }

    public virtual void Handle(OrderPipelineRequest request)
    {
        _next?.Handle(request);
    }
}

// 3. Concrete Handlers
public class AuthenticationHandler : OrderHandler
{
    public override void Handle(OrderPipelineRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token) || request.Token != "valid-jwt-token")
        {
            Console.WriteLine("🛑 [AuthHandler] Access Denied: Invalid or missing token. (CHAIN STOPPED)");
            return; // Short-circuit!
        }

        Console.WriteLine("✅ [AuthHandler] User authenticated successfully.");
        base.Handle(request);
    }
}

public class RateLimitingHandler : OrderHandler
{
    public override void Handle(OrderPipelineRequest request)
    {
        if (request.UserIp == "192.168.1.50")
        {
            Console.WriteLine("🛑 [RateLimitHandler] Too many requests from this IP. (CHAIN STOPPED)");
            return; // Short-circuit!
        }

        Console.WriteLine("✅ [RateLimitHandler] Rate limit check passed.");
        base.Handle(request);
    }
}

public class ValidationHandler : OrderHandler
{
    public override void Handle(OrderPipelineRequest request)
    {
        if (request.Amount <= 0)
        {
            Console.WriteLine("🛑 [ValidationHandler] Order amount must be greater than zero. (CHAIN STOPPED)");
            return; // Short-circuit!
        }

        Console.WriteLine("✅ [ValidationHandler] Order payload validated.");
        base.Handle(request);
    }
}

public class OrderExecutionHandler : OrderHandler
{
    public override void Handle(OrderPipelineRequest request)
    {
        Console.WriteLine($"🎉 [OrderExecutionHandler] Order of ${request.Amount} successfully processed and saved to database!");
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class ChainOfResponsibilityExample
{
    public static void Run()
    {
        Console.WriteLine("=== CHAIN OF RESPONSIBILITY PATTERN ===");

        // Assemble chain
        var auth = new AuthenticationHandler();
        var rateLimit = new RateLimitingHandler();
        var validation = new ValidationHandler();
        var execution = new OrderExecutionHandler();

        auth.SetNext(rateLimit)
            .SetNext(validation)
            .SetNext(execution);

        Console.WriteLine("--- Scenario 1: Bad Token (Stops at Auth) ---");
        auth.Handle(new OrderPipelineRequest { Token = "expired-token", UserIp = "10.0.0.1", Amount = 150m });

        Console.WriteLine("\n--- Scenario 2: Blocked IP (Stops at Rate Limit) ---");
        auth.Handle(new OrderPipelineRequest { Token = "valid-jwt-token", UserIp = "192.168.1.50", Amount = 150m });

        Console.WriteLine("\n--- Scenario 3: Invalid Amount (Stops at Validation) ---");
        auth.Handle(new OrderPipelineRequest { Token = "valid-jwt-token", UserIp = "10.0.0.1", Amount = -10m });

        Console.WriteLine("\n--- Scenario 4: Perfect Request (Flows completely through) ---");
        auth.Handle(new OrderPipelineRequest { Token = "valid-jwt-token", UserIp = "10.0.0.1", Amount = 250m });

        Console.WriteLine();
    }
}
