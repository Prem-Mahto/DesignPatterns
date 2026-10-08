namespace designPattern.Behavioral;

// 1. The State Interface
public interface IOrderState
{
    string StateName { get; }
    void ProcessPayment(Order context, decimal amount);
    void Ship(Order context);
    void Cancel(Order context);
}

// 2. The Context (Maintains current state)
public class Order
{
    public Guid Id { get; } = Guid.NewGuid();
    public decimal TotalAmount { get; }
    public IOrderState CurrentState { get; set; }

    public Order(decimal totalAmount)
    {
        TotalAmount = totalAmount;
        CurrentState = new DraftState(); // Initial state
        Console.WriteLine($"[ORDER {Id.ToString()[..8]}] Created in state: {CurrentState.StateName} (Total: ${TotalAmount})");
    }

    public void ProcessPayment(decimal amount) => CurrentState.ProcessPayment(this, amount);
    public void Ship() => CurrentState.Ship(this);
    public void Cancel() => CurrentState.Cancel(this);
}

// 3. Concrete States
public class DraftState : IOrderState
{
    public string StateName => "Draft";

    public void ProcessPayment(Order context, decimal amount)
    {
        if (amount >= context.TotalAmount)
        {
            Console.WriteLine($"[PAYMENT] Received ${amount}. Payment verified.");
            context.CurrentState = new PaidState();
            Console.WriteLine($"[TRANSITION] Order moved from Draft -> {context.CurrentState.StateName}");
        }
        else
        {
            Console.WriteLine($"[PAYMENT ERROR] Insufficient amount. Required: ${context.TotalAmount}, received: ${amount}");
        }
    }

    public void Ship(Order context)
    {
        Console.WriteLine("[ERROR] Cannot ship an order in Draft state! Payment required.");
    }

    public void Cancel(Order context)
    {
        Console.WriteLine("[CANCEL] Order cancelled before payment.");
        context.CurrentState = new CancelledState();
    }
}

public class PaidState : IOrderState
{
    public string StateName => "Paid";

    public void ProcessPayment(Order context, decimal amount)
    {
        Console.WriteLine("[PAYMENT] Order is already paid. Ignoring duplicate charge.");
    }

    public void Ship(Order context)
    {
        Console.WriteLine("[SHIPPING] Generating tracking label and dispatching parcel.");
        context.CurrentState = new ShippedState();
        Console.WriteLine($"[TRANSITION] Order moved from Paid -> {context.CurrentState.StateName}");
    }

    public void Cancel(Order context)
    {
        Console.WriteLine("[CANCEL] Issuing automated refund of payment...");
        context.CurrentState = new CancelledState();
        Console.WriteLine($"[TRANSITION] Order refunded and moved to {context.CurrentState.StateName}");
    }
}

public class ShippedState : IOrderState
{
    public string StateName => "Shipped";

    public void ProcessPayment(Order context, decimal amount)
    {
        Console.WriteLine("[ERROR] Order is already shipped. Cannot process payment.");
    }

    public void Ship(Order context)
    {
        Console.WriteLine("[ERROR] Order is already in transit with courier.");
    }

    public void Cancel(Order context)
    {
        Console.WriteLine("[ERROR] Cannot cancel order that is already shipped! Return merchandise required.");
    }
}

public class CancelledState : IOrderState
{
    public string StateName => "Cancelled";

    public void ProcessPayment(Order context, decimal amount)
    {
        Console.WriteLine("[ERROR] Cannot pay for a cancelled order.");
    }

    public void Ship(Order context)
    {
        Console.WriteLine("[ERROR] Cannot ship a cancelled order.");
    }

    public void Cancel(Order context)
    {
        Console.WriteLine("[ERROR] Order is already cancelled.");
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class StateExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- State Pattern (Finite State Machine) ---");

        var order = new Order(150m);

        // Attempt invalid transition
        Console.WriteLine("\n1. Attempting to ship without paying:");
        order.Ship();

        // Valid payment
        Console.WriteLine("\n2. Processing valid payment:");
        order.ProcessPayment(150m);

        // Valid shipping
        Console.WriteLine("\n3. Shipping after payment:");
        order.Ship();

        // Attempt invalid cancellation after shipping
        Console.WriteLine("\n4. Attempting to cancel shipped order:");
        order.Cancel();
    }
}
