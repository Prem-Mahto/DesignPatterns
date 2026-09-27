using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DesignPatterns_Practice.Structural
{

    public interface IWeatherService
    {
        Task<string> GetForecastAsync(string city);
    }
    public class ApiWeatherService : IWeatherService
    {
        public async Task<string> GetForecastAsync(string city)
        {
            Console.WriteLine($"[API] Fetching fresh data from satellite for '{city}'...");
            await Task.Delay(1000);
            return $"Sunny, 25°C in {city}";
        }
    }

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

    public class LoggedWeatherService : IWeatherService
    {
        private readonly IWeatherService _inner;

        public LoggedWeatherService(IWeatherService inner)
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

    //any combination of logging, caching and main class is possible
    public class DecoratorExample
    {
        public static void Run()
        {
            var weatherService =
                new LoggedWeatherService(
                    new CachedWeatherService(
                        new ApiWeatherService()
                    )
                );

            Console.WriteLine("--- Request 1 (Cache is cold) ---");
            Console.WriteLine(weatherService.GetForecastAsync("London").Result);

            Console.WriteLine("\n--- Request 2 (Cache is warm) ---");
            Console.WriteLine(weatherService.GetForecastAsync("London").Result);

            var weatherService2 =
                new CachedWeatherService(
                    new LoggedWeatherService(
                        new ApiWeatherService()
                    )
                );

            Console.WriteLine("\n\n--- Request 1 (Logging) ---");
            Console.WriteLine(weatherService2.GetForecastAsync("London").Result);

            Console.WriteLine("\n--- Request 2 (Logging skipped) ---");
            Console.WriteLine(weatherService2.GetForecastAsync("London").Result);

            var weatherService3 =
                new LoggedWeatherService(
                    new ApiWeatherService()
                );

            Console.WriteLine("\n\n--- Request 1 (No Cache) ---");
            Console.WriteLine(weatherService3.GetForecastAsync("London").Result);

            Console.WriteLine("\n--- Request 2 (No Cache) ---");
            Console.WriteLine(weatherService3.GetForecastAsync("London").Result);
        }
    }
}
