namespace designPattern.Creational;

public sealed class ApplicationConfiguration
{
    // Thread-safe lazy-initialized instance holder
    private static readonly Lazy<ApplicationConfiguration> _instance =
        new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration());

    // Private constructor prevents external 'new ApplicationConfiguration()'
    private ApplicationConfiguration()
    {
        Console.WriteLine("Loading configuration file from disk... (Expensive op)");
        ApiBaseUrl = "https://api.myapp.com";
    }

    // Public global access point
    public static ApplicationConfiguration Instance => _instance.Value;

    // Instance properties / methods
    public string ApiBaseUrl { get; set; }

    public void LogCurrentState()
    {
        Console.WriteLine($"Current API Base URL: {ApiBaseUrl}");
    }
}

public class SingletonExample
{
    public static void Run()
    {
        Console.WriteLine("=== SINGLETON PATTERN ===");
        Console.WriteLine("App started.");
        // At this point: ApplicationConfiguration instance does NOT exist in memory yet.
        // The Lazy wrapper exists, but your constructor has NOT run.
        Console.WriteLine("Waiting 1 second before first access...");
        Thread.Sleep(1000);

        Console.WriteLine("About to ask for Instance for the first time...");
        // --> THIS EXACT MOMENT triggers () => new ApplicationConfiguration()
        var config1 = ApplicationConfiguration.Instance;

        Console.WriteLine("Asking for Instance a second time...");
        // --> This DOES NOT run the constructor. It grabs the cached reference.
        var config2 = ApplicationConfiguration.Instance;

        config1.ApiBaseUrl = "https://updated-api.myapp.com";

        // config2 reflects the change because config1 and config2 are the exact same instance in memory
        config2.LogCurrentState();

        Console.WriteLine($"Are both references identical? {ReferenceEquals(config1, config2)}");
        Console.WriteLine();
    }
}
