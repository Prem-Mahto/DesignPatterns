using System.Runtime.InteropServices;

namespace designPattern.Enterprise;

// The Standard Complete .NET Dispose Pattern (IDisposable + IAsyncDisposable + Finalizer)
public class AdvancedResourceManager : IDisposable, IAsyncDisposable
{
    private IntPtr _nativeBuffer; // Simulated unmanaged resource (OS memory handle)
    private MemoryStream? _managedStream; // Managed disposable resource
    private bool _disposed = false; // Idempotency flag

    public AdvancedResourceManager(string initialData)
    {
        // Allocate managed resource
        _managedStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(initialData));

        // Allocate native unmanaged memory (e.g., Marshal.AllocHGlobal)
        _nativeBuffer = Marshal.AllocHGlobal(1024);
        Console.WriteLine("  [INIT] Allocated 1024 bytes of native unmanaged memory and managed MemoryStream.");
    }

    public void WriteData(string text)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Console.WriteLine($"  [WORK] Processing payload: '{text}' (Stream length: {_managedStream?.Length} bytes)");
    }

    // 1. Synchronous Public IDisposable
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this); // Tell GC: Destructor unnecessary, object cleaned cleanly!
        Console.WriteLine("  [DISPOSE] Synchronous cleanup complete. GC.SuppressFinalize registered.");
    }

    // 2. Asynchronous Public IAsyncDisposable (for async network/file resources)
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(disposing: false); // Cleanup unmanaged resources
        GC.SuppressFinalize(this);
        Console.WriteLine("  [DISPOSE-ASYNC] Asynchronous cleanup complete.");
    }

    // Core cleanup engine
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Clean up MANAGED disposable objects
            if (_managedStream != null)
            {
                _managedStream.Dispose();
                _managedStream = null;
                Console.WriteLine("  [CLEANUP] Managed MemoryStream disposed.");
            }
        }

        // Clean up UNMANAGED resources (native handles/pointers)
        if (_nativeBuffer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_nativeBuffer);
            _nativeBuffer = IntPtr.Zero;
            Console.WriteLine("  [CLEANUP] Native unmanaged buffer freed from memory.");
        }

        _disposed = true;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_managedStream != null)
        {
            await _managedStream.DisposeAsync();
            _managedStream = null;
            Console.WriteLine("  [CLEANUP] Managed stream asynchronously flushed and disposed.");
        }
    }

    // 3. Finalizer (Airbag safety net in case developer forgets to call Dispose)
    ~AdvancedResourceManager()
    {
        Console.WriteLine("  ⚠️ [FINALIZER] Finalizer executed by GC! Developer forgot to call Dispose()!");
        Dispose(disposing: false); // ONLY touch unmanaged resources in finalizer!
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class DisposeExample
{
    public static async Task Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 3: The Complete Dispose Pattern ---");

        Console.WriteLine("\n1. Demonstrating 'using' statement (Deterministic Cleanup):");
        using (var manager = new AdvancedResourceManager("Transaction Audit Log"))
        {
            manager.WriteData("Sample entry 101");
        } // Dispose called automatically here

        Console.WriteLine("\n2. Demonstrating 'await using' statement (Async Cleanup):");
        await using (var asyncManager = new AdvancedResourceManager("Cloud Blob Data"))
        {
            asyncManager.WriteData("Streaming binary chunk");
        } // DisposeAsync called automatically here
    }
}
