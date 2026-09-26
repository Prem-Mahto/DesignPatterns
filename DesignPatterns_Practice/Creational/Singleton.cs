using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns_Practice.Creational
{
    // ==========================================
    // 1. BROKEN: Non-thread-safe manual singleton
    // ==========================================
    public class UnsafeSingleton
    {
        private static UnsafeSingleton? _instance;
        private static int _instanceCount = 0;

        private UnsafeSingleton()
        {
            // Simulate real-world work (reading config, DB handshake, etc.)
            // This widens the race window so multiple threads catch _instance == null
            Thread.Sleep(50);

            Interlocked.Increment(ref _instanceCount);
            Console.WriteLine($"[UNSAFE] Constructor called! Total instances created: {_instanceCount}");
        }

        public static UnsafeSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnsafeSingleton();
                }
                return _instance;
            }
        }
    }

    // ==========================================
    // 2. SAFE: Lazy<T> thread-safe singleton
    // ==========================================
    public class SafeLazySingleton
    {
        private static readonly Lazy<SafeLazySingleton> _lazyInstance =
            new Lazy<SafeLazySingleton>(() => new SafeLazySingleton());

        private static int _instanceCount = 0;

        private SafeLazySingleton()
        {
            Thread.Sleep(1);
            Interlocked.Increment(ref _instanceCount);
            Console.WriteLine($"[SAFE] Constructor called! Total instances created: {_instanceCount}");
        }

        public static string test = "test";
        public static SafeLazySingleton Instance => _lazyInstance.Value;
    }

    // ==========================================
    // Test Runner: Spawns 10 concurrent threads
    // ==========================================
    public class SingletonExample
    {
        public static void Run()
        {
            Console.WriteLine("--- Testing UnsafeSingleton with 10 concurrent threads ---");
            // Fire 10 parallel threads at the exact same time
            Parallel.For(0, 10, i =>
            {
                var instance = UnsafeSingleton.Instance;
            });

            Console.WriteLine("\n--- Testing SafeLazySingleton with 10 concurrent threads ---");
            Console.WriteLine(SafeLazySingleton.test);// accessing static field won't call the lazy instance creator
            // Fire 10 parallel threads at the exact same time
            Parallel.For(0, 10, i =>
            {
                var instance = SafeLazySingleton.Instance;
            });
        }
    }
}
