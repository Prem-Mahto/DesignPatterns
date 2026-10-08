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
     - *[Comparison: Prototype vs. Flyweight](#deep-dive-qa-prototype-vs-flyweight-why-both-exist--the-4-fundamental-differences)*
   - [Foundational Deep Dives](#foundational-deep-dives)
     - *Why use Properties instead of Class Variables (Fields)?*
     - *When to use `internal` vs `public` for Classes, Methods, and Properties?*
3. [Structural Design Patterns](#3-structural-design-patterns)
   - [Overview of the 7 Structural Patterns](#overview-of-the-7-structural-patterns)
   - [Pattern 1: Adapter Pattern](#pattern-1-adapter-structural)
   - [Pattern 2: Decorator Pattern](#pattern-2-decorator-structural)
     - *[Comparison: Decorator vs. Proxy](#deep-dive-qa-decorator-vs-proxy-why-they-look-similar--their-4-fundamental-architectural-differences)*
   - [Pattern 3: Facade Pattern](#pattern-3-facade-structural)
     - *Deep Dive: Does Facade just move complexity? Centralization & Law of Conservation of Complexity*
   - [Pattern 4: Proxy Pattern](#pattern-4-proxy-structural)
     - *Deep Dive: Dependency Injection for Proxies with `IHttpContextAccessor` and Keyed Services*
     - *Deep Dive: Decorator vs. Proxy (Why they look similar & their 4 fundamental architectural differences)*
   - [Pattern 5: Composite Pattern](#pattern-5-composite-structural)
   - [Pattern 6: Bridge Pattern](#pattern-6-bridge-structural)
   - [Pattern 7: Flyweight Pattern](#pattern-7-flyweight-structural)
     - *Deep Dive: Prototype vs. Flyweight (Why both exist & the 4 fundamental differences)*
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

> [!NOTE]
> **Prototype vs. Flyweight Comparison:**  
> Wondering why Prototype and Flyweight seem to solve the same problem of managing many similar objects and how they fundamentally differ?  
> See the complete architectural breakdown, memory layout comparison, and comparison matrix in the [Flyweight Pattern Deep Dive: Prototype vs. Flyweight](#deep-dive-qa-prototype-vs-flyweight-why-both-exist--the-4-fundamental-differences).

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

While *Creational* patterns were all about **how objects are born**, *Structural* patterns are all about **how objects fit together like Lego bricks** to form larger, flexible systems without tight coupling.

## Overview of the 7 Classic GoF Structural Design Patterns

### 1. Adapter (The "Universal Travel Plug")
* **The Problem:** You have a new system that expects Interface A, but you have an existing or third-party legacy library that only provides Interface B. Their shapes don't match.
* **The Solution:** A wrapper class that translates calls from what your code expects into what the incompatible class needs.
* **Real-world C# Example:** Wrapping a legacy SOAP XML payment gateway so it looks like your modern `IPaymentService` JSON interface.

### 2. Decorator (The "Gift Wrap" / "Coffee Toppings")
* **The Problem:** You want to add extra features (caching, logging, encryption, metrics) to a class dynamically without modifying its existing code or creating a nightmare of subclasses (`CachedEncryptedLoggedRepository`).
* **The Solution:** Wrap the object inside another object that implements the same interface, adding behavior before or after delegating the call.
* **Real-world C# Example:** ASP.NET Core Middleware pipeline, `CryptoStream` wrapping a `FileStream`, or adding caching to an `IRepository`.

### 3. Facade (The "Hotel Front Desk")
* **The Problem:** A complex subsystem has 15 micro-services, classes, or initialization steps. Consumers get overwhelmed trying to orchestrate them in the right order.
* **The Solution:** Provide a single, simplified, high-level interface that hides the messy wiring behind one simple method call.
* **Real-world C# Example:** A checkout facade: `orderFacade.PlaceOrder()` handles inventory deduction, credit card charging, shipping label generation, and invoice emailing in one call.

### 4. Proxy (The "Security Guard" / "Bodyguard")
* **The Problem:** You want to control, secure, or delay access to an expensive or sensitive object without the caller knowing.
* **The Solution:** A stand-in placeholder that implements the exact same interface as the real object, intercepting calls to check permissions, log activity, or lazily initialize the real object.
* **Real-world C# Example:** Entity Framework's "Lazy Loading" dynamic proxies (loading navigation properties only when accessed), or an API caching proxy.

### 5. Composite (The "Tree / Folder Structure")
* **The Problem:** You have a hierarchy where an item can be a single element OR a collection of elements (e.g., a file vs. a folder containing files and sub-folders). You don't want callers to write messy `if (item is Folder) { foreach... } else { ... }`.
* **The Solution:** Treat individual objects and compositions of objects uniformly through a shared interface.
* **Real-world C# Example:** UI element trees (WPF/HTML DOM: a `Panel` contains `Buttons` and child `Panels`, and calling `.Draw()` draws everything recursively).

### 6. Bridge (The "Decoupler")
* **The Problem:** You have two independent dimensions that can grow indefinitely (e.g., Shapes: *Circle, Square, Triangle* and Renderers: *DirectX, OpenGL, Vulkan*). If you use inheritance, you end up with 3 × 3 = 9 classes (`DirectXCircle`, `OpenGLCircle`, etc.).
* **The Solution:** Separate the abstraction from its implementation via composition so both can grow independently without causing a subclass explosion.
* **Real-world C# Example:** Decoupling a `MessageSender` (SMS, Email) from the `MessagePriority` (Urgent, Normal).

### 7. Flyweight (The "Memory Optimizer")
* **The Problem:** Your application creates millions of small objects (e.g., 100,000 trees in a game, or every character in a 500-page text editor), causing severe RAM exhaustion and Out-Of-Memory (OOM) crashes.
* **The Solution:** Share common, immutable state (like textures, fonts, or colors) among all instances instead of duplicating it in every single object.
* **Real-world C# Example:** .NET's internal **String Interning** (`string.Intern()`), game engine particle systems.

### Quick Comparison Matrix

| Pattern | One-line Persona | Primary Focus |
| :--- | :--- | :--- |
| **Adapter** | *The Translator* | Makes two **incompatible interfaces** work together. |
| **Decorator** | *The Wrapper* | **Adds responsibilities dynamically** without inheritance. |
| **Facade** | *The Front Desk* | Provides a **simplified entry point** to a complex system. |
| **Proxy** | *The Bodyguard* | **Controls access** (security, lazy loading, caching). |
| **Composite** | *The Tree* | Treats **single items and collections uniformly**. |
| **Bridge** | *The Decoupler* | Splits a class into **two independent dimensions**. |
| **Flyweight** | *The Memory Saver* | **Shares common state** across millions of objects to save RAM. |

---

## Pattern 1: Adapter (Structural)

### 1. What is the Problem? (The Real-World Pain)
Imagine you are building a modern **Customer Notification System**. In your clean architecture, all your services depend on a standard, elegant interface:

```csharp
public interface INotificationService
{
    Task SendAsync(string recipientEmailOrPhone, string message);
}
```

Now, your company signs a contract with a legacy SMS gateway provider called **SpeedySms Inc.** They give you their closed-source DLL:

```csharp
// ❌ Third-party / Legacy code (you CANNOT change this class!):
public class SpeedySmsLegacyApi
{
    // Weird method name, expects long phone numbers with no '+' sign, and takes weird flags
    public bool DispatchMessageV2(long internationalPhoneNumber, string text, int priorityFlag, bool isFlash)
    {
        Console.WriteLine($"[SpeedySms Gateway] Sent to +{internationalPhoneNumber}: {text}");
        return true;
    }
}
```

#### The Dilemma:
* **The Bad Solution:** Litter your entire codebase with `if (usingSpeedySms) { ... parse phone to long, call DispatchMessageV2 ... }`. You pollute clean business logic with vendor junk. If you switch vendors next year, you must rewrite half your application.
* **The Architect's Solution:** Build an **Adapter**.

### 2. The Core Concept (Plain English)
> **"Convert the interface of an existing class into another interface that clients expect. An Adapter lets classes work together that couldn't otherwise because of incompatible interfaces."**

**The Analogy: A Travel Power Plug**  
You travel from the US to the UK with your US laptop charger (flat 2-prong plug). The UK wall socket has 3 rectangular pins. 
- You do **not** cut your laptop cable.
- You do **not** rip the wall socket out of the hotel.
- You buy a **£5 plug adapter** that sits in the middle: your laptop plugs into the adapter, and the adapter plugs into the wall.

```
[ Your Business Code ] ---> ( INotificationService )
                                    │
                                    ▼
                         [ SpeedySmsAdapter ]  <-- The Adapter!
                                    │
                                    ▼
                         [ SpeedySmsLegacyApi ] (Incompatible 3rd-Party)
```

### 3. Implementation in Modern C#

#### Step 1: The Target Contract
```csharp
public interface INotificationService
{
    Task SendAsync(string recipient, string message);
}
```

#### Step 2: The Adaptee (External Class)
```csharp
public class SpeedySmsLegacyApi
{
    public bool DispatchMessageV2(long internationalPhoneNumber, string text, int priorityFlag, bool isFlash)
    {
        Console.WriteLine($"[Vendor API] Dispatched to +{internationalPhoneNumber}: \"{text}\" (Priority: {priorityFlag})");
        return true;
    }
}
```

#### Step 3: The Adapter
```csharp
public class SpeedySmsAdapter : INotificationService
{
    private readonly SpeedySmsLegacyApi _legacyApi;

    public SpeedySmsAdapter(SpeedySmsLegacyApi legacyApi)
    {
        _legacyApi = legacyApi;
    }

    public Task SendAsync(string recipient, string message)
    {
        // 1. Adapt and translate inputs: strip out non-digits
        string cleanPhone = new string(recipient.Where(char.IsDigit).ToArray());
        
        if (!long.TryParse(cleanPhone, out long parsedPhone))
        {
            throw new ArgumentException($"Invalid phone number format: {recipient}");
        }

        // 2. Map standard call to vendor parameters
        int normalPriority = 1;
        bool isFlash = false;

        bool success = _legacyApi.DispatchMessageV2(parsedPhone, message, normalPriority, isFlash);

        if (!success)
        {
            throw new InvalidOperationException("Failed to dispatch SMS through vendor gateway.");
        }

        return Task.CompletedTask;
    }
}
```

#### Step 4: Consuming in Business Logic
```csharp
public class OrderService
{
    private readonly INotificationService _notifier;

    public OrderService(INotificationService notifier)
    {
        _notifier = notifier;
    }

    public async Task CompleteOrderAsync(string customerPhone, decimal amount)
    {
        Console.WriteLine($"Order of ${amount} completed.");
        await _notifier.SendAsync(customerPhone, $"Your order of ${amount} is confirmed!");
    }
}
```

#### In `Program.cs` (Dependency Injection):
```csharp
builder.Services.AddSingleton<SpeedySmsLegacyApi>();
builder.Services.AddScoped<INotificationService, SpeedySmsAdapter>();
builder.Services.AddScoped<OrderService>();
```
*If your company switches to Twilio next year, write `TwilioAdapter : INotificationService` and change 1 line in `Program.cs`.*

### 4. The Architect's View: Anti-Corruption Layer (ACL)
In **Domain-Driven Design (DDD)** or **Microservices**, the Adapter is the building block of an **Anti-Corruption Layer (ACL)**. It prevents external models, weird schemas, or legacy SOAP XML structures from corrupting your clean domain entities.

### Summary: When to use Adapter

| Use Adapter When... | Don't Use Adapter When... |
| :--- | :--- |
| Integrating a 3rd-party library/SDK whose interface doesn't match your design | You have full control over both classes and can simply change them directly |
| Integrating legacy code without rewriting or breaking it | You want to add *new behaviors* (use **Decorator** instead) |
| Creating an Anti-Corruption Layer between systems | You want to simplify a complex multi-class subsystem (use **Facade** instead) |

---

## Pattern 2: Decorator (Structural)

### 1. What is the Problem? (The Real-World Pain: Inheritance Explosion)
Imagine you have a service that fetches weather data from an expensive external API:

```csharp
public interface IWeatherService
{
    Task<string> GetForecastAsync(string city);
}

public class ApiWeatherService : IWeatherService
{
    public async Task<string> GetForecastAsync(string city)
    {
        await Task.Delay(1000); 
        return $"Sunny, 25°C in {city}";
    }
}
```

Now, the business comes with new requirements:
1. *"We need in-memory **Caching** so we don't hit the API every second."*
2. *"We need **Telemetry/Logging** to measure how long calls take."*
3. *"We need **Resilience/Retry** if the network drops."*

#### The Junior Developer's Instinct: Use Inheritance!
* `CachedWeatherService : ApiWeatherService`
* `LoggedWeatherService : ApiWeatherService`
* `CachedAndLoggedWeatherService : CachedWeatherService`
* `RetryCachedAndLoggedWeatherService : ...`

**The Disaster:**
* **Class Explosion:** 3 features lead to 8 combinations of classes. 5 features lead to 32 subclasses!
* **Violates SRP:** Weather logic gets polluted with caching dictionaries, stopwatches, and retry loops.
* **Brittle:** If you replace `ApiWeatherService` with `DatabaseWeatherService`, none of your subclasses work with it!

### 2. The Core Concept (Plain English)
> **"Attach new responsibilities to an object dynamically by wrapping it inside a decorator that implements the exact same interface."**

**Analogy: Winter Clothing or Coffee Add-ons**  
Think of ordering coffee at Starbucks:
1. You start with a **Plain Coffee** ($2.00).
2. You wrap it with **Milk** (adds $0.50).
3. You wrap it with **Caramel Syrup** (adds $0.75).
4. You wrap it with **Whipped Cream** (adds $0.50).  
Each layer adds new flavor and cost, but to the customer, **the entire cup is still a `Coffee`**.

```
[ LoggingDecorator ]
   └── [ CachingDecorator ]
          └── [ Real ApiWeatherService ]
```

### 3. Implementation in Modern C#

#### Step 1: The Base Product
```csharp
public interface IWeatherService
{
    Task<string> GetForecastAsync(string city);
}

public class ApiWeatherService : IWeatherService
{
    public async Task<string> GetForecastAsync(string city)
    {
        Console.WriteLine($"[API] Fetching fresh data from satellite for '{city}'...");
        await Task.Delay(500); 
        return $"Sunny, 24°C in {city}";
    }
}
```

#### Step 2: Decorator #1 (Adding Caching)
```csharp
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
```

#### Step 3: Decorator #2 (Adding Performance Logging)
```csharp
using System.Diagnostics;

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
```

### 4. Assembling and Running the Decorators
```csharp
class Program
{
    static async Task Main()
    {
        IWeatherService weatherService = 
            new LoggingWeatherService(
                new CachedWeatherService(
                    new ApiWeatherService()
                )
            );

        Console.WriteLine("--- Request 1 (Cache is cold) ---");
        var result1 = await weatherService.GetForecastAsync("London");

        Console.WriteLine("\n--- Request 2 (Cache is warm) ---");
        var result2 = await weatherService.GetForecastAsync("London");
    }
}
```

#### Output:
```text
--- Request 1 (Cache is cold) ---
[API] Fetching fresh data from satellite for 'London'...
[Telemetry] Request for 'London' took 512ms.

--- Request 2 (Cache is warm) ---
[Cache HIT] Returning cached data for 'London'.
[Telemetry] Request for 'London' took 0ms.
```

### 5. The Architect's View: How .NET Uses Decorators Everywhere
* **.NET IO Streams:**
  ```csharp
  Stream fileStream = File.OpenWrite("data.bin");
  Stream gzipStream = new GZipStream(fileStream, CompressionMode.Compress);
  Stream cryptoStream = new CryptoStream(gzipStream, encryptor, CryptoStreamMode.Write);
  ```
* **Clean DI with "Scrutor":**
  ```csharp
  builder.Services.AddScoped<IWeatherService, ApiWeatherService>();
  builder.Services.Decorate<IWeatherService, CachedWeatherService>();
  builder.Services.Decorate<IWeatherService, LoggingWeatherService>();
  ```

---

> [!NOTE]
> **Decorator vs. Proxy Comparison:**  
> Wondering why the Decorator and Proxy patterns look so similar structurally and how they differ architecturally?  
> See the detailed breakdown, lifecycle comparison, and comparison matrix in the [Proxy Pattern Deep Dive: Decorator vs. Proxy](#deep-dive-qa-decorator-vs-proxy-why-they-look-similar--their-4-fundamental-architectural-differences).

---

## Pattern 3: Facade (Structural)

### 1. What is the Problem? (The Real-World Pain)
Imagine an **E-Commerce Checkout API**. To place an order, you must coordinate 5 subsystems:
1. Check product inventory in warehouse.
2. Calculate sales tax.
3. Charge the customer's credit card.
4. Reserve shipping with FedEx/DHL.
5. Send an order confirmation email.

#### Without a Facade (The Messy Controller):
```csharp
// ❌ Controller is drowning in dependencies and orchestration logic!
public class CheckoutController : ControllerBase
{
    private readonly IInventoryService _inventory;
    private readonly ITaxCalculator _tax;
    private readonly IPaymentService _payment;
    private readonly IShippingService _shipping;
    private readonly IEmailService _email;

    public CheckoutController(
        IInventoryService inventory, ITaxCalculator tax, 
        IPaymentService payment, IShippingService shipping, IEmailService email)
    {
        _inventory = inventory; _tax = tax; _payment = payment; _shipping = shipping; _email = email;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CheckoutRequest request)
    {
        if (!await _inventory.IsInStockAsync(request.ProductId, request.Quantity)) 
            return BadRequest("Out of stock");

        var taxAmount = await _tax.CalculateTaxAsync(request.Amount, request.Country);
        var total = request.Amount + taxAmount;

        var paymentSuccess = await _payment.ChargeAsync(request.CardToken, total);
        if (!paymentSuccess) return BadRequest("Payment failed");

        var trackingNumber = await _shipping.BookShipmentAsync(request.Address, request.ProductId);
        await _email.SendConfirmationAsync(request.Email, trackingNumber);

        return Ok(new { TrackingNumber = trackingNumber });
    }
}
```

### 2. The Core Concept (Plain English)
> **"Provide a unified, higher-level interface to a set of interfaces in a subsystem. A Facade makes the subsystem easier to use by hiding its complexity behind one simple method call."**

**Analogy: The Hotel Front Desk (Concierge)**  
You don't call the kitchen, wine cellar, valet, and housekeeping individually. You pick up the phone, dial **`0` for the Front Desk (The Facade)**, and say: *"Book me a romantic dinner and arrange a car for 8 PM."*

### 3. Implementation in Modern C#

#### Step 1: Subsystems
```csharp
public class InventoryService
{
    public bool CheckStock(string sku, int qty) => true;
    public void DeductStock(string sku, int qty) => Console.WriteLine($"[Warehouse] {qty} units of {sku} reserved.");
}

public class PaymentService
{
    public bool ProcessPayment(string cardToken, decimal amount)
    {
        Console.WriteLine($"[Stripe] Successfully charged ${amount}.");
        return true;
    }
}

public class ShippingService
{
    public string GenerateLabel(string address) => "FEDEX-TRACK-99482";
}

public class NotificationService
{
    public void SendReceipt(string email, string tracking) => Console.WriteLine($"[Email] Receipt sent to {email}");
}
```

#### Step 2: The Facade
```csharp
public interface IOrderProcessingFacade
{
    Task<OrderResult> PlaceOrderAsync(OrderRequest request);
}

public class OrderProcessingFacade : IOrderProcessingFacade
{
    private readonly InventoryService _inventory;
    private readonly PaymentService _payment;
    private readonly ShippingService _shipping;
    private readonly NotificationService _notification;

    public OrderProcessingFacade(
        InventoryService inventory, PaymentService payment, 
        ShippingService shipping, NotificationService notification)
    {
        _inventory = inventory; _payment = payment; _shipping = shipping; _notification = notification;
    }

    public async Task<OrderResult> PlaceOrderAsync(OrderRequest request)
    {
        if (!_inventory.CheckStock(request.Sku, request.Quantity))
            return OrderResult.Failed("Item out of stock.");

        _inventory.DeductStock(request.Sku, request.Quantity);

        var paid = _payment.ProcessPayment(request.PaymentToken, request.TotalAmount);
        if (!paid) return OrderResult.Failed("Payment declined.");

        var tracking = _shipping.GenerateLabel(request.ShippingAddress);
        _notification.SendReceipt(request.CustomerEmail, tracking);

        return OrderResult.Success(tracking);
    }
}
```

#### Step 3: Clean Calling Code (Only 1 Dependency!)
```csharp
public class CheckoutController : ControllerBase
{
    private readonly IOrderProcessingFacade _orderFacade;

    public CheckoutController(IOrderProcessingFacade orderFacade) => _orderFacade = orderFacade;

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(OrderRequest request)
    {
        var result = await _orderFacade.PlaceOrderAsync(request);
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return Ok(new { TrackingNumber = result.TrackingNumber });
    }
}
```

---

### Deep Dive Q&A: Does Facade just move complexity?

> **Question:** Doesn't this just move all the complexity from checkout controller to the facade?
> - *Duplication:* What if tomorrow you add a Mobile App, a Batch Order processor, or an Admin Portal? You have to copy-paste this 40-line sequence into three other places!
> - *Fragile:* If the payment step needs a new fraud-check parameter next month, you have to find and update every single place where checkout was written.

**Answer:**
**Yes, absolutely! You are 100% correct.** The complexity didn't disappear into thin air.
> **Law of Conservation of Complexity:** Every system has an inherent amount of irreducible complexity. You cannot eliminate it; you can only decide **where it lives** and **how many times it is repeated**.

#### 1. Centralization vs. Sprawl (The 1 Place vs. 4 Places Rule)
```
WITHOUT FACADE:
[ Web Controller ]      ──> 40 lines (Inventory -> Tax -> Pay -> Ship -> Email)
[ Mobile API ]          ──> 40 lines (Copied & Pasted)
[ Batch CSV Importer ]  ──> 40 lines (Copied & Pasted)
[ Admin Phone Orders ]  ──> 40 lines (Copied & Pasted)

WITH FACADE:
[ Web Controller ]     ───┐
[ Mobile API ]         ───┼──> [ IOrderProcessingFacade ] ──> 40 lines in ONE place!
[ Batch CSV Importer ] ───┤           │
[ Admin Phone Orders ] ───┘           └── Coordinates subsystems
```
If a fraud-check step is added, you edit **one file**, and all 4 entry points instantly inherit the fix.

#### 2. Separation of Concerns (HTTP vs. Business Logic)
A controller's job is **HTTP communication** (headers, tokens, HTTP status codes), not database updates or warehouse logic. By moving it to a Facade, any caller (Console app, Azure Function, RabbitMQ worker) can reuse it.

#### 3. Unit Testing Becomes 10x Easier
- **Without Facade:** You must mock 5 services in every controller test.
- **With Facade:** Mock **1 interface** (`Mock<IOrderProcessingFacade>`).

---

## Pattern 4: Proxy (Structural)

### 1. What is the Problem? (The Real-World Pain)
Imagine a **Confidential Document Management System** (`RealDocumentService`). You need:
1. **Security / Access Control:** Only users with `"Admin"` or `"HRManager"` roles can view documents.
2. **Auditing:** Every single view attempt must be logged for compliance.
3. **Lazy Loading:** Avoid loading heavy 50MB PDF rendering engines into memory until the user actually views a document.

### 2. The Core Concept (Plain English)
> **"Provide a surrogate or placeholder for another object to control access to it."**

**Analogy: The Bodyguard / Executive Assistant**  
You don't walk directly into the CEO's office. You speak to the **Executive Assistant (The Proxy)** first. The assistant checks your identity. If authorized, they let you in. If not, you are turned away before wasting the CEO's time.

```
[ Client Code ] ──> ( IDocumentService )
                           │
                           ▼
                 [ DocumentServiceProxy ]  <-- Checks permissions / audits
                           │
                           ▼ (If authorized)
                 [ RealDocumentService ]   <-- Does actual heavy work
```

### 3. Implementation in Modern C#
```csharp
public interface IDocumentService
{
    void DisplayDocument(string documentId);
}

public class RealDocumentService : IDocumentService
{
    public RealDocumentService() => Console.WriteLine("[RealDocumentService] Initializing PDF rendering engine...");
    public void DisplayDocument(string id) => Console.WriteLine($"[RealDocumentService] Rendering: '{id}'");
}

public class DocumentServiceProxy : IDocumentService
{
    private RealDocumentService? _realService; // Lazy loaded!
    private readonly string _currentUserRole;

    public DocumentServiceProxy(string currentUserRole) => _currentUserRole = currentUserRole;

    public void DisplayDocument(string documentId)
    {
        Console.WriteLine($"[Audit Log] User with role '{_currentUserRole}' requested doc '{documentId}'.");

        if (_currentUserRole != "Admin" && _currentUserRole != "HRManager")
        {
            Console.WriteLine($"[Access Denied] User '{_currentUserRole}' unauthorized!\n");
            return;
        }

        _realService ??= new RealDocumentService(); // Virtual Proxy: Lazy loading!
        _realService.DisplayDocument(documentId);
    }
}
```

#### Output:
```text
--- Scenario 1: Regular Employee attempts access ---
[Audit Log] User with role 'Employee' requested doc 'Salary_Q3_2026.pdf'.
[Access Denied] User 'Employee' is unauthorized to view confidential docs!

--- Scenario 2: HR Manager attempts access ---
[Audit Log] User with role 'HRManager' requested doc 'Salary_Q3_2026.pdf'.
[RealDocumentService] Initializing PDF rendering engine...
[RealDocumentService] Rendering document contents for: 'Salary_Q3_2026.pdf'
```

---

### Deep Dive Q&A: How to configure Dependency Injection for Proxy in ASP.NET Core

> **Question:** How will DI work for this?

In ASP.NET Core, user roles should be read dynamically from `IHttpContextAccessor`, and the real service kept protected or lazy:

```csharp
public class DocumentServiceProxy : IDocumentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private RealDocumentService? _realService;

    public DocumentServiceProxy(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public void DisplayDocument(string documentId)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || (!user.IsInRole("Admin") && !user.IsInRole("HRManager")))
            throw new UnauthorizedAccessException("Unauthorized.");

        // Lazy resolution from DI on demand:
        _realService ??= _serviceProvider.GetRequiredKeyedService<RealDocumentService>("real");
        _realService.DisplayDocument(documentId);
    }
}
```

#### Registration in `Program.cs` (.NET 8/9 Keyed Services):
```csharp
builder.Services.AddHttpContextAccessor();
builder.Services.AddKeyedScoped<RealDocumentService>("real");
builder.Services.AddScoped<IDocumentService, DocumentServiceProxy>();
```

---

### Deep Dive Q&A: Decorator vs. Proxy: Why they look similar & their 4 fundamental architectural differences

> **Question:** Decorator design pattern and proxy design pattern look very similar.

**Answer:**
In C# code, both patterns often look almost **identical** structurally:
- Both implement the exact same interface as the target class.
- Both wrap an instance of that interface via composition.
- Both forward calls to the wrapped instance with logic before or after.

```csharp
// Structurally, both look like this:
public class Wrapper : IService
{
    private readonly IService _inner;
    public Wrapper(IService inner) => _inner = inner;

    public void DoWork()
    {
        // Do something before
        _inner.DoWork();
        // Do something after
    }
}
```

> **"Design patterns are distinguished by their INTENT, not their syntax."**

Here are the **4 fundamental architectural differences**:

#### 1. Intent: Enhancement vs. Access Control
- **Decorator:** Enhances an object by adding new features/behaviors dynamically (e.g., Starbucks coffee toppings: milk, caramel, whipped cream).
- **Proxy:** Controls, protects, delays, or coordinates access to an object (e.g., Nightclub bouncer / CEO executive assistant).

#### 2. Who Controls the Object's Lifecycle?
- **Decorator:** The caller creates the real object and passes it in (`new LoggingWeatherService(realService)`). The decorator doesn't care how the target was created.
- **Proxy:** The proxy often creates, delays, or manages the lifecycle internally (`_realService ??= new RealDocumentService()`). The client often does not even know the real object exists.

#### 3. Chaining: Nesting Dolls vs. Single Gatekeeper
- **Decorator:** Designed to be stacked recursively (`new RetryDecorator(new LoggingDecorator(new CachingDecorator(service))))`.
- **Proxy:** Typically a 1-to-1 surrogate. You don't stack 5 proxies in front of 1 object.

#### 4. Ability to Block Execution (Short-Circuiting)
- **Decorator:** Almost always delegates to the inner object to decorate its results.
- **Proxy:** Frequently aborts and never calls the real object (e.g., Protection Proxy throws `UnauthorizedException` without touching the real service; Remote Proxy makes an RPC network call instead of local execution).

#### Summary Comparison Matrix

| Criteria | Decorator | Proxy |
| :--- | :--- | :--- |
| **What is its job?** | **Enhance** functionality. | **Control access** / represent the object. |
| **Object Creation** | Created by caller and passed in. | Often created, delayed, or managed by the Proxy itself. |
| **Relationship** | Can be chained recursively ($N$ wrappers). | Usually 1-to-1 surrogate. |
| **Can it prevent execution?** | Rarely (usually executes both wrapper + inner). | Frequently (blocks unauthorized access, returns cached stubs). |
| **Everyday .NET Example** | `CryptoStream`, `GZipStream`, `Scrutor`. | EF Core Lazy Loading Proxies, Moq/NSubstitute mocks, gRPC client stubs. |

> **The Golden Rule to Remember:**  
> - If you wrap a class to **add flavors, features, or cross-cutting concerns** (and you could stack 3 of them) $\rightarrow$ **Decorator**.  
> - If you wrap a class to **act as a gatekeeper, security guard, lazy-loader, or network boundary** $\rightarrow$ **Proxy**.

---

## Pattern 5: Composite (Structural)

### 1. What is the Problem? (The Real-World Pain)
In an e-commerce store, a cart contains individual products (Mouse $25, Keyboard $75) and nested bundles (Gamer Pack = Mouse + Keyboard; Ultimate Office = Monitor + Gamer Pack). Without Composite, callers write messy recursive loops and type checks (`if (item is Bundle)`).

### 2. The Core Concept (Plain English)
> **"Compose objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions of objects uniformly."**

**Analogy: Shipping Boxes & Packing Slips**  
When a scale weighs an Amazon box, it doesn't care whether items are loose or packed inside 3 smaller nested boxes. It just asks the root package: *"What is your total weight?"*

```
                 [ Ultimate Desk Setup (Bundle) ]  <-- Composite
                   ├── Monitor ($300)              <-- Leaf
                   └── [ Gamer Pack (Bundle) ]     <-- Composite
                         ├── Keyboard ($50)        <-- Leaf
                         └── Mouse ($25)           <-- Leaf
```

### 3. Implementation in Modern C#
```csharp
public interface ICatalogItem
{
    string Name { get; }
    decimal GetPrice();
    void Display(int indent = 0);
}

public class Product : ICatalogItem // Leaf
{
    public string Name { get; }
    private readonly decimal _price;
    public Product(string name, decimal price) { Name = name; _price = price; }
    public decimal GetPrice() => _price;
    public void Display(int indent = 0) => Console.WriteLine($"{new string(' ', indent * 2)}- {Name} (${_price})");
}

public class ProductBundle : ICatalogItem // Composite
{
    public string Name { get; }
    private readonly List<ICatalogItem> _items = new();
    private readonly decimal _discountPercentage;

    public ProductBundle(string name, decimal discount = 0) { Name = name; _discountPercentage = discount; }
    public void Add(ICatalogItem item) => _items.Add(item);

    public decimal GetPrice()
    {
        decimal subTotal = _items.Sum(i => i.GetPrice());
        return subTotal - (subTotal * (_discountPercentage / 100m));
    }

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent * 2)}+ [BUNDLE] {Name} (Discount: {_discountPercentage}%)");
        _items.ForEach(i => i.Display(indent + 1));
    }
}
```

#### Output:
```text
+ [BUNDLE] Ultimate Office Setup (Discount: 5%)
  - 4K Monitor ($400)
  + [BUNDLE] Gamer Accessories Pack (Discount: 10%)
    - Gaming Mouse ($25)
    - Mechanical Keyboard ($75)

Final Calculated Total: $465.50
```

---

## Pattern 6: Bridge (Structural)

### 1. What is the Problem? (The Real-World Pain: Cartesian Explosion)
You have 2 independent dimensions of change:
1. **Notification Types (Abstraction):** *System Alert*, *User Reminder*.
2. **Delivery Channels (Implementation):** *Email*, *SMS*, *Slack*.

With pure inheritance, 2 types × 3 channels = **6 classes** (`EmailSystemAlert`, `SmsSystemAlert`, etc.).  
4 types × 5 channels = **20 classes**! Every new delivery channel requires writing a subclass for every notification type.

### 2. The Core Concept (Plain English)
> **"Decouple an abstraction from its implementation so that the two can vary independently."**

**Analogy: Universal Remote Control & Appliances**  
A remote control (Abstraction) communicates with TVs, Soundbars, and Projectors (Implementations) via a standard protocol (The Bridge). You don't manufacture a hardwired `SimpleSonyTvRemote`.

```
ABSTRACTION HIERARCHY                    IMPLEMENTATION HIERARCHY
(Business Concept)                        (Platform / Channel)

 Notification (Base)                     IMessageSender (Interface)
   ├── SystemAlert                       ├── EmailSender
   └── UserReminder                      ├── SmsSender
          │                              └── SlackSender
          └─── Has a reference to ───────► (The Bridge)
```

### 3. Implementation in Modern C#
```csharp
public interface IMessageSender
{
    void SendMessage(string title, string body);
}

public class EmailSender : IMessageSender
{
    public void SendMessage(string title, string body) => Console.WriteLine($"[EMAIL] Subject: '{title}' | Body: {body}");
}

public class SmsSender : IMessageSender
{
    public void SendMessage(string title, string body) => Console.WriteLine($"[SMS] Alert: {title} - {body}");
}

public abstract class Notification
{
    protected readonly IMessageSender _sender; // The Bridge
    protected Notification(IMessageSender sender) => _sender = sender;
    public abstract void Notify(string message);
}

public class SystemAlertNotification : Notification
{
    public SystemAlertNotification(IMessageSender sender) : base(sender) { }
    public override void Notify(string message) => _sender.SendMessage("CRITICAL ALERT", message.ToUpperInvariant());
}

public class UserReminderNotification : Notification
{
    public UserReminderNotification(IMessageSender sender) : base(sender) { }
    public override void Notify(string message) => _sender.SendMessage("Friendly Reminder", message);
}
```
*Math drops from multiplication ($N \times M$) to addition ($N + M$). Adding WhatsApp requires only 1 class instead of 4.*

---

## Pattern 7: Flyweight (Structural)

### 1. What is the Problem? (The Real-World Pain)
Your game map has **1,000,000 trees**. If each tree allocates its own 50KB 3D mesh texture:  
$$1,000,000 \times 50\text{ KB} \approx \mathbf{50\text{ GB RAM}}$$  
The application crashes with an `OutOfMemoryException`.

### 2. The Core Concept: Intrinsic vs. Extrinsic State
- **Intrinsic State (Shared / Flyweight):** Heavy, constant across all instances (Tree name, 3D mesh, bark texture).
- **Extrinsic State (Unique / Contextual):** Light, changes per instance (`X`, `Y` coordinates passed into `Draw(x, y)`).

**Analogy: Word Processor Font Glyphs**  
A 500-page book with 300,000 characters does not allocate 300,000 heavy graphical objects for the letter `'a'`. It creates **one single shared Flyweight** for `'a'`, and the 5,000 occurrences simply supply their coordinates.

### 3. Implementation in Modern C#
```csharp
public class TreeType // Flyweight
{
    public string Name { get; }
    public string Color { get; }
    public byte[] Texture3DData { get; }

    public TreeType(string name, string color, byte[] texture) { Name = name; Color = color; Texture3DData = texture; }
    public void Draw(int x, int y) => Console.WriteLine($"Drawing '{Name}' tree at ({x}, {y})");
}

public class TreeFactory
{
    private static readonly Dictionary<string, TreeType> _types = new();

    public static TreeType GetTreeType(string name, string color)
    {
        string key = $"{name}_{color}";
        if (!_types.TryGetValue(key, out var type))
        {
            type = new TreeType(name, color, new byte[50 * 1024]); // 50KB asset
            _types[key] = type;
        }
        return type;
    }
}

public class Tree
{
    private readonly int _x; private readonly int _y;
    private readonly TreeType _type; // Pointer to shared flyweight

    public Tree(int x, int y, TreeType type) { _x = x; _y = y; _type = type; }
    public void Render() => _type.Draw(_x, _y);
}
```

#### Memory Savings:
| Without Flyweight | With Flyweight |
| :--- | :--- |
| 1,000,000 separate 50 KB textures | **2 shared 50 KB textures** = 100 KB |
| 1,000,000 coordinate pairs | 1,000,000 coordinate pairs = ~16 MB |
| **Total: ~50 GB RAM (Crash!)** | **Total: ~16.1 MB RAM (Fast!)** |

---

### Deep Dive Q&A: Prototype vs. Flyweight: Why both exist & the 4 fundamental differences

> **Question:** Flyweight and Prototype seem to fix the same problem. Why do we need two?

**Answer:**
At first glance, both patterns seem similar because **both deal with managing thousands of similar objects efficiently and avoiding heavy instantiation overhead**.

However, they solve completely different architectural problems through opposite mechanics:

> **The 1-Sentence Golden Rule:**  
> - **Prototype** produces **independent, distinct copies** of an object (`Clone()`) so each can be mutated separately without re-running expensive initialization logic.  
> - **Flyweight** shares **one single immutable object** across thousands of contexts to prevent out-of-memory (OOM) crashes ($O(1)$ memory).

#### Real-World Analogy
* **Prototype is a Photocopy:**  
  You take a master job application form and photocopy it 50 times. Each applicant gets their **own paper**. Applicant A can write their name, strike out lines, or spill coffee on their copy—it has zero impact on Applicant B.
* **Flyweight is a Highway Billboard:**  
  10,000 drivers drive down a highway and look at the **exact same billboard**. You do **not** construct 10,000 identical billboards. The billboard itself is immutable (constant), while each driver has their own car speed and GPS coordinates (extrinsic context).

#### The 4 Fundamental Architectural Differences

##### 1. Creational vs. Structural
* **Prototype (Creational):** Solves **how objects are created**. Instead of calling a slow constructor (e.g., parsing a 50MB configuration file, compiling shaders, or querying a database), you clone an existing pre-warmed instance.
* **Flyweight (Structural):** Solves **how objects are structured in memory**. Instead of allocating 1,000,000 objects on the heap, you allocate **one** and reference it 1,000,000 times.

##### 2. Number of Objects in RAM
* **Prototype:** If you request 100,000 units, you get **100,000 distinct objects** allocated on the heap ($O(N)$ memory).
* **Flyweight:** If you render 100,000 forest trees, you have **1 shared `TreeType` object** in memory and 100,000 tiny coordinate structs $(X, Y)$ ($O(1)$ intrinsic memory).

##### 3. Mutability & Independence
* **Prototype:** The cloned object is **fully mutable and independent**. Changing a clone's health, color, or weapon has zero effect on the prototype or other clones.
* **Flyweight:** The flyweight object **MUST BE IMMUTABLE**. Because 100,000 callers share that exact same reference, modifying it would corrupt all callers across the entire application.

##### 4. Intrinsic vs. Extrinsic State
* **Prototype:** Carries all its own data internally within its private fields.
* **Flyweight:** Splits state into two parts:
  * **Intrinsic (Shared):** Texture, 3D Mesh, Audio clips (stored inside the Flyweight).
  * **Extrinsic (Contextual):** Position `(X, Y, Z)`, current velocity (kept by the caller and passed in as method arguments).

#### Side-by-Side Code Comparison (C#)

##### Prototype: Cloning to create independent, mutable copies
```csharp
// Expensive initialization done once
var baseGoblin = new MonsterPrototype("Goblin", maxHealth: 100, Load3DModelFromDisk("goblin.fbx"));

// Creating 2 distinct goblins via Prototype
var goblin1 = (MonsterPrototype)baseGoblin.Clone();
var goblin2 = (MonsterPrototype)baseGoblin.Clone();

// Changing goblin1 DOES NOT affect goblin2
goblin1.CurrentHealth = 20; // goblin2 still has 100
```

##### Flyweight: Sharing one instance to save RAM
```csharp
// Shared Flyweight: 1 instance in memory (~10 MB 3D model & textures)
public class TreeType
{
    public string Name { get; }
    public byte[] MeshData { get; }

    public TreeType(string name, byte[] meshData) 
        => (Name, MeshData) = (name, meshData);

    // Extrinsic state (x, y) is passed in from outside!
    public void Draw(int x, int y) => RenderEngine.Draw(MeshData, x, y);
}

// Client holds only lightweight extrinsic data (16 bytes per tree)
public struct Tree
{
    public int X;
    public int Y;
    public TreeType SharedType; // Reference to the 1 flyweight
}

// 1,000,000 trees share ONE TreeType instance
var oakType = TreeFactory.GetTreeType("Oak", oakMeshBytes);
var forest = new List<Tree>();
for (int i = 0; i < 1_000_000; i++)
{
    forest.Add(new Tree { X = random.Next(), Y = random.Next(), SharedType = oakType });
}
```

#### Summary Comparison Matrix

| Feature | Prototype Pattern | Flyweight Pattern |
| :--- | :--- | :--- |
| **GoF Classification** | **Creational** | **Structural** |
| **Primary Goal** | Fast, flexible creation of new objects without invoking costly constructors. | Drastic RAM reduction when handling millions of fine-grained objects. |
| **Object Identity** | Unique instances (`ReferenceEquals(a, b) == false`). | Shared instance (`ReferenceEquals(a, b) == true`). |
| **State Mutability** | Clones can be freely mutated independently. | Flyweights **must be immutable**. |
| **Heap Memory Impact** | Increases linearly with every new clone ($O(N)$). | Constant memory footprint regardless of count ($O(1)$ for intrinsic data). |
| **Real-World .NET Examples** | `ICloneable`, `record with { ... }`, WPF / Avalonia template cloning. | `string.Intern()`, Font glyph caches, Roslyn Syntax Trees. |

#### Can they work together?
**Yes!** In game engines and high-performance systems, they often complement each other:
You can use **Prototype** to clone an NPC or game unit (so each unit has its own independent AI state, target, and health), while all clones hold a reference to the same **Flyweight** (3D mesh, texture, and animation rigs) to keep memory usage minimal.

---

# 4. Behavioral Design Patterns

## Overview of the 11 Classic GoF Behavioral Patterns

Behavioral Patterns are about **how objects communicate, collaborate, and distribute responsibilities**.

1. **Strategy:** The Swappable Brain / Algorithm.
2. **Observer:** The Newsletter Publisher / Event Hub.
3. **Command:** The Action Ticket / Undo-Redo.
4. **Mediator:** The Air Traffic Controller.
5. **Chain of Responsibility:** The Escalation Pipeline / Middleware.
6. **State:** The Mood Ring / Finite State Machine.
7. **Template Method:** The Recipe Outline.
8. **Iterator:** The Tour Guide (`IEnumerable<T>`).
9. **Memento:** The Save-Game Checkpoint (Ctrl+Z).
10. **Visitor:** The Tax Auditor / Health Inspector.
11. **Interpreter:** Custom Language Evaluator.

---

## Pattern 1: Strategy (Behavioral)

### 1. What is the Problem? (The Real-World Pain)
Marketing asks for different discount rules (VIP, Black Friday, First-Time Buyer).
```csharp
// ❌ Disaster: Monolithic 60-line switch statement violating OCP
public decimal CalculateFinalPrice(Order order, DiscountType discountType)
{
    decimal finalPrice = order.SubTotal;
    switch (discountType)
    {
        case DiscountType.Vip: finalPrice -= order.SubTotal * 0.20m; break;
        case DiscountType.BlackFriday: if (order.SubTotal > 100) finalPrice -= 25m; break;
        // ...
    }
    return finalPrice;
}
```

### 2. The Core Concept (Plain English)
> **"Define a family of algorithms, put each of them into a separate class, and make their objects interchangeable."**

**Analogy: Navigation on Google Maps**  
Choose Driving, Walking, Bicycling, or Public Transit. Google Maps doesn't rewrite its whole app for each vehicle; it executes the chosen `IRouteStrategy`.

### 3. Implementation in Modern C#
```csharp
public interface IDiscountStrategy { decimal ApplyDiscount(decimal totalAmount); }

public class NoDiscountStrategy : IDiscountStrategy { public decimal ApplyDiscount(decimal t) => t; }
public class VipDiscountStrategy : IDiscountStrategy { public decimal ApplyDiscount(decimal t) => t * 0.80m; }
public class BlackFridayDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal t) => t > 100m ? t - 25m : t;
}

public class CheckoutService
{
    private IDiscountStrategy _discountStrategy;
    public CheckoutService(IDiscountStrategy s) => _discountStrategy = s;
    public void SetStrategy(IDiscountStrategy s) => _discountStrategy = s;
    public decimal Checkout(decimal cartTotal) => _discountStrategy.ApplyDiscount(cartTotal);
}
```
*Functional Shortcut: Use delegates (`Func<decimal, decimal>`) for inline strategies (e.g. LINQ's `.OrderBy(x => x.Age)`).*

---

## Pattern 2: Observer (Behavioral)

### 1. What is the Problem? (The Real-World Pain)
The price of Bitcoin updates every few seconds. Mobile apps, trading bots, and dashboards all need updates. Polling every 500ms wastes 99% of bandwidth. Hardcoding references inside `StockTicker` violates loose coupling.

### 2. The Core Concept (Plain English)
> **"Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically."**

**Analogy: YouTube Channel & Notification Bell 🔔**  
You don't drive to YouTube HQ daily. You hit "Subscribe". The creator uploads once, and YouTube pushes notifications to all subscribers.

```
                  ┌───> [ MobileAppSubscriber ]    (Observer 1)
[ StockTicker ] ──┼───> [ TradingBotSubscriber ]   (Observer 2)
  (Publisher)     └───> [ DashboardSubscriber ]    (Observer 3)
```

### 3. Implementation in Modern C#
```csharp
public interface IStockObserver { void OnPriceChanged(string symbol, decimal newPrice); }

public class StockTicker
{
    private readonly string _symbol;
    private decimal _price;
    private readonly List<IStockObserver> _observers = new();

    public StockTicker(string symbol, decimal price) { _symbol = symbol; _price = price; }
    public void Attach(IStockObserver o) => _observers.Add(o);
    public void Detach(IStockObserver o) => _observers.Remove(o);

    public void SetPrice(decimal newPrice)
    {
        if (_price != newPrice) { _price = newPrice; Notify(); }
    }

    private void Notify() => _observers.ForEach(o => o.OnPriceChanged(_symbol, _price));
}

public class MobileAppAlert : IStockObserver
{
    public void OnPriceChanged(string s, decimal p) => Console.WriteLine($"📱 [Mobile App] {s} is ${p}!");
}

public class AutoTradingBot : IStockObserver
{
    private readonly decimal _threshold;
    public AutoTradingBot(decimal t) => _threshold = t;
    public void OnPriceChanged(string s, decimal p)
    {
        if (p < _threshold) Console.WriteLine($"🤖 [Bot] BUY ORDER EXECUTED at ${p}!");
    }
}
```

### The #1 Memory Leak in C# (The Lapsed Listener Problem):
If `StockTicker` is a **Singleton**, and a short-lived transient service subscribes using:
```csharp
ticker.PriceChanged += myTransientService.OnPriceChanged;
```
The ticker holds a strong reference to `myTransientService`. The Garbage Collector **can never collect it**! Always unsubscribe (`-=`) inside `Dispose()`.

---

## Pattern 3: Command (Behavioral)

### 1. What is the Problem? (The Real-World Pain)
Calling `bankAccount.Withdraw(100)` directly executes and vanishes. You cannot store it in a queue, serialize it to JSON, or support an **Undo (Ctrl+Z)** button.

### 2. The Core Concept (Plain English)
> **"Encapsulate a request as a standalone object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations."**

**Analogy: Restaurant Order Ticket 🧾**  
You don't shout orders at the chef. The waiter writes an order ticket (The Command). It can sit in a queue, be audited, or be cancelled/torn up before cooking starts.

### 3. Implementation in Modern C# (With Full Undo)
```csharp
public interface ITransactionCommand
{
    bool Execute();
    void Undo();
}

public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }
    public BankAccount(string acc, decimal bal) { AccountNumber = acc; Balance = bal; }
    public void Deposit(decimal amt) => Balance += amt;
    public bool Withdraw(decimal amt)
    {
        if (Balance >= amt) { Balance -= amt; return true; }
        return false;
    }
}

public class DepositCommand : ITransactionCommand
{
    private readonly BankAccount _acc; private readonly decimal _amt;
    public DepositCommand(BankAccount acc, decimal amt) { _acc = acc; _amt = amt; }
    public bool Execute() { _acc.Deposit(_amt); return true; }
    public void Undo() { _acc.Withdraw(_amt); }
}

public class WithdrawCommand : ITransactionCommand
{
    private readonly BankAccount _acc; private readonly decimal _amt;
    private bool _ok;
    public WithdrawCommand(BankAccount acc, decimal amt) { _acc = acc; _amt = amt; }
    public bool Execute() { _ok = _acc.Withdraw(_amt); return _ok; }
    public void Undo() { if (_ok) _acc.Deposit(_amt); }
}

public class TransactionManager
{
    private readonly Stack<ITransactionCommand> _history = new();
    public void ExecuteTransaction(ITransactionCommand c) { if (c.Execute()) _history.Push(c); }
    public void UndoLastTransaction() { if (_history.Count > 0) _history.Pop().Undo(); }
}
```

---

## Pattern 4: Mediator (Behavioral)

### 1. Problem: The "Spiderweb" Coupling Chaos
When Plane A, Plane B, Plane C, Ground Crew, and Weather Service communicate directly with each other, 10 services create **45 direct connections**. Changing one class breaks 5 others.

### 2. The Core Concept (Plain English)
> **"Define an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly."**

**Analogy: The Air Traffic Control (ATC) Tower 🗼**  
Planes never radio each other. All planes speak **only** to the Control Tower (The Mediator).

```
[ Plane A ] ───┐               ┌───> [ Plane B ]
               ▼               │
       [ CONTROL TOWER ] ──────┤
          (Mediator)           │
[ Plane C ] ───┘               └───> [ Ground Crew ]
```

### 3. Classic GoF Implementation
```csharp
public interface IAirTrafficControl
{
    void RegisterFlight(Airplane plane);
    void SendMessage(string message, Airplane sender);
}

public abstract class Airplane
{
    protected readonly IAirTrafficControl _atc;
    public string CallSign { get; }
    protected Airplane(IAirTrafficControl atc, string callSign) { _atc = atc; CallSign = callSign; }
    public abstract void Receive(string message);
    public void Send(string msg) => _atc.SendMessage(msg, this);
}

public class AirTrafficControlTower : IAirTrafficControl
{
    private readonly List<Airplane> _planes = new();
    public void RegisterFlight(Airplane p) => _planes.Add(p);
    public void SendMessage(string msg, Airplane sender) =>
        _planes.Where(p => p != sender).ToList().ForEach(p => p.Receive(msg));
}
```

---

### Deep Dive: Air Traffic Control rewritten with Modern MediatR

> **Question:** Could you write the air traffic controller program in modern MediatR pattern?

In MediatR, you don't write a custom tower class. You define **Commands (1-to-1)**, **Notifications (1-to-many)**, and **Pipeline Behaviors**:

```csharp
using MediatR;

// 1. 1-to-1 Command
public record LandingClearance(bool IsApproved, string Runway, string Reason);
public record RequestLandingCommand(string FlightNumber, string AircraftType) : IRequest<LandingClearance>;

public class RequestLandingHandler : IRequestHandler<RequestLandingCommand, LandingClearance>
{
    public Task<LandingClearance> Handle(RequestLandingCommand req, CancellationToken ct) =>
        Task.FromResult(new LandingClearance(true, "Runway-26L (Long)", "Cleared to land."));
}

// 2. 1-to-Many Notification (Broadcast)
public record MaydayAlertNotification(string FlightNumber, string EmergencyType) : INotification;

public class EmergencyServicesHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification n, CancellationToken ct)
    {
        Console.WriteLine($"🚨 [FIRE & RESCUE] Dispatched for {n.FlightNumber}! Emergency: {n.EmergencyType}");
        return Task.CompletedTask;
    }
}

public class GroundOperationsHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification n, CancellationToken ct)
    {
        Console.WriteLine($"🛑 [GROUND OPS] Halting taxiing. Clearing runway for {n.FlightNumber}.");
        return Task.CompletedTask;
    }
}

// 3. Pipeline Behavior (Blackbox Flight Recorder)
public class FlightTelemetryBehavior<TReq, TResp> : IPipelineBehavior<TReq, TResp> where TReq : notnull
{
    public async Task<TResp> Handle(TReq req, RequestHandlerDelegate<TResp> next, CancellationToken ct)
    {
        Console.WriteLine($"📡 [RADAR] Intercepted: {typeof(TReq).Name}");
        var res = await next();
        Console.WriteLine($"📡 [RADAR] Processed successfully.");
        return res;
    }
}
```

#### Consuming via `IMediator`:
```csharp
// 1-to-1 Command:
var clearance = await mediator.Send(new RequestLandingCommand("Delta-101", "HeavyBoeing777"));

// 1-to-Many Broadcast:
await mediator.Publish(new MaydayAlertNotification("United-99", "Hydraulic Failure"));
```

---

## Pattern 5: Chain of Responsibility (Behavioral)

### 1. What is the Problem? (The Real-World Pain)
In an Order Processing API, an order must pass Auth $\rightarrow$ Rate Limiting $\rightarrow$ Validation $\rightarrow$ Fraud Check before saving. Writing this in one method violates SRP, is untestable, and makes reordering checks impossible.

### 2. The Core Concept (Plain English)
> **"Pass requests along a chain of handlers. Upon receiving a request, each handler decides either to process the request or to pass it to the next handler in the chain (or short-circuit and stop)."**

**Analogy: Airport Security Checkpoint 🛂**  
Station 1 (Boarding Pass) $\rightarrow$ Station 2 (Luggage X-Ray) $\rightarrow$ Station 3 (Passport Control). If your visa is denied at Station 3, you are stopped immediately (**short-circuit**).

```
Request ──> [ AuthHandler ] ──> [ RateLimitHandler ] ──> [ ValidationHandler ] ──> [ Execute Order ]
                 │                     │                          │
                 ▼ (If fails)          ▼ (If fails)               ▼ (If fails)
            (Short-circuit!)      (Short-circuit!)           (Short-circuit!)
```

### 3. Implementation in Modern C#
```csharp
public class OrderRequest { public string Token { get; set; } = ""; public string UserIp { get; set; } = ""; public decimal Amount { get; set; } }

public abstract class OrderHandler
{
    private OrderHandler? _next;
    public OrderHandler SetNext(OrderHandler next) { _next = next; return next; }
    public virtual void Handle(OrderRequest req) => _next?.Handle(req);
}

public class AuthenticationHandler : OrderHandler
{
    public override void Handle(OrderRequest req)
    {
        if (req.Token != "valid-jwt") { Console.WriteLine("🛑 [AuthHandler] Access Denied. (SHORT-CIRCUITED)"); return; }
        base.Handle(req);
    }
}

public class RateLimitingHandler : OrderHandler
{
    public override void Handle(OrderRequest req)
    {
        if (req.UserIp == "192.168.1.50") { Console.WriteLine("🛑 [RateLimitHandler] Rate limited. (SHORT-CIRCUITED)"); return; }
        base.Handle(req);
    }
}

public class ValidationHandler : OrderHandler
{
    public override void Handle(OrderRequest req)
    {
        if (req.Amount <= 0) { Console.WriteLine("🛑 [ValidationHandler] Invalid amount. (SHORT-CIRCUITED)"); return; }
        base.Handle(req);
    }
}

public class OrderExecutionHandler : OrderHandler
{
    public override void Handle(OrderRequest req) => Console.WriteLine($"🎉 Order of ${req.Amount} successfully executed!");
}
```
*Connecting the chain:*
```csharp
auth.SetNext(rateLimit).SetNext(validation).SetNext(execution);
```
*In ASP.NET Core, the **Middleware Pipeline (`app.Use(...)`)** is an asynchronous Chain of Responsibility.*

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
