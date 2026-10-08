namespace designPattern.Enterprise;

// 1. Strongly Typed Configuration POCO
public class SmtpSettings
{
    public const string SectionName = "SmtpSettings";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string ApiKey { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Host))
            throw new InvalidOperationException("Configuration Error: SmtpSettings.Host is required.");
        if (Port <= 0 || Port > 65535)
            throw new InvalidOperationException($"Configuration Error: SmtpSettings.Port {Port} is invalid.");
    }
}

// 2. Options Pattern Abstractions (Demonstrating .NET IOptions / IOptionsMonitor)
public interface IOptions<out T> where T : class
{
    T Value { get; }
}

public interface IOptionsMonitor<out T> where T : class
{
    T CurrentValue { get; }
    void OnChange(Action<T> listener);
}

// Concrete simulated implementations
public class OptionsWrapper<T> : IOptions<T> where T : class
{
    public T Value { get; }
    public OptionsWrapper(T value) => Value = value;
}

public class OptionsMonitorWrapper<T> : IOptionsMonitor<T> where T : class
{
    private T _currentValue;
    private readonly List<Action<T>> _listeners = new();

    public T CurrentValue => _currentValue;

    public OptionsMonitorWrapper(T initialValue) => _currentValue = initialValue;

    public void OnChange(Action<T> listener) => _listeners.Add(listener);

    // Simulating appsettings.json file reload in production
    public void Reload(T newValue)
    {
        _currentValue = newValue;
        Console.WriteLine("\n[HOT-RELOAD] Configuration file updated on disk!");
        foreach (var listener in _listeners)
        {
            listener(newValue);
        }
    }
}

// 3. Consumer Service
public class EmailService
{
    private readonly IOptions<SmtpSettings> _staticOptions;
    private readonly IOptionsMonitor<SmtpSettings> _liveOptions;

    public EmailService(IOptions<SmtpSettings> staticOptions, IOptionsMonitor<SmtpSettings> liveOptions)
    {
        _staticOptions = staticOptions;
        _liveOptions = liveOptions;

        // Subscribe to live hot-reloads
        _liveOptions.OnChange(updated =>
        {
            Console.WriteLine($"  [LISTENER] Live configuration notified: Host changed to '{updated.Host}', Port to {updated.Port}");
        });
    }

    public void SendEmail(string to, string subject)
    {
        var settings = _liveOptions.CurrentValue;
        Console.WriteLine($"[EMAIL] Sending to {to} | Host: {settings.Host}:{settings.Port} | SSL: {settings.EnableSsl} | Subject: '{subject}'");
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class OptionsExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 2: The Options Pattern ---");

        var initialConfig = new SmtpSettings
        {
            Host = "smtp.sendgrid.net",
            Port = 587,
            EnableSsl = true,
            ApiKey = "SG.initial-key"
        };
        initialConfig.Validate();

        var staticOptions = new OptionsWrapper<SmtpSettings>(initialConfig);
        var liveMonitor = new OptionsMonitorWrapper<SmtpSettings>(initialConfig);

        var emailService = new EmailService(staticOptions, liveMonitor);

        // Send with initial settings
        emailService.SendEmail("alice@example.com", "Welcome to the Platform");

        // Simulate DevOps updating appsettings.json while server runs
        liveMonitor.Reload(new SmtpSettings
        {
            Host = "smtp.internal-relay.corp",
            Port = 25,
            EnableSsl = false,
            ApiKey = "INTERNAL.relay-key"
        });

        // Send with hot-reloaded settings without restarting process
        emailService.SendEmail("bob@example.com", "System Alert");
    }
}
