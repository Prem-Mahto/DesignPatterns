using System.Diagnostics;

namespace designPattern.Structural;

// 1. The Component Interface
public interface IWeatherService
{
    Task<string> GetForecastAsync(string city);
}

// 2. The Core Component
public class ApiWeatherService : IWeatherService
{
    public async Task<string> GetForecastAsync(string city)
    {
        Console.WriteLine($"[API] Fetching fresh data from satellite for '{city}'...");
        await Task.Delay(200); // Simulate network latency
        return $"Sunny, 24°C in {city}";
    }
}

// 3. Decorator 1: Caching
public class CachedWeatherService : IWeatherService
{
    private readonly IWeatherService _inner;
    private readonly Dictionary<string, string> _cache = new();

    public CachedWeatherService(IWeatherService inner)
    {
        _inner = inner;
    }

    public async Task<string> GetForecastAsync(string city)
    {
        if (_cache.TryGetValue(city, out var cachedData))
        {
            Console.WriteLine($"[Cache HIT] Returning cached data for '{city}'.");
            return cachedData;
        }

        var freshData = await _inner.GetForecastAsync(city);
        _cache[city] = freshData;
        return freshData;
    }
}

// 4. Decorator 2: Telemetry / Logging
public class LoggingWeatherService : IWeatherService
{
    private readonly IWeatherService _inner;

    public LoggingWeatherService(IWeatherService inner)
    {
        _inner = inner;
    }

    public async Task<string> GetForecastAsync(string city)
    {
        var sw = Stopwatch.StartNew();
        var result = await _inner.GetForecastAsync(city);
        sw.Stop();
        Console.WriteLine($"[Telemetry] Request for '{city}' took {sw.ElapsedMilliseconds}ms.");
        return result;
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class DecoratorExample
{
    public static void Run()
    {
        Console.WriteLine("=== DECORATOR PATTERN ===");

        // Chain decorators: Logging -> Caching -> Core API
        IWeatherService weatherService =
            new LoggingWeatherService(
                new CachedWeatherService(
                    new ApiWeatherService()
                )
            );

        Console.WriteLine("--- Request 1 (Cache is cold) ---");
        var result1 = weatherService.GetForecastAsync("London").GetAwaiter().GetResult();
        Console.WriteLine($"Result: {result1}");

        Console.WriteLine("\n--- Request 2 (Cache is warm) ---");
        var result2 = weatherService.GetForecastAsync("London").GetAwaiter().GetResult();
        Console.WriteLine($"Result: {result2}");

        Console.WriteLine();
    }
}
