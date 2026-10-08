namespace designPattern.Distributed;

// 1. Endpoints
public interface IHttpEndpoint
{
    string HandleRequest(string path);
}

// Legacy Monolith (.NET Framework 4.8 / IIS)
public class LegacyMonolithApp : IHttpEndpoint
{
    public string HandleRequest(string path)
    {
        return $"  🏛️ [LEGACY MONOLITH (IIS / .NET 4.8)] Handled request for '{path}' using legacy stored procedures.";
    }
}

// Modern Cloud Microservice (.NET 9 / Linux Container)
public class ModernOrdersMicroservice : IHttpEndpoint
{
    public string HandleRequest(string path)
    {
        return $"  🚀 [MODERN SERVICE (.NET 9 / K8s)] Handled request for '{path}' using high-speed Minimal APIs and EF Core 9.";
    }
}

// 2. Reverse Proxy Router (Simulating Microsoft YARP - Yet Another Reverse Proxy)
public class YarpReverseProxy
{
    private readonly Dictionary<string, IHttpEndpoint> _routeMap = new();
    private readonly IHttpEndpoint _legacyFallback;

    public YarpReverseProxy(IHttpEndpoint legacyFallback)
    {
        _legacyFallback = legacyFallback;
    }

    public void RegisterMigratedRoute(string pathPrefix, IHttpEndpoint targetService)
    {
        _routeMap[pathPrefix] = targetService;
        Console.WriteLine($"  [YARP-CONFIG] Route '{pathPrefix}*' migrated to new modern microservice!");
    }

    public string RouteRequest(string path)
    {
        foreach (var (prefix, endpoint) in _routeMap)
        {
            if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return endpoint.HandleRequest(path);
            }
        }

        // Strangler Fig Fallback: un-migrated routes continue hitting the old monolith
        return _legacyFallback.HandleRequest(path);
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class StranglerFigExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- Tier 2 Pattern 5: The Strangler Fig Pattern (Legacy Modernization) ---");

        var monolith = new LegacyMonolithApp();
        var modernOrders = new ModernOrdersMicroservice();

        var proxy = new YarpReverseProxy(monolith);

        Console.WriteLine("Step 1: Day 1 - 100% of routes hit legacy monolith:");
        Console.WriteLine(proxy.RouteRequest("/api/customers/12"));
        Console.WriteLine(proxy.RouteRequest("/api/orders/99"));

        Console.WriteLine("\nStep 2: Month 1 - Migrated '/api/orders' to modern .NET 9 service:");
        proxy.RegisterMigratedRoute("/api/orders", modernOrders);

        Console.WriteLine("\nStep 3: Incoming traffic now dynamically split by YARP reverse proxy:");
        // Hits modern service:
        Console.WriteLine(proxy.RouteRequest("/api/orders/101"));
        // Still hits legacy monolith safely without breaking:
        Console.WriteLine(proxy.RouteRequest("/api/customers/45"));
        Console.WriteLine(proxy.RouteRequest("/api/billing/invoice-9"));

        Console.WriteLine("\nResult: Zero downtime, zero big-bang rewrite risk. Monolith strangled route by route!");
    }
}
