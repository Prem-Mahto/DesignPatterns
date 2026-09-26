# C# & .NET Design Patterns: Complete Masterclass & Architectural Guide

> **Author / Mentor:** Antigravity (Veteran C# Architect & Software Developer)  
> **Repository:** `D:\repoes\designPattern`  
> **Target Framework:** .NET 9+ / Modern C#

---

## Table of Contents

1. [Architectural Mindset & Foundations](#1-architectural-mindset--foundations)
2. [Creational Design Patterns](#2-creational-design-patterns)
   - [Pattern 1: Singleton Pattern](#pattern-1-singleton-creational)
     - *Deep Dive: When is the instance created? Does `Lazy<T>.Value` create new instances?*
   - [Pattern 2: Factory Patterns (Simple, Factory Method, Abstract Factory)](#pattern-2-factory-creational)
     - *Deep Dive: Handling unconfigured services in .NET DI (`GetRequiredKeyedService` vs `GetKeyedService`)*
     - *Deep Dive: Simple Factory vs Abstract Factory (Cloud Provider Example)*
     - *Deep Dive: OCP in Parameterized Factory vs True GoF Factory Method vs Modern .NET Open/Closed Resolvers*
   - [Pattern 3: Builder Pattern](#pattern-3-builder-creational)
     - *Deep Dive: Parameter Object / Options Model vs Builder*
     - *Deep Dive: The Step Builder (Compile-Time Type-State Machine with IntelliSense)*
   - [Pattern 4: Prototype Pattern](#pattern-4-prototype-creational)
     - *Deep Dive: Deep Cloning with Composition (Recursive Prototype vs JSON Serialization)*
   - [Foundational Deep Dives](#foundational-deep-dives)
     - *Why use Properties instead of Class Variables (Fields)?*
     - *When to use `internal` vs `public` for Classes, Methods, and Properties?*
3. [Structural Design Patterns](#3-structural-design-patterns)
   - [Overview of the 7 Structural Patterns](#overview-of-the-7-structural-patterns)
   - [Pattern 1: Adapter Pattern](#pattern-1-adapter-structural)
   - [Pattern 2: Decorator Pattern](#pattern-2-decorator-structural)
   - [Pattern 3: Facade Pattern](#pattern-3-facade-structural)
     - *Deep Dive: Does Facade just move complexity? Centralization & Law of Conservation of Complexity*
   - [Pattern 4: Proxy Pattern](#pattern-4-proxy-structural)
     - *Deep Dive: Dependency Injection for Proxies with `IHttpContextAccessor` and Keyed Services*
   - [Pattern 5: Composite Pattern](#pattern-5-composite-structural)
   - [Pattern 6: Bridge Pattern](#pattern-6-bridge-structural)
   - [Pattern 7: Flyweight Pattern](#pattern-7-flyweight-structural)
4. [Behavioral Design Patterns](#4-behavioral-design-patterns)
   - [Overview of the 11 Behavioral Patterns](#overview-of-the-11-behavioral-patterns)
   - [Pattern 1: Strategy Pattern](#pattern-1-strategy-behavioral)
   - [Pattern 2: Observer Pattern](#pattern-2-observer-behavioral)
   - [Pattern 3: Command Pattern (With Undo/Redo)](#pattern-3-command-behavioral)
   - [Pattern 4: Mediator Pattern](#pattern-4-mediator-behavioral)
     - *Deep Dive: Air Traffic Control rewritten with Modern MediatR (Commands, Notifications, Pipeline Behaviors)*
   - [Pattern 5: Chain of Responsibility Pattern](#pattern-5-chain-of-responsibility-behavioral)
5. [Top C# / .NET-Specific Enterprise Patterns](#5-top-c--net-specific-enterprise-patterns)
   - [Pattern 1: Repository & Unit of Work Pattern](#net-pattern-1-repository--unit-of-work)
   - [Pattern 2: The Options Pattern (`IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>`)](#net-pattern-2-the-options-pattern)
   - [Deep Dive: .NET Garbage Collection (GC) Internals](#deep-dive-net-garbage-collection-gc-internals)
   - [Deep Dive: Dispose vs Finalize](#deep-dive-dispose-vs-finalize)
     - *Can a class have both Dispose and a Finalizer?*
     - *What happens if a class implements neither?*
     - *What happens if a class has unmanaged resources and implements neither? (Permanent Native Leak)*
     - *Can we skip Dispose and rely only on the Finalizer?*
   - [Pattern 3: The Complete Dispose / Resource Cleanup Pattern (`IDisposable` & `IAsyncDisposable`)](#net-pattern-3-the-complete-dispose-pattern)
   - [Pattern 4: The Result Pattern (Railway-Oriented Programming)](#net-pattern-4-the-result-pattern)

---

# 1. Architectural Mindset & Foundations

> **"A design pattern is not a goal; it is a battle-tested solution to a recurring problem."**  
> Never use a pattern just because it sounds fancy. You use a pattern when writing plain, straightforward code starts to cause real architectural pain (tight coupling, brittle code, untestable components, or scalability bottlenecks).

### The Roadmap:
Design patterns from the classic Gang of Four (GoF) fall into three broad families:
1. **Creational Patterns** *(How objects are born)*: Singleton, Factory Method, Abstract Factory, Builder, Prototype.
2. **Structural Patterns** *(How objects fit together)*: Adapter, Decorator, Facade, Composite, Proxy, Bridge, Flyweight.
3. **Behavioral Patterns** *(How objects talk to each other)*: Strategy, Observer, Command, Mediator, Chain of Responsibility, State, Template Method, Iterator, Memento, Visitor, Interpreter.

---

# 2. Creational Design Patterns

## Pattern 1: Singleton (Creational)

### 1. What is the Problem? (The Real-World Pain)
Imagine you are building a banking application or an e-commerce platform. You have a component that manages:
- An in-memory cache
- An application configuration / feature-flag reader
- A hardware or serial port interface

What happens if every class that needs to access this resource creates a new instance using `new AppSettings()` or `new HardwarePort()`?
- **Memory & Resource waste:** You allocate redundant, expensive resources over and over.
- **State Inconsistency:** If Service A modifies cache copy 1, Service B won't see it because it is looking at cache copy 2.
- **Hardware Conflicts:** Two instances trying to lock the exact same physical printer or COM port will crash the process.

### 2. The Core Concept (Plain English)
The **Singleton Pattern** ensures that:
1. A class has **only one instance** throughout the entire lifetime of the application.
2. It provides a **single, global point of access** to that instance.

Think of it like the **President or Prime Minister of a country**: at any given moment, there can only be one official president in office. Everyone in the government references the exact same person.

### 3. How to Implement It in Modern C#
In modern C#, the cleanest, safest, and highest-performing way to write a Singleton is using .NET's built-in **`Lazy<T>`**. It gives you **thread safety** and **lazy initialization** (it only allocates memory when first called) without requiring manual locking.

```csharp
public sealed class ApplicationConfiguration
{
    // 1. Thread-safe, lazy-initialized instance holder
    private static readonly Lazy<ApplicationConfiguration> _instance =
        new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration());

    // 2. Private constructor prevents external 'new ApplicationConfiguration()'
    private ApplicationConfiguration()
    {
        Console.WriteLine("Loading configuration file from disk... (Expensive op)");
        ApiBaseUrl = "https://api.myapp.com";
    }

    // 3. Public global access point
    public static ApplicationConfiguration Instance => _instance.Value;

    // Instance properties / methods
    public string ApiBaseUrl { get; set; }

    public void LogCurrentState()
    {
        Console.WriteLine($"Current Base URL: {ApiBaseUrl}");
    }
}
```

#### How it is consumed:
```csharp
class Program
{
    static void Main()
    {
        // Both variables point to the exact same memory address
        var config1 = ApplicationConfiguration.Instance;
        var config2 = ApplicationConfiguration.Instance;

        config1.ApiBaseUrl = "https://updated-api.myapp.com";

        // config2 reflects the change because config1 and config2 are the exact same instance
        config2.LogCurrentState(); // Outputs: "Current Base URL: https://updated-api.myapp.com"

        Console.WriteLine(ReferenceEquals(config1, config2)); // Outputs: True
    }
}
```

### 4. The Architect's Perspective (Modern .NET Reality)
- **Testing Nightmare:** Classic Singletons with static `Instance` properties make unit testing difficult because they introduce global state that persists between tests and cannot be easily mocked.
- **Tight Coupling:** Any class calling `ApplicationConfiguration.Instance` is tightly bound to that concrete implementation.
- **How Modern .NET / ASP.NET Core Solves This:** We use the built-in Dependency Injection container:
  ```csharp
  builder.Services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
  ```

---

### Deep Dive Q&A: When will the instance be created? Does `_instance.Value` create a new instance each time?

> **Question:** Explain this: When will the instance be created? Every time `Instance.Value` is called, wouldn't it create a new instance?
> ```csharp
> private static readonly Lazy<ApplicationConfiguration> _instance =
>     new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration());
> ```

**Answer:**
#### 1. Wouldn't calling `_instance.Value` create a new instance every time?
**No, absolutely not.** 
When you create `new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration())`, you are not giving it an object; you are giving it a **recipe (a delegate/lambda)**.

Internally, `Lazy<T>` works like this pseudocode:
```csharp
public class Lazy<T>
{
    private T _cachedValue;
    private bool _isValueCreated = false;
    private readonly Func<T> _factory;
    private readonly object _lock = new object();

    public T Value
    {
        get
        {
            if (_isValueCreated) return _cachedValue;

            lock (_lock)
            {
                if (!_isValueCreated)
                {
                    _cachedValue = _factory(); // Runs your lambda ONCE
                    _isValueCreated = true;
                }
            }
            return _cachedValue;
        }
    }
}
```
- **Call #1 to `.Value`:** Sees `_isValueCreated == false`. Runs the lambda, creates the object, stores it in `_cachedValue`, sets `_isValueCreated = true`, and returns it.
- **Call #2 to `.Value`:** Sees `_isValueCreated == true`. It **skips** the lambda completely and returns the cached instance.
- **Call #1,000,000 to `.Value`:** Returns the exact same cached reference.

#### 2. When exactly is the instance created?
The instance is created **on the very first access to `.Value`** (when someone accesses `ApplicationConfiguration.Instance`).
- **Startup:** Constructor has NOT run yet.
- **First Call to `Instance`:** Constructor runs.
- **Second Call to `Instance`:** Constructor does NOT run.

---

## Pattern 2: Factory (Creational)

### 1. What is the Problem? (The Real-World Pain)
Imagine you are building an **E-Commerce Checkout System** processing payments via Credit Card, PayPal, and Crypto.

Without a factory, your checkout service looks like this:
```csharp
// ❌ BAD: Tightly coupled, violates Open/Closed Principle
public class OrderService
{
    public void ProcessOrder(Order order, string paymentMethod)
    {
        IPaymentProcessor processor;

        if (paymentMethod == "CreditCard")
            processor = new CreditCardProcessor("apiKey_123", timeout: 30);
        else if (paymentMethod == "PayPal")
            processor = new PayPalProcessor("clientId_abc", "secret_xyz");
        else if (paymentMethod == "Crypto")
            processor = new CryptoProcessor(network: "Ethereum", gasLimit: 21000);
        else
            throw new NotSupportedException("Invalid payment method");

        processor.Charge(order.TotalAmount);
    }
}
```
**Why does an Architect cringe?**
1. **Tight Coupling:** `OrderService` must know API keys, secrets, and setup parameters for every processor.
2. **Violation of Open/Closed Principle (OCP):** Adding ApplePay requires editing `OrderService`.
3. **Duplication:** Refunding logic in `RefundService` copies the exact same `if/else` block.

### 2. The Core Concept (Plain English)
> **"Delegate the responsibility of object instantiation away from business logic to a dedicated creator."**

**Analogy:** When you order a pizza at a restaurant, you don't enter the kitchen to buy flour, knead dough, and turn on the oven. You tell the waiter: *"Bring me a Pepperoni Pizza."* The kitchen (Factory) knows the recipe and temperature. You just eat the pizza (`IPizza.Eat()`).

### 3. Implementation with Abstraction
```csharp
public interface IPaymentProcessor
{
    void Charge(decimal amount);
}

public class CreditCardProcessor : IPaymentProcessor
{
    public void Charge(decimal amount) => Console.WriteLine($"Charging ${amount} via Credit Card Gateway.");
}

public class PayPalProcessor : IPaymentProcessor
{
    public void Charge(decimal amount) => Console.WriteLine($"Redirecting to PayPal to authorize ${amount}.");
}

public class ApplePayProcessor : IPaymentProcessor
{
    public void Charge(decimal amount) => Console.WriteLine($"Charging ${amount} via Apple Pay biometric token.");
}
```

```csharp
public enum PaymentMethod { CreditCard, PayPal, ApplePay }

public interface IPaymentProcessorFactory
{
    IPaymentProcessor Create(PaymentMethod method);
}

public class PaymentProcessorFactory : IPaymentProcessorFactory
{
    public IPaymentProcessor Create(PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.CreditCard => new CreditCardProcessor(),
            PaymentMethod.PayPal => new PayPalProcessor(),
            PaymentMethod.ApplePay => new ApplePayProcessor(),
            _ => throw new ArgumentOutOfRangeException(nameof(method), $"Unsupported: {method}")
        };
    }
}
```

#### Clean Business Logic:
```csharp
public class OrderService
{
    private readonly IPaymentProcessorFactory _paymentFactory;

    public OrderService(IPaymentProcessorFactory paymentFactory)
    {
        _paymentFactory = paymentFactory;
    }

    public void ProcessOrder(decimal totalAmount, PaymentMethod method)
    {
        IPaymentProcessor processor = _paymentFactory.Create(method);
        processor.Charge(totalAmount);
    }
}
```

---

### Deep Dive Q&A: What happens if a payment method is not configured?

> **Question:** What happens if a payment method is not configured?
> ```csharp
> var processor = _serviceProvider.GetRequiredKeyedService<IPaymentProcessor>(method);
> ```

**Answer:**
`GetRequiredKeyedService` throws an **`InvalidOperationException`** at runtime:
```text
System.InvalidOperationException: No service for type 'IPaymentProcessor' and service key 'ApplePay' has been registered.
```
The word **`Required`** is .NET's explicit contract: *"If you can't find this, crash immediately with an exception."*

#### Handling Gracefully in Production:
Use `GetKeyedService` (without "Required"), which returns **`null`**:
```csharp
var processor = _serviceProvider.GetKeyedService<IPaymentProcessor>(method);
if (processor is null)
{
    throw new PaymentMethodNotSupportedException($"Payment method '{method}' is currently not enabled.");
}
processor.Charge(amount);
```

---

### Deep Dive: Simple Factory (The Everyday Idiom)

It is **not** an official Gang of Four (GoF) pattern. It is simply a coding idiom: a single helper class with a method (often `static`) that wraps object creation with a `switch` statement:
```csharp
public static class NotificationFactory
{
    public static INotification Create(string channel) => channel.ToLowerInvariant() switch
    {
        "email" => new EmailNotification(),
        "sms" => new SmsNotification(),
        _ => throw new ArgumentException("Unknown channel")
    };
}
```
* **Pros:** Extremely simple. Perfect when you only have 2–3 types that rarely change.
* **Cons (Violates OCP):** Every time a new channel is introduced (e.g., WhatsApp), you must edit `NotificationFactory.cs`.

---

### Deep Dive: Abstract Factory (For "Families" of Related Objects)

While the **Factory Method** produces a *single* product, the **Abstract Factory** is designed for an entirely different scale: producing an entire **family (suite) of related products that must work together**.

#### 1. What is the Problem? (The "Frankenstein" System)
Imagine you are building an **Enterprise Multi-Cloud Document Processing Platform**.
Your company sells this software to clients hosting either on **AWS** or **Azure**.

Every cloud deployment requires a **suite of three collaborating services**:
1. **File Storage** (AWS S3 vs. Azure Blob Storage)
2. **Message Queue** (AWS SQS vs. Azure Service Bus)
3. **Audit Database** (AWS DynamoDB vs. Azure CosmosDB)

##### The Disaster Without Abstract Factory:
Suppose developers instantiate services using individual factories or direct `new` statements throughout the codebase:
```csharp
// ❌ DISASTER: The "Frankenstein" Cloud State
public class DocumentProcessor
{
    public void Process(Document doc)
    {
        // Developer A instantiated an AWS S3 bucket:
        IFileStorage storage = new AwsS3Storage();

        // Developer B instantiated an Azure Service Bus queue:
        IMessageQueue queue = new AzureServiceBusQueue(); 

        // Developer C instantiated an AWS DynamoDB database:
        IAuditDatabase database = new AwsDynamoDatabase();

        storage.Upload(doc.FileName, doc.Content);
        queue.Publish("document-uploaded"); // 💥 CRASH!
        database.LogAudit("Upload complete");
    }
}
```
**Why does an Architect cringe looking at this?**
1. **The Inconsistent Family Bug (Accidental Cross-Contamination):** An AWS S3 bucket is sending events to an Azure Service Bus queue! The AWS credentials and VPC settings do not match Azure's active directory.
2. **Configuration Hell:** Business classes are polluted with dozens of `if (cloudProvider == "AWS") ... else if (cloudProvider == "Azure") ...` checks.
3. **Violates Open/Closed Principle:** Adding Google Cloud Platform (GCP) requires hunting down and modifying every file in the codebase.

#### 2. The Core Concept (Plain English)
> **"Provide an interface for creating families of related or dependent objects without specifying their concrete classes."**

**The Real-World Analogy: Interior Design Furniture Suites 🛋️**
* **Family 1 (Victorian Style):** Victorian Sofa, Victorian Coffee Table, Victorian Chair.
* **Family 2 (Modern Art Deco):** Art Deco Glass Sofa, Art Deco Steel Coffee Table, Art Deco Neon Chair.

If you put an ornate, floral Victorian Velvet Sofa next to an ultra-futuristic Art Deco Neon Chair, your living room looks mismatched. 
The **Abstract Factory** guarantees:
> *"If you choose the Victorian suite, EVERY piece of furniture delivered to your house is guaranteed to be Victorian. You will never accidentally receive a neon steel table."*

```
                         [ ICloudServiceFactory ] (Abstract Factory)
                                    │
           ┌────────────────────────┴────────────────────────┐
           ▼                                                 ▼
[ AwsServiceFactory ]                             [ AzureServiceFactory ]
 (Concrete Factory 1)                              (Concrete Factory 2)
   ├── Creates: S3Storage                            ├── Creates: AzureBlobStorage
   ├── Creates: SqsQueue                             ├── Creates: AzureServiceBusQueue
   └── Creates: DynamoDbDatabase                     └── Creates: CosmosDbDatabase
```

#### 3. Complete Implementation in Modern C#

##### Step 1: The Abstract Product Interfaces (The Family Members)
```csharp
// Family Member 1: Storage
public interface IFileStorage
{
    void UploadFile(string fileName, byte[] data);
}

// Family Member 2: Messaging Queue
public interface IMessageQueue
{
    void EnqueueMessage(string message);
}

// Family Member 3: Audit Database
public interface IAuditDatabase
{
    void RecordLog(string action);
}
```

##### Step 2: Concrete Products for Family 1 (AWS Family)
```csharp
public class AwsS3Storage : IFileStorage
{
    public void UploadFile(string fileName, byte[] data) =>
        Console.WriteLine($"📦 [AWS S3] Uploaded '{fileName}' ({data.Length} bytes) to bucket 'production-docs'.");
}

public class AwsSqsQueue : IMessageQueue
{
    public void EnqueueMessage(string message) =>
        Console.WriteLine($"📬 [AWS SQS] Enqueued message to 'arn:aws:sqs:us-east-1:order-events': \"{message}\"");
}

public class AwsDynamoDatabase : IAuditDatabase
{
    public void RecordLog(string action) =>
        Console.WriteLine($"🗄️ [AWS DynamoDB] Wrote audit entry: '{action}' to table 'SystemAuditLog'.");
}
```

##### Step 3: Concrete Products for Family 2 (Azure Family)
```csharp
public class AzureBlobStorage : IFileStorage
{
    public void UploadFile(string fileName, byte[] data) =>
        Console.WriteLine($"📦 [Azure Blob] Uploaded '{fileName}' ({data.Length} bytes) to container 'client-assets'.");
}

public class AzureServiceBusQueue : IMessageQueue
{
    public void EnqueueMessage(string message) =>
        Console.WriteLine($"📬 [Azure Service Bus] Published to topic 'orders-topic': \"{message}\"");
}

public class AzureCosmosDatabase : IAuditDatabase
{
    public void RecordLog(string action) =>
        Console.WriteLine($"🗄️ [Azure CosmosDB] Inserted document partition 'Audit': '{action}'.");
}
```

##### Step 4: The Abstract Factory Interface
```csharp
public interface ICloudServiceFactory
{
    IFileStorage CreateStorage();
    IMessageQueue CreateQueue();
    IAuditDatabase CreateDatabase();
}
```

##### Step 5: Concrete Factories (One Factory per Family)
```csharp
// Factory for AWS Ecosystem
public class AwsServiceFactory : ICloudServiceFactory
{
    public IFileStorage CreateStorage() => new AwsS3Storage();
    public IMessageQueue CreateQueue() => new AwsSqsQueue();
    public IAuditDatabase CreateDatabase() => new AwsDynamoDatabase();
}

// Factory for Azure Ecosystem
public class AzureServiceFactory : ICloudServiceFactory
{
    public IFileStorage CreateStorage() => new AzureBlobStorage();
    public IMessageQueue CreateQueue() => new AzureServiceBusQueue();
    public IAuditDatabase CreateDatabase() => new AzureCosmosDatabase();
}
```

##### Step 6: The Client / Consumer (Pure Clean Architecture)
Notice how `DocumentWorkflowManager` has **zero knowledge** of whether it runs on AWS or Azure:
```csharp
public class DocumentWorkflowManager
{
    private readonly IFileStorage _storage;
    private readonly IMessageQueue _queue;
    private readonly IAuditDatabase _database;

    // Inject the Abstract Factory!
    public DocumentWorkflowManager(ICloudServiceFactory factory)
    {
        // The factory GUARANTEES all 3 services belong to the exact same cloud provider!
        _storage = factory.CreateStorage();
        _queue = factory.CreateQueue();
        _database = factory.CreateDatabase();
    }

    public void ProcessDocument(string fileName, byte[] data)
    {
        Console.WriteLine($"\n--- Processing Document: {fileName} ---");
        _storage.UploadFile(fileName, data);
        _queue.EnqueueMessage($"FileReady:{fileName}");
        _database.RecordLog($"Uploaded and enqueued {fileName} at {DateTime.UtcNow:HH:mm:ss}");
    }
}
```

##### Step 7: Running the Code (1-Line Family Swap)
```csharp
class Program
{
    static void Main()
    {
        byte[] sampleData = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // Mock PDF bytes

        // SCENARIO 1: Client A deployed on AWS
        Console.WriteLine("=== DEPLOYMENT 1: AWS ENVIRONMENT ===");
        ICloudServiceFactory awsFactory = new AwsServiceFactory();
        var awsManager = new DocumentWorkflowManager(awsFactory);
        awsManager.ProcessDocument("Quarterly_Report.pdf", sampleData);

        // SCENARIO 2: Client B deployed on Azure (Only 1 line changes!)
        Console.WriteLine("\n=== DEPLOYMENT 2: AZURE ENVIRONMENT ===");
        ICloudServiceFactory azureFactory = new AzureServiceFactory();
        var azureManager = new DocumentWorkflowManager(azureFactory);
        azureManager.ProcessDocument("Tax_Statement_2026.pdf", sampleData);
    }
}
```

##### Output:
```text
=== DEPLOYMENT 1: AWS ENVIRONMENT ===
--- Processing Document: Quarterly_Report.pdf ---
📦 [AWS S3] Uploaded 'Quarterly_Report.pdf' (4 bytes) to bucket 'production-docs'.
📬 [AWS SQS] Enqueued message to 'arn:aws:sqs:us-east-1:order-events': "FileReady:Quarterly_Report.pdf"
🗄️ [AWS DynamoDB] Wrote audit entry: 'Uploaded and enqueued Quarterly_Report.pdf at 12:45:00' to table 'SystemAuditLog'.

=== DEPLOYMENT 2: AZURE ENVIRONMENT ===
--- Processing Document: Tax_Statement_2026.pdf ---
📦 [Azure Blob] Uploaded 'Tax_Statement_2026.pdf' (4 bytes) to container 'client-assets'.
📬 [Azure Service Bus] Published to topic 'orders-topic': "FileReady:Tax_Statement_2026.pdf"
🗄️ [Azure CosmosDB] Inserted document partition 'Audit': 'Uploaded and enqueued Tax_Statement_2026.pdf at 12:45:00'.
```

#### 4. The Architect's Deep Dive: Trade-offs & The Achilles' Heel ⚠️

* **The Strength (Adding New Families is 100% OCP):**  
  To add Google Cloud Platform (GCP), write `GcpStorage`, `GcpQueue`, `GcpDatabase`, and `GcpServiceFactory`. Zero modifications to existing classes!
* **The Achilles' Heel (Adding New Product Types Breaks Everything):**  
  If management decides every cloud suite also needs a Cache (`ICacheService`), you must modify `ICloudServiceFactory` to add `ICacheService CreateCache();`. **Every single concrete factory (`AwsServiceFactory`, `AzureServiceFactory`, etc.) immediately breaks and fails to compile.**

> **The Architect's Rule:**  
> Use Abstract Factory when the **family members (Product types) are stable and well-known upfront**, but the **variations/ecosystems (Concrete families) are expected to grow**.

#### 5. Summary: Factory Method vs. Abstract Factory

| Feature | Factory Method | Abstract Factory |
| :--- | :--- | :--- |
| **How many products?** | **1 Product** (e.g. `IPaymentProcessor`) | **A Family / Suite of Products** (e.g. `IFileStorage` + `IMessageQueue` + `IAuditDatabase`) |
| **Implementation** | Uses **Inheritance** (a single method `CreateProcessor()` overridden in subclasses). | Uses **Composition** (an object containing multiple factory methods). |
| **Primary Guarantee** | Decouples business logic from knowing which concrete class is created. | Guarantees that related products from the same family are **never accidentally mixed up**. |

---

### Deep Dive Q&A: OCP in Parameterized Factory vs True GoF Factory Method vs Modern .NET Resolvers

> **Question:** You mentioned Simple Factory breaks the open/close principle because for each new type we have to modify the factory class, but I see a similar thing in Factory Method too with the `switch` statement!

**Answer:**
You caught something that confuses 90% of developers! What was shown earlier with the `switch` statement is a **Parameterized Factory**. 

#### The True GoF Factory Method (Subclasses decide, NO switch statements):
```csharp
public abstract class PaymentProcessorFactory
{
    public abstract IPaymentProcessor CreateProcessor(); // The Factory Method!

    public void ExecutePayment(decimal amount)
    {
        IPaymentProcessor processor = CreateProcessor();
        Console.WriteLine("Applying fraud checks...");
        processor.Charge(amount);
        Console.WriteLine("Audit log saved.");
    }
}

public class CreditCardFactory : PaymentProcessorFactory
{
    public override IPaymentProcessor CreateProcessor() => new CreditCardProcessor();
}

public class PayPalFactory : PaymentProcessorFactory
{
    public override IPaymentProcessor CreateProcessor() => new PayPalProcessor();
}
```
*To add ApplePay, create `ApplePayProcessor` and `ApplePayFactory`. Existing classes are 100% untouched.*

#### Modern .NET Open/Closed Resolver Pattern:
To avoid subclass explosion (20 classes for 10 processors), modern .NET injects `IEnumerable<T>`:
```csharp
public class ModernPaymentFactory
{
    private readonly Dictionary<string, IPaymentProcessor> _processors;

    public ModernPaymentFactory(IEnumerable<IPaymentProcessor> processors)
    {
        _processors = processors.ToDictionary(p => p.MethodName, p => p, StringComparer.OrdinalIgnoreCase);
    }

    public IPaymentProcessor GetProcessor(string method) =>
        _processors.TryGetValue(method, out var p) ? p : throw new NotSupportedException();
}
```

---

## Pattern 3: Builder (Creational)

### 1. What is the Problem? (The Real-World Pain)

Have you ever seen a constructor that looks like this in a real project?

```csharp
// ❌ The "Telescoping Constructor" Nightmare
var email = new EmailMessage(
    "dev@company.com", 
    "user@domain.com", 
    null,                    // cc?
    null,                    // bcc?
    "Welcome!",              // subject
    "Hello User...",         // body
    true,                    // isHtml?
    false,                   // hasAttachments?
    null,                    // attachments collection?
    5,                       // retryCount?
    Priority.High            // priority
);
```

#### Why is this an architectural disaster?
1. **Unreadable Code:** What does the 3rd `null` mean? What does `true, false` mean without looking up the constructor definition?
2. **Order Dependency:** Swap two `null`s or two `booleans` by accident, and you introduce a subtle runtime bug.
3. **Constructor Explosion:** If some callers want 3 parameters, others want 5, others want 8, developers start writing 6 overloaded constructors.
4. **The "Setter" Problem:** If you make all properties `public { get; set; }` to avoid constructors, your object is **mutable and unsafe**—anyone can mutate fields halfway through, leaving the object in an invalid, half-baked state.

---

### 2. The Core Concept (Plain English)

> **"Separate the construction of a complex object from its representation, allowing you to produce different variations step-by-step."**

**Analogy: Customizing a Subway Sandwich or a Gaming PC**  
You don’t ask the counter clerk for a pre-made sandwich that comes with 20 ingredients where you have to say *"no onions, no pickles, no mustard"*. 
Instead, you build it fluently step-by-step:
- Choose bread: *Italian Herbs & Cheese*
- Choose protein: *Chicken Teriyaki*
- Add cheese: *Provolone*
- Toast it: *Yes*
- Wrap and hand over: *Ready to eat!*

---

### 3. Implementation in Modern C#: The "Fluent" Builder

Let's build a clean, bulletproof **`EmailMessage`** builder.

#### Step 1: The Complex Product (Notice it is Immutable!)
The product itself should be **immutable** (cannot be altered after creation) and its constructor can be made internal/private so nobody bypasses the builder:

```csharp
public class EmailMessage
{
    public string From { get; }
    public List<string> ToRecipients { get; } = new();
    public List<string> CcRecipients { get; } = new();
    public string Subject { get; }
    public string Body { get; }
    public bool IsHtml { get; }
    public int Priority { get; }

    // Internal constructor: forces creation via Builder
    internal EmailMessage(
        string from, 
        List<string> to, 
        List<string> cc, 
        string subject, 
        string body, 
        bool isHtml, 
        int priority)
    {
        From = from;
        ToRecipients = to;
        CcRecipients = cc;
        Subject = subject;
        Body = body;
        IsHtml = isHtml;
        Priority = priority;
    }
}
```

#### Step 2: The Fluent Builder
We use **method chaining** (returning `this`) so the caller can chain calls cleanly:

```csharp
public class EmailMessageBuilder
{
    private string _from;
    private readonly List<string> _to = new();
    private readonly List<string> _cc = new();
    private string _subject = string.Empty;
    private string _body = string.Empty;
    private bool _isHtml = false;
    private int _priority = 1; // Default: Normal

    public EmailMessageBuilder From(string sender)
    {
        _from = sender;
        return this; // Returns 'this' to allow chaining
    }

    public EmailMessageBuilder To(string recipient)
    {
        _to.Add(recipient);
        return this;
    }

    public EmailMessageBuilder Cc(string recipient)
    {
        _cc.Add(recipient);
        return this;
    }

    public EmailMessageBuilder WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public EmailMessageBuilder WithBody(string body, bool isHtml = false)
    {
        _body = body;
        _isHtml = isHtml;
        return this;
    }

    public EmailMessageBuilder WithHighPriority()
    {
        _priority = 3;
        return this;
    }

    // THE BUILD METHOD: Validates business rules before object is born!
    public EmailMessage Build()
    {
        // Centralized validation: ensure object is NEVER in an invalid state
        if (string.IsNullOrWhiteSpace(_from))
            throw new InvalidOperationException("Email must have a 'From' sender.");

        if (_to.Count == 0)
            throw new InvalidOperationException("Email must have at least one 'To' recipient.");

        return new EmailMessage(_from, _to, _cc, _subject, _body, _isHtml, _priority);
    }
}
```

#### Step 3: Consuming the Builder (Clean & Self-Documenting)

```csharp
var email = new EmailMessageBuilder()
    .From("admin@company.com")
    .To("client@domain.com")
    .Cc("manager@company.com")
    .WithSubject("Invoice #1042 Ready")
    .WithBody("<h1>Please find your invoice attached.</h1>", isHtml: true)
    .WithHighPriority()
    .Build();
```
Look at that code. **No mystery parameters, no `null, null, true, false`, and 100% self-documenting.**

---

### 4. The Architect's View: Where You See This Every Day in .NET

You already use the Builder Pattern constantly in modern .NET:

1. **ASP.NET Core Startup:**
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   builder.Services.AddControllers();
   builder.Services.AddEndpointsApiExplorer();
   var app = builder.Build(); // <-- Creates the immutable WebApplication!
   ```
2. **`StringBuilder`:**
   ```csharp
   var sb = new StringBuilder().Append("Hello").Append(" ").Append("World").ToString();
   ```
3. **Entity Framework Core `ModelBuilder`:**
   ```csharp
   modelBuilder.Entity<Order>()
       .HasKey(o => o.Id)
       .Property(o => o.Total)
       .IsRequired();
   ```

---

### Summary Checklist: When to use Builder

| Use Builder When... | Don't Use Builder When... |
| :--- | :--- |
| The object has **4+ constructor arguments** or many optional combinations | The object is simple with just 2 or 3 required fields |
| You want an **immutable** object that cannot be modified after `.Build()` | You are modeling a simple DTO / POCO with `{ get; set; }` |
| Construction requires **strict validation rules** before the object is created | You just want a simple data holder (`record`) |

---

### Deep Dive Q&A: Can we pass a model into the constructor instead of a Builder?

> **Question:** One question: we could have a model passed into the constructor like this:
> ```csharp
> var emailModel = new EmailModel() { to = "x@company.com", from = "y@company.com" };
> emailModel.subject = "welcome";
> var email = new EmailMessage(emailModel);
> ```

**Answer:**
This is the **Parameter Object** (or **Options Pattern**). While better than 10 parameters, the Builder Pattern is chosen for 4 architectural reasons:
1. **Meaningful Domain Operations vs Dumb Properties:** Builder methods express business intent (`.WithHighPriority()`, `.AttachEncryptedPdf()`). Models just have raw properties.
2. **Guarding against Incomplete State:** An `EmailModel` can be instantiated completely empty without compile warnings.
3. **Step-by-step assembly across multiple services:** Different middleware can add audit headers or branding to the builder before `.Build()`.
4. **Class Clutter (Shadow Classes):** 50 domain entities would require 50 shadow `XYZModel` classes.

---

### Deep Dive Q&A: The Step Builder (Type-State Pattern with IntelliSense)

> **Question:** Could you give a full example of how this is achieved, the IntelliSense part?
> ```csharp
> // IntelliSense won't even SHOW you '.WithSubject()' until you call '.From()' and '.To()'
> EmailBuilder
>     .From("admin@company.com")       // returns IRequireTo interface
>     .To("user@company.com")          // returns IRequireSubject interface
>     .WithSubject("Hello")            // returns IOptionalSettings interface
>     .Build();
> ```

#### Complete Implementation:
```csharp
public class EmailMessage
{
    public string From { get; }
    public string To { get; }
    public string Subject { get; }
    public string? Body { get; }
    public bool IsHighPriority { get; }

    internal EmailMessage(string from, string to, string subject, string? body, bool isHighPriority)
    {
        From = from; To = to; Subject = subject; Body = body; IsHighPriority = isHighPriority;
    }
}

// Stage Interfaces
public interface IRequireTo
{
    IRequireSubject To(string recipient);
}

public interface IRequireSubject
{
    IOptionalEmailSettings WithSubject(string subject);
}

public interface IOptionalEmailSettings
{
    IOptionalEmailSettings WithBody(string body);
    IOptionalEmailSettings WithHighPriority();
    EmailMessage Build();
}

// The Single Builder implementing all interfaces:
public class EmailBuilder : IRequireTo, IRequireSubject, IOptionalEmailSettings
{
    private readonly string _from;
    private string _to = string.Empty;
    private string _subject = string.Empty;
    private string? _body;
    private bool _isHighPriority;

    private EmailBuilder(string from) => _from = from;

    public static IRequireTo From(string from) => new EmailBuilder(from);

    public IRequireSubject To(string recipient)
    {
        _to = recipient;
        return this; // Cast to IRequireSubject
    }

    public IOptionalEmailSettings WithSubject(string subject)
    {
        _subject = subject;
        return this; // Cast to IOptionalEmailSettings
    }

    public IOptionalEmailSettings WithBody(string body) { _body = body; return this; }
    public IOptionalEmailSettings WithHighPriority() { _isHighPriority = true; return this; }

    public EmailMessage Build() => new EmailMessage(_from, _to, _subject, _body, _isHighPriority);
}
```

---

## Pattern 4: Prototype (Creational)

### 1. What is the Problem? (The Real-World Pain)
Creating objects via `new` can be painfully expensive when initializing templates, 3D meshes, tax rules, or loading 50MB files. Calling `new HeavyObject()` 1,000 times destroys CPU and memory.

### 2. The Core Concept
Create one master **prototype** once, and **clone** it when needed, mutating only what is different (like photocopying a master legal lease agreement).

### 3. Shallow Copy vs. Deep Copy in .NET
- **Shallow Copy (`MemberwiseClone()`):** Copies value types, but for reference types (lists, nested objects), it copies only the memory pointer! Both clones point to the same list.
- **Deep Copy:** Recursively clones the object **and** creates brand-new copies of all nested objects/collections.

```csharp
public interface IPrototype<T> { T Clone(); }

public class EmploymentContract : IPrototype<EmploymentContract>
{
    public string EmployeeName { get; set; }
    public decimal BaseSalary { get; set; }
    public List<string> Benefits { get; set; }

    public EmploymentContract Clone()
    {
        var clone = (EmploymentContract)this.MemberwiseClone();
        clone.Benefits = new List<string>(this.Benefits); // Deep copy list!
        return clone;
    }
}
```
*Modern C# cheat code: Use `record` with non-destructive mutation (`var clone = template with { EmployeeName = "John" };`).*

---

### Deep Dive Q&A: Deep Cloning with Composition

> **Question:** Let's say `EmploymentContract` has a composition (`public ClassA a { get; set; }`), then how will we deep-clone the object?

**Answer:**
#### Approach 1: Recursive Prototype (Pure OOP & High Performance)
Make `ClassA` implement `IPrototype<ClassA>` too:
```csharp
public class ClassA : IPrototype<ClassA>
{
    public string Department { get; set; }
    public int OfficeNumber { get; set; }
    public ClassA Clone() => (ClassA)this.MemberwiseClone();
}

public class EmploymentContract : IPrototype<EmploymentContract>
{
    public string EmployeeName { get; set; }
    public List<string> Benefits { get; set; }
    public ClassA A { get; set; } // Composition!

    public EmploymentContract Clone()
    {
        var clone = (EmploymentContract)this.MemberwiseClone();
        clone.Benefits = new List<string>(this.Benefits);
        clone.A = this.A?.Clone(); // Deep clone composed object!
        return clone;
    }
}
```

#### Approach 2: JSON Serialization (For Deep 10-level Graphs)
```csharp
public EmploymentContract Clone()
{
    var json = JsonSerializer.Serialize(this);
    return JsonSerializer.Deserialize<EmploymentContract>(json)!;
}
```

---

## Foundational Deep Dives

### Deep Dive: Why use Properties instead of Class Variables (Fields)?

> **Question:** Why not use class variables (fields)? Why use properties when both getter and setter are simple and public?

**Answer:**
1. **Binary Compatibility (The `MissingFieldException` Trap):** In IL bytecode, a field is `ldfld`; a property is a method call (`callvirt get_Name()`). Changing a field to a property later crashes all compiled DLLs at runtime!
2. **Frameworks Ignore Fields:** `System.Text.Json`, EF Core, ASP.NET Model Binding, and Blazor/WPF Data Binding bind exclusively to properties by default.
3. **Interfaces Cannot Have Fields:** Interfaces cannot declare fields, only properties.
4. **Polymorphism:** Properties can be `virtual`, `abstract`, and `override`. Fields cannot.
5. **Zero Performance Difference:** The .NET JIT compiler automatically inlines simple auto-properties into direct memory access at runtime.

---

### Deep Dive: When to use `internal` vs `public`

> **Question:** When to make a class internal and when to make it public? Same for fields/properties? I only see public everywhere.

**Answer:**
"Public everywhere" is tutorial code. In enterprise software:
> *"Make everything as restrictive as possible by default. Start with `internal`. Only promote to `public` when an external project genuinely needs to access it."*

1. **Clean Architecture / Layer Encapsulation:** In `Infrastructure.csproj`, make all concrete EF Core repositories `internal`. Expose only a public extension method `AddInfrastructure(this IServiceCollection services)`. Web API controllers physically cannot bypass architecture to `new` concrete repositories.
2. **Enforcing Factories & Builders:** Mark entity constructors `internal`. Outside assemblies can hold references to the entity, but cannot construct it without your `Builder` or `Factory`.
3. **API Surface Area & NuGet Packages:** Public classes are a lifetime promise (breaking changes require major version bumps). Internal classes can be deleted or refactored freely.
4. **Testing Internal Classes:** Add `[assembly: InternalsVisibleTo("MyProject.Tests")]` in `.csproj`. Your test project has full access; the outside world sees nothing.

---

# 3. Structural Design Patterns

## Overview of the 7 Structural Patterns

| Pattern | One-Line Persona | Primary Focus |
| :--- | :--- | :--- |
| **Adapter** | *The Translator* | Makes two **incompatible interfaces** work together. |
| **Decorator** | *The Wrapper* | **Adds responsibilities dynamically** without subclass explosion. |
| **Facade** | *The Front Desk* | Provides a **simplified high-level entry point** to a complex system. |
| **Proxy** | *The Bodyguard* | **Controls access** (security, lazy loading, caching, remote boundary). |
| **Composite** | *The Tree* | Treats **single items and collections/trees uniformly**. |
| **Bridge** | *The Decoupler* | Splits a class into **two independent dimensions** of change. |
| **Flyweight** | *The Memory Saver* | **Shares common immutable state** across millions of objects to save RAM. |

---

## Pattern 1: Adapter (Structural)

### 1. Problem / Pain Point
Integrating a legacy or 3rd-party vendor SDK (`SpeedySmsLegacyApi`) whose parameter types and method names do not match your clean `INotificationService`.

### 2. Core Concept
A wrapper that translates calls from what your code expects into what the incompatible class needs (like a US-to-UK wall plug adapter). Acts as an **Anti-Corruption Layer (ACL)**.

```csharp
public class SpeedySmsAdapter : INotificationService
{
    private readonly SpeedySmsLegacyApi _legacyApi;

    public SpeedySmsAdapter(SpeedySmsLegacyApi legacyApi) => _legacyApi = legacyApi;

    public Task SendAsync(string recipient, string message)
    {
        string cleanPhone = new string(recipient.Where(char.IsDigit).ToArray());
        long parsed = long.Parse(cleanPhone);
        _legacyApi.DispatchMessageV2(parsed, message, priorityFlag: 1, isFlash: false);
        return Task.CompletedTask;
    }
}
```

---

## Pattern 2: Decorator (Structural)

### 1. Problem / Pain Point
Adding caching, telemetry, and retry to `IWeatherService` via inheritance leads to `CachedLoggedRetryWeatherService` class explosion ($2^N$ subclasses).

### 2. Core Concept
Wrap the object inside another class implementing the **exact same interface** (like adding milk and caramel to coffee).

```csharp
public class CachedWeatherService : IWeatherService
{
    private readonly IWeatherService _inner;
    private readonly Dictionary<string, string> _cache = new();

    public CachedWeatherService(IWeatherService inner) => _inner = inner;

    public async Task<string> GetForecastAsync(string city)
    {
        if (_cache.TryGetValue(city, out var val)) return val;
        var fresh = await _inner.GetForecastAsync(city);
        _cache[city] = fresh;
        return fresh;
    }
}

// Chaining:
IWeatherService service = new LoggingWeatherService(new CachedWeatherService(new ApiWeatherService()));
```
*In ASP.NET Core DI, use the `Scrutor` library: `builder.Services.Decorate<IWeatherService, CachedWeatherService>();`*

---

## Pattern 3: Facade (Structural)

### 1. Problem / Pain Point
A Web Controller coordinates Inventory, Tax, Payment, Shipping, and Email (5 dependencies, 40 lines of orchestration).

### 2. Core Concept
Provide a unified high-level front desk (`OrderProcessingFacade.PlaceOrderAsync()`).

```csharp
public class OrderProcessingFacade : IOrderProcessingFacade
{
    private readonly InventoryService _inventory;
    private readonly PaymentService _payment;
    private readonly ShippingService _shipping;
    private readonly NotificationService _notification;

    public async Task<OrderResult> PlaceOrderAsync(OrderRequest request)
    {
        _inventory.DeductStock(request.Sku, request.Quantity);
        _payment.ProcessPayment(request.PaymentToken, request.TotalAmount);
        var tracking = _shipping.GenerateLabel(request.ShippingAddress);
        _notification.SendReceipt(request.CustomerEmail, tracking);
        return OrderResult.Success(tracking);
    }
}
```

---

### Deep Dive Q&A: Does Facade just move complexity?

> **Question:** Doesn't this just move all the complexity from checkout controller to the facade?

**Answer:**
**Yes, absolutely!** 
> **Law of Conservation of Complexity:** You cannot eliminate business complexity; you can only decide **where it lives** and **how many times it is repeated**.

* **Without Facade:** The 40 lines are copy-pasted across Web Controller, Mobile API, Batch CSV Importer, and Admin Console (**160 lines in 4 places**). If fraud checks change, you must update 4 files.
* **With Facade:** The complexity lives in **1 single class**. All 4 callers make a 1-line call.
* **Unit Testing:** You test the controller by mocking **1 interface** (`IOrderProcessingFacade`) instead of 5 separate services.

---

## Pattern 4: Proxy (Structural)

### 1. Problem / Pain Point
Controlling, securing, or lazy-loading an expensive/sensitive object (`RealDocumentService`).

### 2. Core Concept
A stand-in / bodyguard that implements the exact same interface (`DocumentServiceProxy`) and intercepts calls to verify roles and audit logs before delegating to the real service.

---

### Deep Dive Q&A: How to configure Dependency Injection for Proxy in ASP.NET Core

> **Question:** How will DI work for this?

```csharp
// In Program.cs (Using .NET 8/9 Keyed Services):
builder.Services.AddHttpContextAccessor();
builder.Services.AddKeyedScoped<RealDocumentService>("real");
builder.Services.AddScoped<IDocumentService, DocumentServiceProxy>();

// Inside DocumentServiceProxy:
public class DocumentServiceProxy : IDocumentService
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly IServiceProvider _sp;
    private RealDocumentService? _real;

    public DocumentServiceProxy(IHttpContextAccessor httpContext, IServiceProvider sp)
    {
        _httpContext = httpContext;
        _sp = sp;
    }

    public void DisplayDocument(string id)
    {
        var user = _httpContext.HttpContext?.User;
        if (user == null || !user.IsInRole("Admin")) throw new UnauthorizedAccessException();

        _real ??= _sp.GetRequiredKeyedService<RealDocumentService>("real"); // Lazy loading!
        _real.DisplayDocument(id);
    }
}
```

---

## Pattern 5: Composite (Structural)

### 1. Problem / Pain Point
E-commerce carts have individual products and nested product bundles (bundles containing bundles). Without composite, callers write nested loops and recursive type checks (`if item is Bundle`).

### 2. Core Concept
Treat individual products (`Product` - Leaf) and bundles (`ProductBundle` - Composite) uniformly via a shared interface (`ICatalogItem`). Calling `GetPrice()` on the root bundle recursively sums all children.

```csharp
public class ProductBundle : ICatalogItem
{
    private readonly List<ICatalogItem> _items = new();
    public decimal GetPrice() => _items.Sum(i => i.GetPrice()) * (1 - _discountPercentage / 100m);
}
```

---

## Pattern 6: Bridge (Structural)

### 1. Problem: Cartesian Explosion
2 independent dimensions of change: Notification Type (SystemAlert, UserReminder) $\times$ Delivery Channel (Email, SMS, Slack). Pure inheritance creates $N \times M$ Cartesian explosion (6 classes for 2 types $\times$ 3 channels; 20 classes for 4 types $\times$ 5 channels).

### 2. Core Concept
Decouple Abstraction (`Notification`) from Implementation (`IMessageSender`) via composition. Math drops from $N \times M$ to $N + M$ (only 9 classes instead of 20!).

```csharp
public abstract class Notification
{
    protected readonly IMessageSender _sender; // The Bridge
    protected Notification(IMessageSender sender) => _sender = sender;
    public abstract void Notify(string msg);
}
```

---

## Pattern 7: Flyweight (Structural)

### 1. Problem / Pain Point
Rendering 1,000,000 trees in a game map. If each tree holds its own 50KB 3D mesh texture: $1,000,000 \times 50\text{ KB} = \mathbf{50\text{ GB RAM}}$ (Crash with OutOfMemory).

### 2. Core Concept
Split state into:
1. **Intrinsic (Shared / Flyweight):** 3D mesh, bark texture, leaf color (held in 1 shared `TreeType` object).
2. **Extrinsic (Contextual):** `X`, `Y` coordinates passed as parameters into `.Draw(x, y)`.
* **Memory Saved:** 50 GB drops to **~16 MB**! Used in .NET String Interning (`string.Intern`).

---

# 4. Behavioral Design Patterns

## Overview of the 11 Behavioral Patterns

1. **Strategy:** Swappable algorithms behind a common interface.
2. **Observer:** Publisher broadcasts updates to registered subscribers.
3. **Command:** Encapsulates actions into objects for queuing, logging, and Undo/Redo.
4. **Mediator:** Central hub that coordinates many-to-many communication to stop spiderweb coupling.
5. **Chain of Responsibility:** Pipeline of handlers where each can process, pass, or short-circuit.
6. **State:** Object changes its behavior when its internal state changes (Finite State Machine).
7. **Template Method:** Base class defines algorithm skeleton; subclasses override specific steps.
8. **Iterator:** Sequentially traverse collections (`IEnumerable<T>`, `yield return`).
9. **Memento:** Capture and restore internal object state snapshots (Checkpoints / Ctrl+Z).
10. **Visitor:** Add new operations to class hierarchies without modifying their code.
11. **Interpreter:** Grammatical representation to evaluate domain expressions.

---

## Pattern 1: Strategy (Behavioral)

### 1. Problem / Pain Point
Monolithic `switch (discountType)` statements violating OCP and making unit testing impossible.

### 2. Core Concept
Extract each algorithm into its own class implementing `IDiscountStrategy` (`NoDiscountStrategy`, `VipDiscountStrategy`, `BlackFridayDiscountStrategy`).

```csharp
public interface IDiscountStrategy { decimal ApplyDiscount(decimal totalAmount); }
public class VipDiscountStrategy : IDiscountStrategy { public decimal ApplyDiscount(decimal t) => t * 0.8m; }

public class CheckoutService
{
    private IDiscountStrategy _strategy;
    public void SetStrategy(IDiscountStrategy s) => _strategy = s;
    public decimal Checkout(decimal total) => _strategy.ApplyDiscount(total);
}
```
*C# Functional Shortcut: Use delegates (`Func<decimal, decimal>`) as lightweight inline strategies (e.g. LINQ's `.OrderBy(x => x.Age)`).*

---

## Pattern 2: Observer (Behavioral)

### 1. Problem / Pain Point
When Bitcoin price changes, mobile app, trading bot, and dashboard need real-time updates without polling every 100ms.

### 2. Core Concept
`StockTicker` (Subject) notifies registered `IStockObserver` instances via `.Attach()`, `.Detach()`, `.Notify()`.

```csharp
public class StockTicker
{
    private readonly List<IStockObserver> _observers = new();
    public void Attach(IStockObserver o) => _observers.Add(o);
    public void Detach(IStockObserver o) => _observers.Remove(o);
    public void SetPrice(decimal p) { _price = p; Notify(); }
    private void Notify() => _observers.ForEach(o => o.OnPriceChanged(_symbol, _price));
}
```

### The #1 Memory Leak in C# (Lapsed Listener):
If a singleton publisher holds an event subscription to a transient subscriber (`ticker.PriceChanged += myControl.Update`), the GC can **never collect the subscriber**. Always unsubscribe (`-=`) in `Dispose()`.

---

## Pattern 3: Command (Behavioral)

### 1. Problem / Pain Point
Calling `bankAccount.Withdraw(100)` directly cannot be queued, scheduled, logged, or undone.

### 2. Core Concept
Package the request into `ITransactionCommand` with `Execute()` and `Undo()`. Maintain a `Stack<ITransactionCommand>` in `TransactionManager` to enable full Ctrl+Z undo/rollback capabilities.

```csharp
public class DepositCommand : ITransactionCommand
{
    private readonly BankAccount _account;
    private readonly decimal _amount;
    public bool Execute() { _account.Deposit(_amount); return true; }
    public void Undo() { _account.Withdraw(_amount); }
}

public class TransactionManager
{
    private readonly Stack<ITransactionCommand> _history = new();
    public void ExecuteTransaction(ITransactionCommand c) { if (c.Execute()) _history.Push(c); }
    public void UndoLast() { if (_history.Count > 0) _history.Pop().Undo(); }
}
```

---

## Pattern 4: Mediator (Behavioral)

### 1. Problem: Spiderweb Coupling Chaos
10 services talking to each other directly creates a chaotic spiderweb ($N(N-1)/2 = 45$ direct connections).

### 2. Core Concept
All components talk **only** to the Mediator (like airplanes talking only to the Air Traffic Control Tower).

---

### Deep Dive: Air Traffic Control rewritten with Modern MediatR

```csharp
// 1. 1-to-1 Command
public record LandingClearance(bool IsApproved, string Runway, string Reason);
public record RequestLandingCommand(string FlightNumber, string AircraftType) : IRequest<LandingClearance>;

public class RequestLandingHandler : IRequestHandler<RequestLandingCommand, LandingClearance>
{
    public Task<LandingClearance> Handle(RequestLandingCommand req, CancellationToken ct) =>
        Task.FromResult(new LandingClearance(true, "Runway-26L", "Cleared to land."));
}

// 2. 1-to-Many Notification (Broadcast)
public record MaydayAlertNotification(string FlightNumber, string EmergencyType) : INotification;

public class EmergencyServicesHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification n, CancellationToken ct)
    {
        Console.WriteLine($"Dispatched fire trucks for {n.FlightNumber}!");
        return Task.CompletedTask;
    }
}

// 3. Pipeline Behavior (Blackbox Logger)
public class FlightTelemetryBehavior<TReq, TResp> : IPipelineBehavior<TReq, TResp> where TReq : notnull
{
    public async Task<TResp> Handle(TReq req, RequestHandlerDelegate<TResp> next, CancellationToken ct)
    {
        Console.WriteLine($"Intercepted: {typeof(TReq).Name}");
        return await next();
    }
}
```

---

## Pattern 5: Chain of Responsibility (Behavioral)

### 1. Problem / Pain Point
Monolithic nested `if/else` checks for Auth $\rightarrow$ Rate Limiting $\rightarrow$ Validation $\rightarrow$ Execution.

### 2. Core Concept
Link handlers in a chain (`OrderHandler.SetNext()`). Any link can process, pass forward, or **short-circuit** (stop the chain).
* **Everyday .NET Example:** **ASP.NET Core Middleware Pipeline** (`app.Use(...)`) where unauthorized requests short-circuit before hitting endpoints.

```csharp
public class AuthenticationHandler : OrderHandler
{
    public override void Handle(OrderRequest request)
    {
        if (request.Token != "valid-jwt") { Console.WriteLine("Blocked!"); return; } // Short-circuit!
        base.Handle(request);
    }
}
```

---

# 5. Top C# / .NET-Specific Enterprise Patterns

## .NET Pattern 1: Repository & Unit of Work

### 1. Problem / Pain Point
Tightly coupling business logic directly to EF Core `AppDbContext` scattered across controllers. Causes partial saves and untestable code.

### 2. Core Concept
* **Repository:** Pretends to be an in-memory collection (`ICustomerRepository.GetByIdAsync`).
* **Unit of Work:** Coordinates transaction boundaries so multiple repository changes commit or roll back together atomically (`_unitOfWork.SaveChangesAsync()`).

### 3. The EF Core Controversy
`DbContext` is already a UoW, and `DbSet<T>` is a repository. Don't write generic `IRepository<T>` wrappers. Write **Domain-Specific Repositories** (`IOrderRepository.GetPendingOrdersForApproval()`) to enforce Clean Architecture and isolate DB dependencies for unit testing.

---

## .NET Pattern 2: The Options Pattern

### 1. Problem / Pain Point
Magic strings (`_config["Smtp:Port"]`), manual parsing, runtime crashes, and inability to unit test.

### 2. Core Concept
Strongly-typed class (`SmtpOptions`) bound in `Program.cs`.

### The 3 Flavors Comparison:
| Interface | Lifetime | Hot-Reloads on JSON change? | Usage |
| :--- | :--- | :--- | :--- |
| **`IOptions<T>`** | Singleton | ❌ No | Static configs that never change without app restart. |
| **`IOptionsSnapshot<T>`** | Scoped | ✅ Yes (per HTTP request) | Web controllers needing fresh request-time configs. |
| **`IOptionsMonitor<T>`** | Singleton | ✅ Yes (live `OnChange` event) | Singletons/background workers needing instant hot-reload. |

*Use `ValidateOnStart()` with Data Annotations to crash immediately during boot if production configs are missing, rather than failing at midnight in production.*

---

## Deep Dive: .NET Garbage Collection (GC) Internals

* **Managed Heap vs Unmanaged Resources:**
  - *Managed:* Allocated by `new` (strings, objects, arrays). Managed automatically by the GC.
  - *Unmanaged:* OS-level resources outside .NET (file handles `IntPtr`, TCP sockets, DB connections, native C++ memory). The GC has **no idea** how to clean these up.
* **Generations:**
  - *Gen 0:* Brand new, short-lived objects. Checked in microseconds.
  - *Gen 1:* Buffer zone for objects surviving one collection.
  - *Gen 2:* Long-lived objects (Singletons, static caches). Expensive full collection.

---

## Deep Dive: Dispose vs. Finalize

### A. Can a class have both Dispose and a Finalizer?
**Yes!** This is the classic Microsoft "Seatbelt and Airbag" pattern:
* `Dispose()` is the **Seatbelt** (explicit, fast, deterministic). It cleans up and calls `GC.SuppressFinalize(this)` to disable the airbag.
* The Finalizer `~MyClass()` is the **Airbag** (safety net). If the developer forgot `Dispose()`, the GC calls the finalizer as a last resort.

### B. What happens if a class implements neither?
**For 95% of C# classes (pure managed data), this is ideal!** The GC cleans them up in Gen 0 in microseconds without finalizer queue overhead.

### C. What happens if a class has unmanaged resources and implements neither?
**A Permanent Native Resource Leak.**
1. The GC reclaims the 24-byte C# wrapper object.
2. The native pointer (`IntPtr`) is lost forever.
3. The 50MB of native memory or OS file handle is orphaned in RAM/Kernel.
4. Causes **Invisible OutOfMemory crashes**, **File is locked errors**, or **OS Handle Exhaustion (10,000 limit)**.
5. The resource is **only** freed when the Operating System kills the entire process.

### D. Can we skip Dispose and rely only on the Finalizer?
**NO! Fatal consequences of skipping Dispose:**
1. *Zero Memory Pressure Trap:* A 32-byte SQL connection creates zero GC pressure. The GC takes a nap while your database connection pool (max 100) exhausts and crashes the website.
2. *File Remains Locked:* The file on disk cannot be read or emailed until the GC decides to run minutes later.
3. *Single Finalizer Thread:* The entire .NET runtime has **only 1 finalizer thread**. Unfinalized objects queue up and stall the engine.
4. *Finalizers cannot touch managed objects:* Referencing managed children inside a finalizer crashes the process with `ObjectDisposedException`.

---

## .NET Pattern 3: The Complete Dispose Pattern

```csharp
public class FileManager : IDisposable, IAsyncDisposable
{
    private IntPtr _unmanagedBuffer; // Unmanaged
    private FileStream? _managedStream; // Managed
    private bool _disposed = false;

    public FileManager(string path)
    {
        _managedStream = new FileStream(path, FileMode.OpenOrCreate);
        _unmanagedBuffer = Marshal.AllocHGlobal(1024);
    }

    // 1. Synchronous Dispose
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this); // Cancel the finalizer!
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Clean managed objects (Only safe when called from Dispose!)
            _managedStream?.Dispose();
            _managedStream = null;
        }

        // Clean unmanaged handles (Safe in both Dispose and Finalizer)
        if (_unmanagedBuffer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_unmanagedBuffer);
            _unmanagedBuffer = IntPtr.Zero;
        }

        _disposed = true;
    }

    // 2. The Finalizer Safety Net
    ~FileManager()
    {
        Dispose(disposing: false);
    }

    // 3. Modern Asynchronous Disposal (await using)
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_managedStream != null)
        {
            await _managedStream.DisposeAsync(); // Non-blocking async I/O!
            _managedStream = null;
        }

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }
}
```

---

## .NET Pattern 4: The Result Pattern (Railway-Oriented Programming)

* **The Problem:** Using `throw new Exception()` for normal business flows (e.g. Insufficient Balance, User Not Found) is an anti-pattern:
  1. Business failures are **expected outcomes**, not exceptions.
  2. Capturing stack traces is **100x to 1000x slower**.
  3. Methods have silent contracts (callers don't know what exceptions might crash the app).
  4. Bloated `try/catch` spaghetti in controllers.
* **The Concept:** A two-track railway line (Success Track vs Failure Track). The moment any step fails, execution switches to the failure track and returns an `Error`.

### Modern Implementation:
```csharp
public enum ErrorType { Failure, NotFound, Validation, Conflict }
public record Error(string Code, string Description, ErrorType Type = ErrorType.Failure);

public class Result<TValue>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException();
    public Error Error { get; }
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, Error error)
    {
        _value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<TValue> Success(TValue val) => new(val, true, Error.None);
    public static Result<TValue> Failure(Error err) => new(default, false, err);

    public static implicit operator Result<TValue>(TValue val) => Success(val);
    public static implicit operator Result<TValue>(Error err) => Failure(err);
}

// Business Service:
public Result<decimal> Withdraw(int accountId, decimal amount)
{
    var account = _repo.GetById(accountId);
    if (account == null) return Error.NotFound("Account.NotFound", "Account missing.");
    if (account.Balance < amount) return Error.Validation("Account.LowBalance", "Insufficient funds.");

    account.Balance -= amount;
    return account.Balance; // Automatically wraps into Result<decimal>.Success!
}

// Controller:
[HttpPost("withdraw")]
public IActionResult Withdraw(int id, decimal amount)
{
    var result = _service.Withdraw(id, amount);
    if (result.IsSuccess) return Ok(new { Balance = result.Value });

    return result.Error.Type switch
    {
        ErrorType.NotFound => NotFound(result.Error),
        ErrorType.Validation => BadRequest(result.Error),
        _ => StatusCode(500, result.Error)
    };
}
```

---

## Complete Pattern Catalog Summary

| Category | Patterns Covered |
| :--- | :--- |
| **Creational** | Singleton, Factory Method, Abstract Factory, Builder (Step Builder), Prototype |
| **Foundational** | Properties vs Fields, `internal` vs `public` Encapsulation |
| **Structural** | Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight |
| **Behavioral** | Strategy, Observer, Command, Mediator (Classic & MediatR), Chain of Responsibility |
| **.NET Enterprise** | Repository & Unit of Work, Options Pattern (`IOptions`), GC Foundations, Dispose vs Finalize, Complete Dispose Pattern, Result Pattern (ROP) |

*Generated as part of the C# .NET Software Architecture Masterclass.*
