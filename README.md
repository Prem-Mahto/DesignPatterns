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
     - *Deep Dive: Strategy vs Factory & Modern C# Functional Strategy (Lambdas)*
   - [Pattern 2: Observer Pattern](#pattern-2-observer-behavioral)
     - *Deep Dive: The Lapsed Listener Problem (#1 Memory Leak in C#) & Language Events*
   - [Pattern 3: Command Pattern (With Undo/Redo)](#pattern-3-command-behavioral)
     - *Deep Dive: Banking Engine with Undo/Redo & CQRS Job Queues*
   - [Pattern 4: Mediator Pattern](#pattern-4-mediator-behavioral)
     - *Summary Comparison: Mediator vs. Facade vs. Observer*
     - *Deep Dive: Air Traffic Control rewritten with Modern MediatR (Commands, Notifications, Pipeline Behaviors)*
     - *[Deep Dive: MediatR Multi-Pattern Architecture (Mediator vs. Observer vs. CQRS vs. Command vs. Chain of Responsibility)](#deep-dive-mediatr-multi-pattern-architecture-mediator-vs-observer-vs-cqrs-vs-command-vs-chain-of-responsibility)*
   - [Pattern 5: Chain of Responsibility Pattern](#pattern-5-chain-of-responsibility-behavioral)
     - *Summary Comparison: Chain of Responsibility vs. Decorator*
   - [Pattern 6: State Pattern (Finite State Machine)](#pattern-6-state-behavioral)
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
   - [Pattern 4: The Result Pattern (Railway-Oriented Programming)](#net-pattern-4-the-result-pattern-railway-oriented-programming)
   - [Pattern 5: The Specification Pattern](#net-pattern-5-the-specification-pattern)
   - [Pattern 6: Resilience Patterns (Circuit Breaker & Retry with Polly)](#net-pattern-6-resilience-patterns-circuit-breaker--retry)
   - [Pattern 7: The Transactional Outbox Pattern](#net-pattern-7-the-transactional-outbox-pattern)
   - [Pattern 8: CQRS (Command Query Responsibility Segregation)](#net-pattern-8-cqrs-command-query-responsibility-segregation)
6. [Tier 2: High-Impact Enterprise & Distributed Architecture Patterns](#6-tier-2-high-impact-enterprise--distributed-architecture-patterns)
   - [Pattern 1: The Saga Pattern (Distributed Transactions)](#tier-2-pattern-1-the-saga-pattern-distributed-transactions)
   - [Pattern 2: The Idempotency Pattern (Idempotent Consumer & API Key)](#tier-2-pattern-2-the-idempotency-pattern-idempotent-consumer--api-key)
   - [Pattern 3: Template Method Pattern (GoF Behavioral)](#tier-2-pattern-3-template-method-pattern-gof-behavioral)
   - [Pattern 4: Anti-Corruption Layer (ACL - Domain-Driven Design)](#tier-2-pattern-4-anti-corruption-layer-acl---domain-driven-design)
   - [Pattern 5: The Strangler Fig Pattern (Legacy Modernization)](#tier-2-pattern-5-the-strangler-fig-pattern-legacy-modernization)
   - [Summary Comparison: When to Use Which Tier 2 Pattern](#summary-comparison-when-to-use-which-tier-2-pattern)

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
// âŒ BAD: Tightly coupled, violates Open/Closed Principle
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
* **Pros:** Extremely simple. Perfect when you only have 2â€“3 types that rarely change.
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
// âŒ DISASTER: The "Frankenstein" Cloud State
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
        queue.Publish("document-uploaded"); // ðŸ’¥ CRASH!
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

**The Real-World Analogy: Interior Design Furniture Suites ðŸ›‹ï¸**
* **Family 1 (Victorian Style):** Victorian Sofa, Victorian Coffee Table, Victorian Chair.
* **Family 2 (Modern Art Deco):** Art Deco Glass Sofa, Art Deco Steel Coffee Table, Art Deco Neon Chair.

If you put an ornate, floral Victorian Velvet Sofa next to an ultra-futuristic Art Deco Neon Chair, your living room looks mismatched. 
The **Abstract Factory** guarantees:
> *"If you choose the Victorian suite, EVERY piece of furniture delivered to your house is guaranteed to be Victorian. You will never accidentally receive a neon steel table."*

```
                         [ ICloudServiceFactory ] (Abstract Factory)
                                    â”‚
           â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”´â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
           â–¼                                                 â–¼
[ AwsServiceFactory ]                             [ AzureServiceFactory ]
 (Concrete Factory 1)                              (Concrete Factory 2)
   â”œâ”€â”€ Creates: S3Storage                            â”œâ”€â”€ Creates: AzureBlobStorage
   â”œâ”€â”€ Creates: SqsQueue                             â”œâ”€â”€ Creates: AzureServiceBusQueue
   â””â”€â”€ Creates: DynamoDbDatabase                     â””â”€â”€ Creates: CosmosDbDatabase
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
        Console.WriteLine($"ðŸ“¦ [AWS S3] Uploaded '{fileName}' ({data.Length} bytes) to bucket 'production-docs'.");
}

public class AwsSqsQueue : IMessageQueue
{
    public void EnqueueMessage(string message) =>
        Console.WriteLine($"ðŸ“¬ [AWS SQS] Enqueued message to 'arn:aws:sqs:us-east-1:order-events': \"{message}\"");
}

public class AwsDynamoDatabase : IAuditDatabase
{
    public void RecordLog(string action) =>
        Console.WriteLine($"ðŸ—„ï¸ [AWS DynamoDB] Wrote audit entry: '{action}' to table 'SystemAuditLog'.");
}
```

##### Step 3: Concrete Products for Family 2 (Azure Family)
```csharp
public class AzureBlobStorage : IFileStorage
{
    public void UploadFile(string fileName, byte[] data) =>
        Console.WriteLine($"ðŸ“¦ [Azure Blob] Uploaded '{fileName}' ({data.Length} bytes) to container 'client-assets'.");
}

public class AzureServiceBusQueue : IMessageQueue
{
    public void EnqueueMessage(string message) =>
        Console.WriteLine($"ðŸ“¬ [Azure Service Bus] Published to topic 'orders-topic': \"{message}\"");
}

public class AzureCosmosDatabase : IAuditDatabase
{
    public void RecordLog(string action) =>
        Console.WriteLine($"ðŸ—„ï¸ [Azure CosmosDB] Inserted document partition 'Audit': '{action}'.");
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
ðŸ“¦ [AWS S3] Uploaded 'Quarterly_Report.pdf' (4 bytes) to bucket 'production-docs'.
ðŸ“¬ [AWS SQS] Enqueued message to 'arn:aws:sqs:us-east-1:order-events': "FileReady:Quarterly_Report.pdf"
ðŸ—„ï¸ [AWS DynamoDB] Wrote audit entry: 'Uploaded and enqueued Quarterly_Report.pdf at 12:45:00' to table 'SystemAuditLog'.

=== DEPLOYMENT 2: AZURE ENVIRONMENT ===
--- Processing Document: Tax_Statement_2026.pdf ---
ðŸ“¦ [Azure Blob] Uploaded 'Tax_Statement_2026.pdf' (4 bytes) to container 'client-assets'.
ðŸ“¬ [Azure Service Bus] Published to topic 'orders-topic': "FileReady:Tax_Statement_2026.pdf"
ðŸ—„ï¸ [Azure CosmosDB] Inserted document partition 'Audit': 'Uploaded and enqueued Tax_Statement_2026.pdf at 12:45:00'.
```

#### 4. The Architect's Deep Dive: Trade-offs & The Achilles' Heel âš ï¸

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
// âŒ The "Telescoping Constructor" Nightmare
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
4. **The "Setter" Problem:** If you make all properties `public { get; set; }` to avoid constructors, your object is **mutable and unsafe**â€”anyone can mutate fields halfway through, leaving the object in an invalid, half-baked state.

---

### 2. The Core Concept (Plain English)

> **"Separate the construction of a complex object from its representation, allowing you to produce different variations step-by-step."**

**Analogy: Customizing a Subway Sandwich or a Gaming PC**  
You donâ€™t ask the counter clerk for a pre-made sandwich that comes with 20 ingredients where you have to say *"no onions, no pickles, no mustard"*. 
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
* **The Problem:** You have two independent dimensions that can grow indefinitely (e.g., Shapes: *Circle, Square, Triangle* and Renderers: *DirectX, OpenGL, Vulkan*). If you use inheritance, you end up with 3 Ã— 3 = 9 classes (`DirectXCircle`, `OpenGLCircle`, etc.).
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
// âŒ Third-party / Legacy code (you CANNOT change this class!):
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
- You buy a **Â£5 plug adapter** that sits in the middle: your laptop plugs into the adapter, and the adapter plugs into the wall.

```
[ Your Business Code ] ---> ( INotificationService )
                                    â”‚
                                    â–¼
                         [ SpeedySmsAdapter ]  <-- The Adapter!
                                    â”‚
                                    â–¼
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
        return $"Sunny, 25Â°C in {city}";
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
   â””â”€â”€ [ CachingDecorator ]
          â””â”€â”€ [ Real ApiWeatherService ]
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
        return $"Sunny, 24Â°C in {city}";
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
// âŒ Controller is drowning in dependencies and orchestration logic!
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
[ Web Controller ]      â”€â”€> 40 lines (Inventory -> Tax -> Pay -> Ship -> Email)
[ Mobile API ]          â”€â”€> 40 lines (Copied & Pasted)
[ Batch CSV Importer ]  â”€â”€> 40 lines (Copied & Pasted)
[ Admin Phone Orders ]  â”€â”€> 40 lines (Copied & Pasted)

WITH FACADE:
[ Web Controller ]     â”€â”€â”€â”
[ Mobile API ]         â”€â”€â”€â”¼â”€â”€> [ IOrderProcessingFacade ] â”€â”€> 40 lines in ONE place!
[ Batch CSV Importer ] â”€â”€â”€â”¤           â”‚
[ Admin Phone Orders ] â”€â”€â”€â”˜           â””â”€â”€ Coordinates subsystems
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
[ Client Code ] â”€â”€> ( IDocumentService )
                           â”‚
                           â–¼
                 [ DocumentServiceProxy ]  <-- Checks permissions / audits
                           â”‚
                           â–¼ (If authorized)
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
                   â”œâ”€â”€ Monitor ($300)              <-- Leaf
                   â””â”€â”€ [ Gamer Pack (Bundle) ]     <-- Composite
                         â”œâ”€â”€ Keyboard ($50)        <-- Leaf
                         â””â”€â”€ Mouse ($25)           <-- Leaf
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

With pure inheritance, 2 types Ã— 3 channels = **6 classes** (`EmailSystemAlert`, `SmsSystemAlert`, etc.).  
4 types Ã— 5 channels = **20 classes**! Every new delivery channel requires writing a subclass for every notification type.

### 2. The Core Concept (Plain English)
> **"Decouple an abstraction from its implementation so that the two can vary independently."**

**Analogy: Universal Remote Control & Appliances**  
A remote control (Abstraction) communicates with TVs, Soundbars, and Projectors (Implementations) via a standard protocol (The Bridge). You don't manufacture a hardwired `SimpleSonyTvRemote`.

```
ABSTRACTION HIERARCHY                    IMPLEMENTATION HIERARCHY
(Business Concept)                        (Platform / Channel)

 Notification (Base)                     IMessageSender (Interface)
   â”œâ”€â”€ SystemAlert                       â”œâ”€â”€ EmailSender
   â””â”€â”€ UserReminder                      â”œâ”€â”€ SmsSender
          â”‚                              â””â”€â”€ SlackSender
          â””â”€â”€â”€ Has a reference to â”€â”€â”€â”€â”€â”€â”€â–º (The Bridge)
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
  You take a master job application form and photocopy it 50 times. Each applicant gets their **own paper**. Applicant A can write their name, strike out lines, or spill coffee on their copyâ€”it has zero impact on Applicant B.
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

Imagine you are building an **E-Commerce Checkout System**. 

The marketing team comes to you with different discount rules for customers:
1. **Regular Customers:** No discount.
2. **VIP Members:** 20% off everything.
3. **Black Friday Sale:** $25 off if the order is over $100.
4. **First-Time Buyer:** 10% off the first order.

#### The Bad Code (The Nightmare `switch` Statement):
```csharp
// ÂÅ’ Disaster: A single class trying to know every single math formula in the company
public class OrderService
{
    public decimal CalculateFinalPrice(Order order, DiscountType discountType)
    {
        decimal finalPrice = order.SubTotal;

        switch (discountType)
        {
            case DiscountType.None:
                break;

            case DiscountType.Vip:
                finalPrice -= order.SubTotal * 0.20m;
                break;

            case DiscountType.BlackFriday:
                if (order.SubTotal > 100)
                    finalPrice -= 25m;
                break;

            case DiscountType.FirstTimeBuyer:
                finalPrice -= order.SubTotal * 0.10m;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return finalPrice;
    }
}
```

#### Why does an Architect cringe looking at this?
1. **Violates Open/Closed Principle (OCP):** Next week, marketing announces a *"Summer Flash Sale"*. You have to open `OrderService.cs`, add a new enum value, and edit the switch statement. If you make a typo in the `switch`, you break checkout for everyone!
2. **Untestable:** You cannot test the Black Friday discount logic in isolation from the `OrderService`.
3. **Bloat:** Over time, `OrderService` grows into a 2,000-line "God Class" filled with random marketing rules.

---

### 2. The Core Concept (Plain English)

> **"Define a family of algorithms, put each of them into a separate class, and make their objects interchangeable."**

**The Analogy: Navigation on Google Maps Ã°Å¸â€”ÂºÃ¯Â¸Â**  
When you open Google Maps to travel to the airport:
* You can choose: **Driving**, **Walking**, **Bicycling**, or **Public Transit**.
* Each mode calculates the route differently (Walking takes sidewalks; Driving takes highways).
* Google Maps doesn't rewrite its whole app for each vehicle. It has a single interface: `IRouteStrategy.CalculateRoute()`. You simply pick the **Strategy** you want, and the map executes it!

---

### 3. Implementation in Modern C#

#### Step 1: The Strategy Interface (The Common Contract)
```csharp
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal totalAmount);
}
```

#### Step 2: The Concrete Strategies (One class per algorithm)
Each algorithm is encapsulated in its own clean, isolated file:

```csharp
// Strategy 1: No discount
public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal totalAmount) => totalAmount;
}

// Strategy 2: 20% off for VIPs
public class VipDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal totalAmount) => totalAmount * 0.80m;
}

// Strategy 3: Black Friday ($25 off orders over $100)
public class BlackFridayDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal totalAmount)
    {
        if (totalAmount > 100m)
            return totalAmount - 25m;

        return totalAmount;
    }
}
```

#### Step 3: The Context (Consuming the Strategy)
The checkout service doesn't know *how* the discount is calculated; it just delegates to the strategy:

```csharp
public class CheckoutService
{
    private IDiscountStrategy _discountStrategy;

    // We can inject a default strategy in the constructor
    public CheckoutService(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    // OR we can swap strategies dynamically at runtime!
    public void SetStrategy(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal Checkout(decimal cartTotal)
    {
        Console.WriteLine($"Original Total: ${cartTotal}");
        
        // DELEGATE TO STRATEGY:
        decimal finalAmount = _discountStrategy.ApplyDiscount(cartTotal);
        
        Console.WriteLine($"Final Amount after discount: ${finalAmount}\n");
        return finalAmount;
    }
}
```

---

### 4. Running the Code

```csharp
class Program
{
    static void Main()
    {
        decimal cartTotal = 150m;

        // 1. Regular checkout with No Discount
        var checkout = new CheckoutService(new NoDiscountStrategy());
        checkout.Checkout(cartTotal);

        // 2. Customer logs in as VIP -> SWAP the strategy at runtime!
        checkout.SetStrategy(new VipDiscountStrategy());
        checkout.Checkout(cartTotal);

        // 3. Today is Black Friday -> SWAP strategy!
        checkout.SetStrategy(new BlackFridayDiscountStrategy());
        checkout.Checkout(cartTotal);
    }
}
```

#### Output:
```text
Original Total: $150
Final Amount after discount: $150

Original Total: $150
Final Amount after discount: $120.00

Original Total: $150
Final Amount after discount: $125
```

---

### 5. The Architect's Deep Dive

#### A. Strategy vs. Factory (The Classic Confusion)
Developers often ask: *"Wait, both Strategy and Factory use interfaces. What's the difference?"*

* **Factory Pattern is Creational:** Its job is to **create and return an object** (`CreateProcessor()`).
* **Strategy Pattern is Behavioral:** Its job is to **execute an algorithm / business behavior** (`ApplyDiscount()`).

*(In real systems, they often work as best friends: A Factory decides **which** Strategy to pick, and the Strategy **runs** the logic!)*

#### B. Modern C# Functional Strategy (The Delegate Shortcut)
In C#, if a strategy is tiny and only has a single method, you don't even need to create a whole class! You can use a **delegate / lambda** as a lightweight strategy:

```csharp
public class FastCheckoutService
{
    // A Func<decimal, decimal> IS a strategy!
    public decimal Checkout(decimal total, Func<decimal, decimal> discountStrategy)
    {
        return discountStrategy(total);
    }
}

// Consumed with inline lambdas:
var service = new FastCheckoutService();

// VIP strategy:
service.Checkout(100m, total => total * 0.8m);

// Flash Sale strategy:
service.Checkout(100m, total => total - 15m);
```
*(LINQ's `OrderBy(x => x.Age)` is literally the Strategy pattern using a lambda delegate!)*

---

### Summary Checklist

| Without Strategy | With Strategy |
| :--- | :--- |
| Giant `if/else` or `switch` statements | Each algorithm isolated in its own class |
| Adding a new rule requires editing existing classes (violates OCP) | Adding a new rule = creating a new file (strictly respects OCP) |
| Hard to unit-test specific formulas | Each strategy can be unit-tested with 100% code coverage |
---

## Pattern 2: Observer (Behavioral)

### 1. What is the Problem? (The Real-World Pain)

Imagine you are building a **Crypto / Stock Market Ticker**. 

The price of **Bitcoin** or **Apple stock** changes every few seconds. Multiple parts of your system need to know the moment the price updates:
1. **Mobile Push Notification Service** (alerts users on their phones).
2. **Automated Trading Bot** (executes buy/sell orders when prices hit a threshold).
3. **Analytics Dashboard** (updates real-time line charts on the web).

#### Approach A: The Inefficient Way (Polling)
The Mobile App and Trading Bot run a `while (true)` loop and query the database every 500ms: *"Did the price change yet? Did the price change yet?"*
* **Why it fails:** 99% of requests are wasted bandwidth, CPU spikes to 100%, and the database collapses under load.

#### Approach B: The Tightly Coupled Way
The `Stock` class hardcodes direct references to everyone:

```csharp
// ÂÅ’ Architectural Disaster: Tightly coupled to every subscriber!
public class Stock
{
    private MobileApp _mobileApp;
    private TradingBot _bot;
    private AnalyticsDashboard _dashboard;

    public void UpdatePrice(decimal newPrice)
    {
        // If we add a 4th service tomorrow, we have to edit this class!
        _mobileApp.SendPush(newPrice);
        _bot.CheckRules(newPrice);
        _dashboard.Redraw(newPrice);
    }
}
```

---

### 2. The Core Concept (Plain English)

> **"Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically."**

**The Analogy: YouTube Channel & The Notification Bell ðŸ””**
* You don't drive to YouTube HQ every morning to check if your favorite creator uploaded a new video.
* You hit the **"Subscribe"** button and ring the notification bell.
* The YouTuber (**The Subject / Publisher**) uploads a video once.
* YouTube automatically pushes a notification to all 100,000 subscribers (**The Observers**) simultaneously.
* If you get tired of the channel, you hit **"Unsubscribe"** and you stop receiving alerts.

```
                  â”Œâ”€â”€â”€> [ MobileAppSubscriber ]    (Observer 1)
[ StockTicker ] â”€â”€â”¼â”€â”€â”€> [ TradingBotSubscriber ]   (Observer 2)
  (Publisher)     â””â”€â”€â”€> [ DashboardSubscriber ]    (Observer 3)
```

---

### 3. Implementation in Modern C# (Classic GoF Pattern)

#### Step 1: The Observer Interface (The Subscriber Contract)
```csharp
public interface IStockObserver
{
    void OnPriceChanged(string stockSymbol, decimal newPrice);
}
```

#### Step 2: The Subject / Publisher (The Stock Ticker)
The ticker maintains a list of subscribers and notifies them when its state changes:

```csharp
public class StockTicker
{
    private readonly string _symbol;
    private decimal _price;
    
    // The registry of subscribers
    private readonly List<IStockObserver> _observers = new();

    public StockTicker(string symbol, decimal initialPrice)
    {
        _symbol = symbol;
        _price = initialPrice;
    }

    // Subscribe
    public void Attach(IStockObserver observer)
    {
        _observers.Add(observer);
        Console.WriteLine($"[Ticker] Added a new subscriber to {_symbol}.");
    }

    // Unsubscribe
    public void Detach(IStockObserver observer)
    {
        _observers.Remove(observer);
        Console.WriteLine($"[Ticker] Removed a subscriber from {_symbol}.");
    }

    // State change triggers notification broadcast
    public void SetPrice(decimal newPrice)
    {
        if (_price != newPrice)
        {
            _price = newPrice;
            Console.WriteLine($"\n--- [MARKET UPDATE] {_symbol} moved to ${_price} ---");
            Notify();
        }
    }

    private void Notify()
    {
        // Broadcast to all registered observers
        foreach (var observer in _observers)
        {
            observer.OnPriceChanged(_symbol, _price);
        }
    }
}
```

#### Step 3: Concrete Observers (The Subscribers)
Each subscriber reacts to the update in its own unique way:

```csharp
// Subscriber 1: Mobile App
public class MobileAppAlert : IStockObserver
{
    public void OnPriceChanged(string stockSymbol, decimal newPrice)
    {
        Console.WriteLine($"ðŸ“± [Mobile App] Push Alert: {stockSymbol} is now ${newPrice}!");
    }
}

// Subscriber 2: Automated Trading Bot
public class AutoTradingBot : IStockObserver
{
    private readonly decimal _buyThreshold;

    public AutoTradingBot(decimal buyThreshold)
    {
        _buyThreshold = buyThreshold;
    }

    public void OnPriceChanged(string stockSymbol, decimal newPrice)
    {
        if (newPrice < _buyThreshold)
        {
            Console.WriteLine($"ðŸ¤– [Trading Bot] BUY ORDER EXECUTED! Price ${newPrice} is below target ${_buyThreshold}.");
        }
    }
}
```

---

### 4. Running the Code

```csharp
class Program
{
    static void Main()
    {
        var btcTicker = new StockTicker("BTC", 65000m);

        var mobileApp = new MobileAppAlert();
        var tradingBot = new AutoTradingBot(buyThreshold: 60000m);

        // 1. Subscribe both services
        btcTicker.Attach(mobileApp);
        btcTicker.Attach(tradingBot);

        // 2. Price changes -> Both are notified
        btcTicker.SetPrice(62000m);

        // 3. Price drops below bot threshold -> Bot executes buy order!
        btcTicker.SetPrice(59000m);

        // 4. Mobile app unsubscribes
        btcTicker.Detach(mobileApp);

        // 5. Price changes again -> ONLY the bot is notified!
        btcTicker.SetPrice(58000m);
    }
}
```

#### Output:
```text
[Ticker] Added a new subscriber to BTC.
[Ticker] Added a new subscriber to BTC.

--- [MARKET UPDATE] BTC moved to $62000 ---
ðŸ“± [Mobile App] Push Alert: BTC is now $62000!

--- [MARKET UPDATE] BTC moved to $59000 ---
ðŸ“± [Mobile App] Push Alert: BTC is now $59000!
ðŸ¤– [Trading Bot] BUY ORDER EXECUTED! Price $59000 is below target $60000.

[Ticker] Removed a subscriber from BTC.

--- [MARKET UPDATE] BTC moved to $58000 ---
ðŸ¤– [Trading Bot] BUY ORDER EXECUTED! Price $58000 is below target $60000.
```

---

### 5. The Architect's Deep Dive: How .NET Evolved the Observer Pattern

C# and .NET love the Observer pattern so much that it is baked directly into the language in **three different ways**:

#### A. Native C# `event` and `delegate` (Language-Level Observer)
You don't need `Attach()` and `Detach()` interfaces in everyday C#! The language has `+=` and `-=`:

```csharp
public class StockTicker
{
    // C# event IS the Observer pattern!
    public event Action<decimal>? PriceChanged;

    public void SetPrice(decimal price)
    {
        PriceChanged?.Invoke(price); // Notifies all subscribers!
    }
}

// Subscribing is just one line:
ticker.PriceChanged += price => Console.WriteLine($"Price: {price}");
```

#### B. The #1 Memory Leak in C#: "The Lapsed Listener Problem"
âš ï¸Â **Interview Warning:** This is a famous senior .NET interview question!

If `StockTicker` is a **Singleton** (lives for the entire lifetime of your application), and a short-lived UI dialog or transient service subscribes using:
```csharp
ticker.PriceChanged += myTransientService.OnPriceChanged;
```
Under the hood, `ticker` holds a **strong memory reference** to `myTransientService`. 

**Result:** Even if `myTransientService` is done and closed, the Garbage Collector **CANNOT collect it**! It leaks memory forever until the whole app restarts.
* **The Architect's Rule:** Always unsubscribe (`ticker.PriceChanged -= ...`) inside `Dispose()`, or use weak event managers / message brokers.

#### C. Modern Enterprise .NET: MediatR & Reactive Extensions (Rx)
* In ASP.NET Core Clean Architecture, we use **Domain Events** with `INotification` and `INotificationHandler<T>` (via MediatR) to broadcast domain updates across bounded contexts.
* For complex event streams (throttle, debounce, filter), .NET provides **`IObservable<T>` and `IObserver<T>`** in `System.Reactive`.

> ðŸ’¡ **Architectural Note:** For a detailed comparison of MediatR's `INotification` vs. Commands and CQRS, see [Deep Dive: MediatR Multi-Pattern Architecture](#deep-dive-mediatr-multi-pattern-architecture-mediator-vs-observer-vs-cqrs-vs-command-vs-chain-of-responsibility).

---

## Pattern 3: Command (Behavioral)

### 1. What is the Problem? (The Real-World Pain)

Normally, when you want to execute an operation in C#, you call a method directly:
```csharp
bankAccount.Withdraw(100);
```

#### What happens when business requirements get complex?
1. **"We need an Undo button:"** If the user clicked the wrong button or a transaction failed midway, how do you reverse it?
2. **"We need to queue requests:"** What if the database or external payment gateway is temporarily down? How do you save the operation in a queue to retry later?
3. **"We need an Audit Trail / Log:"** How do you record a persistent history of every single action that occurred so you can replay it?

If you just invoke methods directly (`account.Withdraw(100)`), the action executes and vanishes. You cannot store a method call in a list, you cannot serialize it to a database, and you cannot reverse it.

---

### 2. The Core Concept (Plain English)

> **"Encapsulate a request as a standalone object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations."**

**The Analogy: A Restaurant Order Ticket ðŸ§¾**
When you order food at a restaurant:
* You do **not** walk into the kitchen and tell the chef: *"Grill a steak, medium-rare."*
* The waiter writes your request down on a paper **Order Ticket (The Command)**:
  - Table #4: Ribeye Steak, Medium-Rare, Garlic Butter.
* That physical ticket can now:
  - Sit in a **queue** with other tickets.
  - Be passed to any available chef.
  - Be logged for billing.
  - Be **cancelled / torn up** if you change your mind before the steak hits the grill!

```
[ Sender / UI / Invoker ] â”€â”€> [ Creates Command Object ] â”€â”€> [ Receiver / Business Model ]
                                (Has Execute() & Undo())
```

---

### 3. Implementation in Modern C# (With Full "Undo" Support)

Let's build a **Banking Transaction Engine** where every deposit and withdrawal can be queued, logged, and **undone (rolled back)**.

#### Step 1: The Command Interface
Every command must know how to execute itself, and how to reverse (undo) itself:

```csharp
public interface ITransactionCommand
{
    bool Execute();
    void Undo();
}
```

#### Step 2: The Receiver (The core business entity)
The `BankAccount` doesn't know about buttons, queues, or history. It just knows how to deposit and withdraw:

```csharp
public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"[Account {AccountNumber}] Deposited ${amount}. New Balance: ${Balance}");
    }

    public bool Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"[Account {AccountNumber}] Withdrew ${amount}. New Balance: ${Balance}");
            return true;
        }

        Console.WriteLine($"[Account {AccountNumber}] ÂÅ’ Insufficient funds to withdraw ${amount}!");
        return false;
    }
}
```

#### Step 3: Concrete Commands (Deposit & Withdraw)
Each command wraps the `BankAccount` and captures the exact parameters of the action:

```csharp
// Concrete Command 1: Deposit
public class DepositCommand : ITransactionCommand
{
    private readonly BankAccount _account;
    private readonly decimal _amount;
    private bool _isExecuted = false;

    public DepositCommand(BankAccount account, decimal amount)
    {
        _account = account;
        _amount = amount;
    }

    public bool Execute()
    {
        _account.Deposit(_amount);
        _isExecuted = true;
        return true;
    }

    public void Undo()
    {
        if (_isExecuted)
        {
            Console.WriteLine($"--> [UNDO] Reversing Deposit of ${_amount}...");
            _account.Withdraw(_amount); // Reversal of deposit is withdrawal!
            _isExecuted = false;
        }
    }
}

// Concrete Command 2: Withdraw
public class WithdrawCommand : ITransactionCommand
{
    private readonly BankAccount _account;
    private readonly decimal _amount;
    private bool _isExecuted = false;

    public WithdrawCommand(BankAccount account, decimal amount)
    {
        _account = account;
        _amount = amount;
    }

    public bool Execute()
    {
        _isExecuted = _account.Withdraw(_amount);
        return _isExecuted;
    }

    public void Undo()
    {
        if (_isExecuted)
        {
            Console.WriteLine($"--> [UNDO] Reversing Withdrawal of ${_amount}...");
            _account.Deposit(_amount); // Reversal of withdrawal is deposit!
            _isExecuted = false;
        }
    }
}
```

#### Step 4: The Invoker / Transaction Manager (Manages History & Undo)
The invoker maintains an **Undo Stack** of executed commands:

```csharp
public class TransactionManager
{
    // A LIFO (Last-In, First-Out) stack for Undo operations
    private readonly Stack<ITransactionCommand> _history = new();

    public void ExecuteTransaction(ITransactionCommand command)
    {
        if (command.Execute())
        {
            _history.Push(command); // Remember command in history
        }
    }

    public void UndoLastTransaction()
    {
        if (_history.Count > 0)
        {
            var command = _history.Pop(); // Get the last command
            command.Undo();
        }
        else
        {
            Console.WriteLine("No transactions left to undo!");
        }
    }
}
```

---

### 4. Running the Code

```csharp
class Program
{
    static void Main()
    {
        var account = new BankAccount("US-9912", initialBalance: 100m);
        var manager = new TransactionManager();

        Console.WriteLine("=== EXECUTING TRANSACTIONS ===");
        
        // 1. Deposit $50
        manager.ExecuteTransaction(new DepositCommand(account, 50m));

        // 2. Withdraw $30
        manager.ExecuteTransaction(new WithdrawCommand(account, 30m));

        // 3. Withdraw $200 (Will fail due to insufficient funds)
        manager.ExecuteTransaction(new WithdrawCommand(account, 200m));

        Console.WriteLine("\n=== PERFORMING UNDO (Ctrl+Z) ===");
        
        // Undo last successful transaction ($30 withdrawal)
        manager.UndoLastTransaction();

        // Undo deposit ($50 deposit)
        manager.UndoLastTransaction();
        
        // Try undoing again when empty
        manager.UndoLastTransaction();
    }
}
```

#### Output:
```text
=== EXECUTING TRANSACTIONS ===
[Account US-9912] Deposited $50. New Balance: $150
[Account US-9912] Withdrew $30. New Balance: $120
[Account US-9912] ÂÅ’ Insufficient funds to withdraw $200!

=== PERFORMING UNDO (Ctrl+Z) ===
--> [UNDO] Reversing Withdrawal of $30...
[Account US-9912] Deposited $30. New Balance: $150
--> [UNDO] Reversing Deposit of $50...
[Account US-9912] Withdrew $50. New Balance: $100
No transactions left to undo!
```

---

### 5. The Architect's View: CQRS & Modern .NET

In modern enterprise C# systems, you see the Command Pattern everywhere:

#### A. CQRS (Command Query Responsibility Segregation)
In enterprise architectures, we split our system into two halves:
* **Queries:** Fetch data without modifying state (`GetOrderByIdQuery`).
* **Commands:** Standalone objects that mutate state (`CreateOrderCommand`, `CancelSubscriptionCommand`).

#### B. Background Job Queues (Hangfire, RabbitMQ, MassTransit)
Because a Command is a self-contained object holding its data, you can serialize it into **JSON**:
```json
{
  "CommandType": "SendMonthlyNewsletterCommand",
  "ScheduledTime": "2026-09-15T00:00:00Z",
  "TargetUserIds": [101, 102, 103]
}
```
You drop it into a RabbitMQ message broker, and background workers deserialize and call `.Execute()` hours later!

#### C. WPF / MAUI UI Development (`ICommand`)
In XAML UI development, buttons don't have click events hardcoded in code-behind; they bind to an `ICommand` property on a ViewModel (`SaveCommand`, `DeleteCommand`).
---

## Pattern 4: Mediator (Behavioral)

### 1. What is the Problem? (The "Spiderweb" Coupling Chaos)

Imagine you are building an airport landing system, or an enterprise application where different services need to communicate:
* The **Flight Departure Service**
* The **Runway Allocation Service**
* The **Ground Baggage Service**
* The **Fuel Service**

#### Without a Mediator (Many-to-Many Spaghetti):
If every component talks directly to every other component:
* Plane A must radio Plane B, Plane C, and Plane D to ask where they are.
* Plane A must call Ground Crew directly.
* Plane A must call the Weather Service directly.

```
[ Plane A ] <=======> [ Plane B ]
     â–²    \         /    â–²
     â”‚     \       /     â”‚      âŒ THE SPIDERWEB OF CHAOS:
     â”‚      \     /      â”‚      Every class has 5 to 10 dependencies
     â–¼       \   /       â–¼      injected into its constructor!
[ Plane C ] <=======> [ Plane D ]
```

**The Architectural Disaster:**  
If you have 10 services, they need up to **45 direct relationships**!  
$$\text{Connections} = \frac{N(N - 1)}{2} = \frac{10 \times 9}{2} = 45$$  
Changing how Plane A communicates breaks Plane B, C, and D. You cannot test any single service in isolation.

---

### 2. The Core Concept (Plain English)

> **"Define an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly."**

**The Analogy: The Air Traffic Control (ATC) Tower ðŸ—¼**  
Planes in the sky **never** radio each other directly. 
* All planes communicate **only** with the **Control Tower (The Mediator)**.
* The pilot calls the tower: *"Tower, Flight 101 requesting permission to land on Runway 2."*
* The tower checks the runway, coordinates with other planes, and replies: *"Flight 101, hold pattern for 5 minutes."*

```
[ Plane A ] â”€â”€â”€â”Â               â”Œâ”€â”€â”€> [ Plane B ]
               â–¼               â”‚
       [ CONTROL TOWER ] â”€â”€â”€â”€â”€â”€â”¤
          (Mediator)           â”‚
[ Plane C ] â”€â”€â”€â”˜               â””â”€â”€â”€> [ Ground Crew ]
```
Now, instead of 45 chaotic connections, you have **one central hub**.

---

### 3. Implementation in Modern C# (The Classic GoF Pattern)

#### Step 1: The Mediator Interface
The mediator defines how colleagues communicate:

```csharp
public interface IAirTrafficControl
{
    void RegisterFlight(Airplane plane);
    void SendMessage(string message, Airplane sender);
}
```

#### Step 2: The Colleague Base Class
Every airplane holds a reference to the **Mediator**, never to other airplanes:

```csharp
public abstract class Airplane
{
    protected readonly IAirTrafficControl _atc;
    public string CallSign { get; }

    protected Airplane(IAirTrafficControl atc, string callSign)
    {
        _atc = atc;
        CallSign = callSign;
    }

    public abstract void Receive(string message);

    // Communicate ONLY through the mediator
    public void Send(string message)
    {
        Console.WriteLine($"\n[Radio] {CallSign} broadcasts to Tower: \"{message}\"");
        _atc.SendMessage(message, this);
    }
}
```

#### Step 3: Concrete Colleagues (Commercial & Cargo Flights)
```csharp
public class PassengerFlight : Airplane
{
    public PassengerFlight(IAirTrafficControl atc, string callSign) : base(atc, callSign) { }

    public override void Receive(string message)
    {
        Console.WriteLine($"âœˆï¸Â  [Passenger {CallSign}] Heard from Tower: '{message}'");
    }
}

public class CargoFlight : Airplane
{
    public CargoFlight(IAirTrafficControl atc, string callSign) : base(atc, callSign) { }

    public override void Receive(string message)
    {
        Console.WriteLine($"ðŸ“¦ [Cargo {CallSign}] Heard from Tower: '{message}'");
    }
}
```

#### Step 4: The Concrete Mediator (The Control Tower)
The tower coordinates and broadcasts the messages to all other planes:

```csharp
public class AirTrafficControlTower : IAirTrafficControl
{
    private readonly List<Airplane> _planes = new();

    public void RegisterFlight(Airplane plane)
    {
        _planes.Add(plane);
        Console.WriteLine($"[Tower] Flight {plane.CallSign} entered our airspace.");
    }

    public void SendMessage(string message, Airplane sender)
    {
        // Broadcast to all planes EXCEPT the sender
        foreach (var plane in _planes)
        {
            if (plane != sender)
            {
                plane.Receive(message);
            }
        }
    }
}
```

---

### 4. Running the Code

```csharp
class Program
{
    static void Main()
    {
        // 1. Create the Mediator
        IAirTrafficControl tower = new AirTrafficControlTower();

        // 2. Create airplanes (they only know about the tower!)
        var boeing737 = new PassengerFlight(tower, "Delta-101");
        var airbusA320 = new PassengerFlight(tower, "United-452");
        var fedexCargo = new CargoFlight(tower, "FedEx-Heavy");

        // 3. Register with Tower
        tower.RegisterFlight(boeing737);
        tower.RegisterFlight(airbusA320);
        tower.RegisterFlight(fedexCargo);

        // 4. Delta-101 sends a message
        boeing737.Send("Descended to 10,000 feet, entering final approach.");

        // 5. FedEx sends a message
        fedexCargo.Send("Holding at runway 26R, waiting for clearance.");
    }
}
```

#### Output:
```text
[Tower] Flight Delta-101 entered our airspace.
[Tower] Flight United-452 entered our airspace.
[Tower] Flight FedEx-Heavy entered our airspace.

[Radio] Delta-101 broadcasts to Tower: "Descended to 10,000 feet, entering final approach."
âœˆï¸Â  [Passenger United-452] Heard from Tower: 'Descended to 10,000 feet, entering final approach.'
ðŸ“¦ [Cargo FedEx-Heavy] Heard from Tower: 'Descended to 10,000 feet, entering final approach.'

[Radio] FedEx-Heavy broadcasts to Tower: "Holding at runway 26R, waiting for clearance."
âœˆï¸Â  [Passenger Delta-101] Heard from Tower: 'Holding at runway 26R, waiting for clearance.'
âœˆï¸Â  [Passenger United-452] Heard from Tower: 'Holding at runway 26R, waiting for clearance.'
```

Notice: `Delta-101` and `United-452` **never had to know each other existed**. If you add 50 more planes, not a single airplane class changes!

---

### 5. The Architect's View: Why Mediator Dominates Modern ASP.NET Core (MediatR)

In modern C# Clean Architecture and CQRS, we use an in-process Mediator called **MediatR**:

#### The Problem It Solves in Controllers:
Without MediatR, a `UsersController` ends up injecting 6 services:
```csharp
// ÂÅ’ Controller Constructor Bloat:
public UsersController(
    IUserRepository userRepo, 
    IEmailService email, 
    ISmsService sms, 
    ILogger logger, 
    ITokenGenerator token, 
    IAuditService audit) { ... }
```

#### With the Mediator Pattern (Ultra-Thin Controller):
The controller has **ONE** dependency: `IMediator`.

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        // Controller doesn't know WHO handles it, WHERE it lives, or HOW it works!
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }
}
```

#### The Handler (Completely Decoupled in the Core Layer):
```csharp
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _repo;

    public RegisterUserHandler(IUserRepository repo) => _repo = repo;

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        // Business logic runs here in complete isolation!
        return await _repo.CreateUserAsync(request.Email, request.Password);
    }
}
```

---

### 6. Summary Comparison: Mediator vs. Facade vs. Observer

Developers frequently confuse these three in technical interviews. Here is your cheat-sheet:

| Pattern | Relationship | Primary Intent |
| :--- | :--- | :--- |
| **Facade** | **One-way** (Top-down) | Provides a **simplified front-door** to a complex subsystem. |
| **Observer** | **One-to-Many** (One-way) | A Publisher **broadcasts events** to subscribers who listen passively. |
| **Mediator** | **Many-to-Many** (Bi-directional) | Coordinates **two-way collaboration** between independent peers so they don't touch each other. |

---

### Deep Dive: Air Traffic Control rewritten with Modern MediatR

> **Question:** Could you write the air traffic controller program in modern MediatR pattern?

In MediatR, you don't write a custom tower class. MediatR **IS** the tower:
1. **Requests / Commands (1-to-1):** A plane asks the tower for something (e.g., *"Request Landing"*), and **one** handler responds.
2. **Notifications (1-to-Many):** An event happens (e.g., *"Runway Closed"* or *"Emergency Landing"*), and MediatR broadcasts it to **multiple** handlers simultaneously.
3. **Pipeline Behaviors:** Middleware intercepting every message (cross-cutting logging, timing, validation).

```csharp
using MediatR;
using Microsoft.Extensions.DependencyInjection;

// 1. 1-to-1 Command
public record LandingClearance(bool IsApproved, string Runway, string Reason);
public record RequestLandingCommand(string FlightNumber, string AircraftType) : IRequest<LandingClearance>;

public class RequestLandingHandler : IRequestHandler<RequestLandingCommand, LandingClearance>
{
    public Task<LandingClearance> Handle(RequestLandingCommand req, CancellationToken ct)
    {
        if (req.AircraftType == "HeavyBoeing777")
        {
            return Task.FromResult(new LandingClearance(true, "Runway-26L (Long)", "Cleared for landing on long runway. Wind 5 knots."));
        }

        return Task.FromResult(new LandingClearance(true, "Runway-08R", "Cleared to land."));
    }
}

// 2. 1-to-Many Notification (Broadcast)
public record MaydayAlertNotification(string FlightNumber, string EmergencyType) : INotification;

public class EmergencyServicesHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification n, CancellationToken ct)
    {
        Console.WriteLine($"ðŸš¨ [FIRE & RESCUE] Dispatched to runway for {n.FlightNumber}! Emergency: {n.EmergencyType}");
        return Task.CompletedTask;
    }
}

public class GroundOperationsHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification n, CancellationToken ct)
    {
        Console.WriteLine($"ðŸ›‘ [GROUND OPS] Halting taxiing. Clearing perimeter for {n.FlightNumber}.");
        return Task.CompletedTask;
    }
}

public class RadarAuditLoggerHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification n, CancellationToken ct)
    {
        Console.WriteLine($"Ã°Å¸â€œÂ [FLIGHT RECORDER] Incident logged in permanent database: {n.FlightNumber} at {DateTime.UtcNow}.");
        return Task.CompletedTask;
    }
}

// 3. Pipeline Behavior (Blackbox Flight Recorder)
public class FlightTelemetryBehavior<TReq, TResp> : IPipelineBehavior<TReq, TResp> where TReq : notnull
{
    public async Task<TResp> Handle(TReq req, RequestHandlerDelegate<TResp> next, CancellationToken ct)
    {
        Console.WriteLine($"ðŸ“¡ [RADAR TELEMETRY] Intercepted: {typeof(TReq).Name}");
        var res = await next();
        Console.WriteLine($"ðŸ“¡ [RADAR TELEMETRY] Successfully processed response.");
        return res;
    }
}
```

#### Wiring It All Together (`Program.cs`):
```csharp
class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FlightTelemetryBehavior<,>));
        });

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        // SCENARIO 1: Delta-101 requests landing (1-to-1 Command)
        var deltaLandingCommand = new RequestLandingCommand("Delta-101", "HeavyBoeing777");
        LandingClearance result = await mediator.Send(deltaLandingCommand);
        Console.WriteLine($"âœˆï¸Â  [Delta-101 Cockpit] Clearance received: Approved={result.IsApproved}, Assigned={result.Runway}. ({result.Reason})");

        // SCENARIO 2: Flight United-99 declares Emergency (1-to-Many Broadcast)
        Console.WriteLine("\n=======================================================");
        Console.WriteLine("âš ï¸Â United-99 Declares In-Flight Emergency!");
        Console.WriteLine("=======================================================");

        var emergency = new MaydayAlertNotification("United-99", "Hydraulic Failure");
        await mediator.Publish(emergency);
    }
}
```

#### Output:
```text
ðŸ“¡ [RADAR TELEMETRY] Intercepted: RequestLandingCommand
âœˆï¸Â  [Delta-101 Cockpit] Clearance received: Approved=True, Assigned=Runway-26L (Long). (Cleared for landing on long runway. Wind 5 knots.)

=======================================================
âš ï¸Â United-99 Declares In-Flight Emergency!
=======================================================
ðŸš¨ [FIRE & RESCUE] Dispatched to runway for United-99! Emergency: Hydraulic Failure
ðŸ›‘ [GROUND OPS] Halting taxiing. Clearing perimeter for United-99.
Ã°Å¸â€œÂ [FLIGHT RECORDER] Incident logged in permanent database: United-99 at 09/29/2026 10:00:00.
```
---

### Deep Dive: MediatR Multi-Pattern Architecture (Mediator vs. Observer vs. CQRS vs. Command vs. Chain of Responsibility)

While MediatR is titled after the **Mediator Pattern**, under the hood it is an architectural foundation in modern .NET that implements **at least 5 distinct design patterns**.

---

#### 1. What Other Design Patterns Use MediatR?

Beyond **Mediator**, **Observer (Pub/Sub)**, and **CQRS**, MediatR provides the canonical modern .NET implementation for:

1. **The Command Pattern (GoF Behavioral):**
   * Encapsulates an action, intent, and parameters into a single serializable object.
   * Every `IRequest` or `IRequest<TResponse>` in MediatR is a **Command object**.
2. **The Chain of Responsibility Pattern (GoF Behavioral):**
   * Implemented via MediatR **Pipeline Behaviors** (`IPipelineBehavior<TRequest, TResponse>`).
   * Requests traverse an ordered Russian-doll chain of handlers (Validation $\to$ Performance/Logging $\to$ Transactions $\to$ Business Handler), where each middleware link can inspect, mutate, or short-circuit before calling `await next()`.
3. **The Decorator Pattern (GoF Structural):**
   * Pipeline behaviors dynamically decorate and add behavior to your handlers at runtime without altering existing handler code (adhering strictly to OCP).

---

#### 2. Difference in Use Case & Implementation

| Pattern | MediatR Primitive | Cardinality | Expects Return? | Core Architectural Intent | When to Use |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Mediator** | `IMediator.Send(request)` | **1-to-1** | Yes or `Unit` | **Decoupling**: Eliminates many-to-many "spiderweb" dependencies between classes. Caller doesn't know or care who handles it. | Clean Architecture controllers/endpoints to prevent injecting 5â€“10 service dependencies. |
| **Observer / Pub-Sub** | `IMediator.Publish(notification)` | **1-to-Many** (0 to N) | **No** (`Task` only, fire-and-forget in-process) | **Side-Effects**: Broadcasting that a domain event has already occurred (`OrderPlacedEvent`). Multiple independent listeners react. | Triggering audit logs, sending confirmation emails, notifying external websockets after a state change. |
| **Command Pattern** | `IRequest<T>` | **1-to-1** | Yes (`Result<T>`, IDs) | **Encapsulation**: Packaging a business mutation and its parameters into a stand-alone object that can be logged, queued, or retried. | Executing state-altering operations (`CancelSubscriptionCommand`, `TransferMoneyCommand`). |
| **CQRS** | `IRequest<T>` split by naming & handler logic | **1-to-1** | Commands: `Result`<br/>Queries: `ReadModelDto` | **Architectural Segregation**: Completely separating Write/Mutation models from Read/Query models. | Enterprise systems where read throughput and query shapes differ drastically from write/validation logic. |
| **Chain of Responsibility** | `IPipelineBehavior<TRequest, TResponse>` | **1-to-Chain-to-1** | Yes (passes through) | **Cross-Cutting Concerns**: Applying logging, fluent validation, authorization, and transactions in a pipeline before hitting the handler. | Global pre/post processing rules without cluttering business handlers with boilerplate. |

---

#### 3. How to Know Which Pattern is Implemented Just by Reading Code (Code Detective Guide)

When reviewing a C# codebase, look for these specific **code fingerprints**:

##### Fingerprint A: The Observer Pattern (Domain Events / Pub-Sub)
* **Look for:** `INotification` marker interface and **multiple handlers for the same event**.
* **Invocation:** Uses `_mediator.Publish(...)`, **never** `_mediator.Send(...)`.
* **Handlers:** Implement `INotificationHandler<T>` and return `Task` (no return types).

```csharp
// 1. The Event (Implements INotification, NOT IRequest)
public record OrderPlacedEvent(Guid OrderId, decimal Amount) : INotification;

// 2. Observer Handler #1: Sends an Email
public class SendEmailOnOrderPlaced : INotificationHandler<OrderPlacedEvent>
{
    public async Task Handle(OrderPlacedEvent notification, CancellationToken ct)
    {
        // Reacts independently without caller waiting for a return value
    }
}

// 3. Observer Handler #2: Reserves Inventory
public class ReserveStockOnOrderPlaced : INotificationHandler<OrderPlacedEvent>
{
    public async Task Handle(OrderPlacedEvent notification, CancellationToken ct)
    {
        // Reacts to the same event
    }
}

// Invocation uses Publish(), NEVER Send():
await _mediator.Publish(new OrderPlacedEvent(order.Id, order.Total));
```

##### Fingerprint B: CQRS (Command Query Responsibility Segregation)
* **Look for:** Symmetrical folder separation (`Features/Orders/Commands/` vs. `Features/Orders/Queries/`) and distinct naming conventions (`...Command` vs `...Query`).
* **Return Types:**
  * Commands return mutation outcomes (`Result`, `Result<Guid>`, `Unit`). They use Domain Entities & EF Core write contexts.
  * Queries return read-optimized models (`OrderSummaryDto`, `PagedList<T>`). They often bypass EF Core completely (using raw SQL or Dapper with `IDbConnection`) or use `.AsNoTracking()`.

```csharp
// --- WRITE SIDE (COMMAND) ---
// Intent: Mutate state. Returns a Result or ID. Uses Domain Entities & DbContext.
public record CreateUserCommand(string Email, string Password) : IRequest<Result<Guid>>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly AppDbContext _db; // Has DbContext write access
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // Enforces domain invariants, updates database
        return Result<Guid>.Success(Guid.NewGuid());
    }
}

// --- READ SIDE (QUERY) ---
// Intent: Read-only. Never mutates. Returns a DTO directly (often uses Dapper or AsNoTracking).
public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IDbConnection _dapper; // Optimized read bypass!
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        // High-speed direct SQL projection
        return await _dapper.QueryFirstOrDefaultAsync<UserDto>(
            "SELECT Id, Email, CreatedOn FROM Users WHERE Id = @Id", new { Id = request.UserId });
    }
}
```

##### Fingerprint C: Pure Mediator Pattern
* **Look for:** Controllers or services whose constructors inject **only `IMediator`**, completely removing dependencies on 5â€“8 concrete business services.
* **Invocation:** Uses `_mediator.Send(...)` for direct 1-to-1 dispatching where the caller only knows the contract, not the handler.

```csharp
[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly IMediator _mediator; // Zero coupling to IInventoryService, IPaymentService, etc.

    public CheckoutController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutRequest request)
    {
        // 1-to-1 mediated dispatch:
        var result = await _mediator.Send(new ProcessCheckout(request.CartId));
        return Ok(result);
    }
}
```

##### Fingerprint D: Chain of Responsibility (Pipeline Behaviors)
* **Look for:** Classes implementing `IPipelineBehavior<TRequest, TResponse>` that accept a `RequestHandlerDelegate<TResponse> next` parameter.
* **Invocation:** Registered via `services.AddMediatR(cfg => cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)))`.
* **Execution:** Wraps execution in a *Russian-Doll* hierarchy, calling `await next()` to delegate down the chain or throwing/returning early to short-circuit.

```csharp
public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, // <-- The next link in the chain!
        CancellationToken cancellationToken)
    {
        // 1. Inspect before passing down the chain:
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures); // Short-circuit chain!

        // 2. Pass request to next handler in line:
        return await next();
    }
}
```

---

#### 4. Quick Diagnostic Decision Tree

```
When inspecting MediatR code in a solution:
â”‚
â”œâ”€â”€ 1. Does it implement `INotification`?
â”‚    â””â”€â”€ YES â”€â”€â–º OBSERVER / PUB-SUB PATTERN
â”‚                (1-to-Many broadcast, event-driven side-effects, no return value)
â”‚
â”œâ”€â”€ 2. Does it implement `IPipelineBehavior` with `await next()`?
â”‚    â””â”€â”€ YES â”€â”€â–º CHAIN OF RESPONSIBILITY / DECORATOR PATTERN
â”‚                (Middleware pipeline intercepting requests for validation, logging, or transactions)
â”‚
â””â”€â”€ 3. Does it implement `IRequest<T>`?
     â”‚
     â”œâ”€â”€ Are commands and queries cleanly separated into `...Command` (state mutation + Result)
     â”‚   and `...Query` (read projection + DTO, bypassing write models)?
     â”‚    â””â”€â”€ YES â”€â”€â–º CQRS + COMMAND PATTERN
     â”‚
     â””â”€â”€ Is it used primarily to decouple a controller/caller from a single receiver?
          â””â”€â”€ YES â”€â”€â–º MEDIATOR + COMMAND PATTERN
```

---

## Pattern 5: Chain of Responsibility (Behavioral)

### 1. What is the Problem? (The Nested `if/else` Nightmare)

Imagine you are building an **Order Processing API** or an **API Gateway**. 

Before an order can reach your core database logic, it must pass through **4 mandatory checks**:
1. **Authentication Check:** Is the user logged in with a valid JWT token?
2. **Rate Limiting Check:** Has this user sent more than 100 requests in the last minute (DDoS check)?
3. **Validation Check:** Is the JSON body valid (e.g., amount > 0, items not empty)?
4. **Fraud / Risk Check:** Is this order flagged for suspected fraud?

#### The Bad Code (The Monolithic Mega-Method):
```csharp
// ÂÅ’ Architectural Disaster: A single method doing 4 completely unrelated jobs
public void ProcessOrder(OrderRequest request)
{
    // Step 1
    if (!AuthService.IsAuthenticated(request.Token))
    {
        Console.WriteLine("Unauthorized!");
        return;
    }

    // Step 2
    if (!RateLimiter.IsAllowed(request.IpAddress))
    {
        Console.WriteLine("Rate limit exceeded!");
        return;
    }

    // Step 3
    if (request.Amount <= 0)
    {
        Console.WriteLine("Invalid order payload!");
        return;
    }

    // Step 4
    if (FraudService.IsSuspicious(request))
    {
        Console.WriteLine("Flagged for fraud!");
        return;
    }

    // Finally... the actual business logic!
    Database.SaveOrder(request);
}
```

#### Why does an Architect reject this?
* **Violates Single Responsibility Principle (SRP):** This class handles security, networking, validation, fraud analysis, and order persistence all at once.
* **Brittle & Inflexible:** What if you want to turn off Rate Limiting in your development environment? What if you want to reorder the checks? What if you want to add a 5th check (*Geo-IP Blacklisting*)? You have to keep editing this risky method.

---

### 2. The Core Concept (Plain English)

> **"Pass requests along a chain of handlers. Upon receiving a request, each handler decides either to process the request or to pass it to the next handler in the chain (or short-circuit and stop)."**

**The Analogy: Airport Security Checkpoint ðŸ›‚**  
When you enter an international airport to board a flight:
1. **Station 1 (Boarding Pass Gate):** Scans your ticket. If invalid, you are turned away (**short-circuit**). If valid, you walk to Station 2.
2. **Station 2 (Security / X-Ray):** Scans your luggage for prohibited items. If flagged, you are detained (**short-circuit**). If clear, you walk to Station 3.
3. **Station 3 (Passport Control / Immigration):** Checks your visa. If denied, you can't fly. If stamped, you proceed to the gate.

No single security officer does all 3 jobs. Each officer does **one job**, and either passes you forward or stops you.

```
Request â”€â”€> [ AuthHandler ] â”€â”€> [ RateLimitHandler ] â”€â”€> [ ValidationHandler ] â”€â”€> [ Execute Order ]
                 â”‚                     â”‚                          â”‚
                 â–¼ (If fails)          â–¼ (If fails)               â–¼ (If fails)
            (Short-circuit!)      (Short-circuit!)           (Short-circuit!)
```

---

### 3. Implementation in Modern C#

#### Step 1: The Request Context Model
```csharp
public class OrderRequest
{
    public string Token { get; set; } = string.Empty;
    public string UserIp { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
```

#### Step 2: The Base Handler (Manages the Chain Link)
The base class holds a pointer to the **next** handler in line:

```csharp
public abstract class OrderHandler
{
    private OrderHandler? _next;

    // Fluent method to link handlers together: handlerA.SetNext(handlerB).SetNext(handlerC)
    public OrderHandler SetNext(OrderHandler next)
    {
        _next = next;
        return next; // Returns next handler to enable chaining
    }

    public virtual void Handle(OrderRequest request)
    {
        // Default behavior: pass to the next link if it exists
        _next?.Handle(request);
    }
}
```

#### Step 3: Concrete Handlers (Each focuses on ONE responsibility)

```csharp
// Handler 1: Authentication
public class AuthenticationHandler : OrderHandler
{
    public override void Handle(OrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token) || request.Token != "valid-jwt-token")
        {
            Console.WriteLine("ðŸ›‘ [AuthHandler] Access Denied: Invalid or missing token. (CHAIN STOPPED)");
            return; // SHORT-CIRCUITS the chain! Does not call base.Handle()
        }

        Console.WriteLine("âœ… [AuthHandler] User authenticated successfully.");
        base.Handle(request); // Passes to next handler
    }
}

// Handler 2: Rate Limiting
public class RateLimitingHandler : OrderHandler
{
    public override void Handle(OrderRequest request)
    {
        if (request.UserIp == "192.168.1.50") // Simulated blocked IP
        {
            Console.WriteLine("ðŸ›‘ [RateLimitHandler] Too many requests from this IP. (CHAIN STOPPED)");
            return; // Short-circuits!
        }

        Console.WriteLine("âœ… [RateLimitHandler] Rate limit check passed.");
        base.Handle(request);
    }
}

// Handler 3: Business Validation
public class ValidationHandler : OrderHandler
{
    public override void Handle(OrderRequest request)
    {
        if (request.Amount <= 0)
        {
            Console.WriteLine("ðŸ›‘ [ValidationHandler] Order amount must be greater than zero. (CHAIN STOPPED)");
            return; // Short-circuits!
        }

        Console.WriteLine("âœ… [ValidationHandler] Order payload validated.");
        base.Handle(request);
    }
}

// Final Handler: Core Business Execution
public class OrderExecutionHandler : OrderHandler
{
    public override void Handle(OrderRequest request)
    {
        Console.WriteLine($"ðŸŽ‰ [OrderExecutionHandler] Order of ${request.Amount} successfully processed and saved to database!");
    }
}
```

---

### 4. Running the Code

```csharp
class Program
{
    static void Main()
    {
        // 1. Build the Chain: Auth -> RateLimit -> Validation -> Execution
        var auth = new AuthenticationHandler();
        var rateLimit = new RateLimitingHandler();
        var validation = new ValidationHandler();
        var execution = new OrderExecutionHandler();

        auth.SetNext(rateLimit)
            .SetNext(validation)
            .SetNext(execution);

        Console.WriteLine("=== SCENARIO 1: Bad Token (Should stop at Auth) ===");
        auth.Handle(new OrderRequest { Token = "expired-token", UserIp = "10.0.0.1", Amount = 150m });

        Console.WriteLine("\n=== SCENARIO 2: Rate Limited IP (Should stop at RateLimit) ===");
        auth.Handle(new OrderRequest { Token = "valid-jwt-token", UserIp = "192.168.1.50", Amount = 150m });

        Console.WriteLine("\n=== SCENARIO 3: Invalid Amount (Should stop at Validation) ===");
        auth.Handle(new OrderRequest { Token = "valid-jwt-token", UserIp = "10.0.0.1", Amount = -5m });

        Console.WriteLine("\n=== SCENARIO 4: Perfect Request (Flows all the way through!) ===");
        auth.Handle(new OrderRequest { Token = "valid-jwt-token", UserIp = "10.0.0.1", Amount = 250m });
    }
}
```

#### Output:
```text
=== SCENARIO 1: Bad Token (Should stop at Auth) ===
ðŸ›‘ [AuthHandler] Access Denied: Invalid or missing token. (CHAIN STOPPED)

=== SCENARIO 2: Rate Limited IP (Should stop at RateLimit) ===
âœ… [AuthHandler] User authenticated successfully.
ðŸ›‘ [RateLimitHandler] Too many requests from this IP. (CHAIN STOPPED)

=== SCENARIO 3: Invalid Amount (Should stop at Validation) ===
âœ… [AuthHandler] User authenticated successfully.
âœ… [RateLimitHandler] Rate limit check passed.
ðŸ›‘ [ValidationHandler] Order amount must be greater than zero. (CHAIN STOPPED)

=== SCENARIO 4: Perfect Request (Flows all the way through!) ===
âœ… [AuthHandler] User authenticated successfully.
âœ… [RateLimitHandler] Rate limit check passed.
âœ… [ValidationHandler] Order payload validated.
ðŸŽ‰ [OrderExecutionHandler] Order of $250 successfully processed and saved to database!
```

---

### 5. The Architect's Deep Dive: Where .NET Lives and Breathes This

#### A. ASP.NET Core Middleware Pipeline
The ASP.NET Core Middleware Pipeline is an asynchronous Chain of Responsibility:
```csharp
app.UseExceptionHandler(); // Link 1
app.UseHttpsRedirection(); // Link 2
app.UseAuthentication();   // Link 3
app.UseAuthorization();    // Link 4
app.MapControllers();      // Final link!
```
Every middleware receives `HttpContext` and a `RequestDelegate next`. 
If authentication fails, `app.UseAuthentication()` short-circuits with a `401 Unauthorized` and **never calls `await next()`**!

#### B. `HttpClient` and `DelegatingHandler`
When you make HTTP calls using `HttpClient`, you can plug in a chain of `DelegatingHandler`s (e.g., Polly retry policies, logging headers, auth token refresh).

---

### Summary Comparison: Chain of Responsibility vs. Decorator

Developers often ask: *"Both Chain of Responsibility and Decorator wrap objects and pass calls forward. What is the difference?"*

| Feature | Chain of Responsibility | Decorator |
| :--- | :--- | :--- |
| **Short-Circuiting** | **Core Feature:** Any link in the chain can choose to stop and abort the request. | **Rare:** Usually, all decorators execute to add behavior before/after. |
| **Execution Order** | Can break or stop at any point; handlers don't necessarily know about the full chain. | Every layer usually wraps the previous one completely (nested onion). |
| **Intent** | **Passing along a chain of handlers** to find who handles or filters the request. | **Adding features** (logging, caching) to an existing object. |


---

## Pattern 6: State (Behavioral)

### 1. What is the Problem? (The Real-World Pain)
Consider an e-commerce order lifecycle: `Draft` $\rightarrow$ `Submitted` $\rightarrow$ `Paid` $\rightarrow$ `Shipped` $\rightarrow$ `Cancelled`.

Without the State Pattern, domain entities become bloated with fragile, nested `switch` statements across every single method:

```csharp
// âŒ The Nested Switch Statement Hell (Violates SRP & OCP)
public class Order
{
    public OrderStatus Status { get; private set; }

    public void Ship()
    {
        switch (Status)
        {
            case OrderStatus.Draft:
                throw new InvalidOperationException("Cannot ship a draft!");
            case OrderStatus.Submitted:
                throw new InvalidOperationException("Cannot ship an unpaid order!");
            case OrderStatus.Cancelled:
                throw new InvalidOperationException("Cannot ship a cancelled order!");
            case OrderStatus.Shipped:
                throw new InvalidOperationException("Already shipped!");
            case OrderStatus.Paid:
                // Actual shipping logic...
                Status = OrderStatus.Shipped;
                break;
        }
    }

    public void Cancel()
    {
        // Another 40 lines of switch statements...
    }
}
```
Every time business introduces a new status (e.g., `Refunded`, `PartiallyShipped`), you must open and edit **every single method**, risking severe regression bugs where invalid transitions slip through.

---

### 2. The Core Concept (Plain English)

> **"Allow an object to alter its behavior when its internal state changes. The object will appear to change its class."**

Instead of one monolithic class switching on an enum:
1. Define an abstract `OrderState` base class declaring all possible domain actions (`AddItem()`, `Pay()`, `Ship()`, `Cancel()`).
2. Implement a concrete class for each state (`DraftOrderState`, `PaidOrderState`, `ShippedOrderState`, `CancelledOrderState`).
3. The `Order` delegates incoming actions directly to its current state object: `_state.Ship(this)`.
4. The state object itself controls whether the action is legal and executes the transition: `order.TransitionTo(new ShippedOrderState())`.

```
                  â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
                  â”‚  Draft State  â”‚
                  â””â”€â”€â”€â”€â”€â”€â”€â”¬â”€â”€â”€â”€â”€â”€â”€â”˜
                          â”‚ Pay()
                          â–¼
                  â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
                  â”‚  Paid State   â”‚
                  â””â”€â”€â”€â”€â”€â”€â”€â”¬â”€â”€â”€â”€â”€â”€â”€â”˜
                          â”‚ Ship()
                          â–¼
                  â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
                  â”‚ Shipped State â”‚ (Cannot Cancel! Cannot Ship again!)
                  â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

---

### 3. Implementation in Modern C# (.NET 8/9)

#### Step 1: The Abstract State Base Class
```csharp
public abstract class OrderState
{
    // Default implementation: reject illegal operations
    public virtual void AddItem(Order order, string item) 
        => throw new InvalidOperationException($"Cannot add items while in '{GetType().Name}' state.");

    public virtual void Pay(Order order) 
        => throw new InvalidOperationException($"Cannot pay while in '{GetType().Name}' state.");

    public virtual void Ship(Order order) 
        => throw new InvalidOperationException($"Cannot ship while in '{GetType().Name}' state.");

    public virtual void Cancel(Order order) 
        => throw new InvalidOperationException($"Cannot cancel while in '{GetType().Name}' state.");
}
```

#### Step 2: Concrete State Implementations
```csharp
// 1. Draft State: Can add items, pay, or cancel
public class DraftOrderState : OrderState
{
    public override void AddItem(Order order, string item)
    {
        order.Items.Add(item);
        Console.WriteLine($"Added item '{item}' to draft order.");
    }

    public override void Pay(Order order)
    {
        if (order.Items.Count == 0)
            throw new InvalidOperationException("Cannot pay for an empty order.");

        Console.WriteLine("Payment authorized.");
        order.TransitionTo(new PaidOrderState());
    }

    public override void Cancel(Order order)
    {
        Console.WriteLine("Draft order cancelled.");
        order.TransitionTo(new CancelledOrderState());
    }
}

// 2. Paid State: Can ship or cancel (with refund)
public class PaidOrderState : OrderState
{
    public override void Ship(Order order)
    {
        Console.WriteLine("Order shipped via courier.");
        order.TransitionTo(new ShippedOrderState());
    }

    public override void Cancel(Order order)
    {
        Console.WriteLine("Order refunded and cancelled.");
        order.TransitionTo(new CancelledOrderState());
    }
}

// 3. Shipped State (Terminal: No cancellation or reshipping allowed)
public class ShippedOrderState : OrderState
{
    public override void Cancel(Order order) 
        => throw new InvalidOperationException("Cannot cancel an order that has already shipped!");
}

// 4. Cancelled State (Terminal: Inactive)
public class CancelledOrderState : OrderState { }
```

#### Step 3: The Order Context Entity
```csharp
public class Order
{
    private OrderState _state;
    public List<string> Items { get; } = new();

    public Order()
    {
        _state = new DraftOrderState(); // Initial state
    }

    // State transition hook
    internal void TransitionTo(OrderState newState)
    {
        Console.WriteLine($"[TRANSITION] {_state.GetType().Name} â”€â”€> {newState.GetType().Name}");
        _state = newState;
    }

    // Delegate behavior directly to the current state!
    public void AddItem(string item) => _state.AddItem(this, item);
    public void Pay() => _state.Pay(this);
    public void Ship() => _state.Ship(this);
    public void Cancel() => _state.Cancel(this);
}
```

#### Verification:
```csharp
var order = new Order();
order.AddItem("Mechanical Keyboard");
order.Pay();   // [TRANSITION] DraftOrderState â”€â”€> PaidOrderState
order.Ship();  // [TRANSITION] PaidOrderState â”€â”€> ShippedOrderState

// Illegal operation test:
order.Cancel(); // ðŸ’¥ Throws InvalidOperationException: Cannot cancel an order that has already shipped!
```

> **Production Tip:** For large enterprise workflows with 15+ states, persistence to SQL, and triggers, consider using the battle-tested **`Stateless`** NuGet library by Nicholas Blumhardt.

# 5. Top C# / .NET-Specific Enterprise Patterns

## .NET Pattern 1: Repository & Unit of Work

### 1. What is the Problem? (The Real-World Pain)

Imagine you are building an enterprise banking or e-commerce app. A controller or business service needs to process an order:

```csharp
// âŒ BAD: Business service tightly coupled to Entity Framework Core
public class OrderService
{
    private readonly AppDbContext _context; // Direct database dependency!

    public OrderService(AppDbContext context) => _context = context;

    public async Task PlaceOrderAsync(int customerId, List<OrderItem> items)
    {
        // Raw EF Core LINQ queries scattered inside business logic:
        var customer = await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId && c.IsActive);

        // Deduct loyalty points...
        customer.LoyaltyPoints -= 100;

        var order = new Order { CustomerId = customerId, Items = items };
        await _context.Orders.AddAsync(order);

        // What if deducting loyalty points succeeds, but saving the order crashes?
        await _context.SaveChangesAsync();
    }
}
```

#### Why does an Architect cringe looking at this?
1. **Zero Abstraction:** Your business logic is married to Entity Framework Core and SQL Server. If tomorrow management wants to switch a sub-module to MongoDB, Redis, or an external REST API, you have to rewrite your entire business layer.
2. **Untestable Business Logic:** You cannot easily unit test `OrderService` without setting up an actual SQL database or EF Core in-memory provider (which often behaves differently from real SQL).
3. **Partial Saves (Data Inconsistency):** If Service A calls `SaveChangesAsync()` midway through a workflow, and Service B fails 2 seconds later, your database is left in a corrupted, half-saved state.

---

### 2. The Core Concept (Plain English)

This pattern is a dynamic duo of two collaborating ideas:

#### A. The Repository (The In-Memory Illusion)
A **Repository** mediates between the domain and data mapping layers. To your business logic, a repository pretends to be a simple **in-memory collection** (like a `List<Customer>`). 
Your business logic doesn't care whether data comes from PostgreSQL, SQL Server, an Azure Blob, or a mock list in a unit test.

#### B. The Unit of Work (The Transaction Coordinator)
A **Unit of Work** maintains a list of objects affected by a business transaction and coordinates the writing out of changes. 
Instead of each repository calling "Save" individually, they all register their changes with the **Unit of Work**, which commits **all of them together in a single atomic database transaction** (`COMMIT`) or rolls everything back if any single step fails (`ROLLBACK`).

**Analogy: The Shopping Cart Checkout ðŸ›’**
* You pick an Apple $\rightarrow$ put it in the cart (Repository `Add`).
* You pick a Bread $\rightarrow$ put it in the cart (Repository `Add`).
* You return a bruised Orange $\rightarrow$ put it back on the shelf (Repository `Remove`).
* None of this charges your credit card yet!
* You walk to the cashier (**The Unit of Work**) and swipe your card once (**`Commit()`**). Either the entire purchase succeeds together, or the whole transaction is declined.

---

### 3. Implementation in Modern C#

#### Step 1: The Repository Contracts
An architect creates **domain-specific** repositories (not just generic CRUD):

```csharp
public interface ICustomerRepository
{
    Task<Customer?> GetActiveCustomerWithOrdersAsync(int customerId);
    void Update(Customer customer);
}

public interface IOrderRepository
{
    Task AddAsync(Order order);
}
```

#### Step 2: The Unit of Work Contract
The Unit of Work groups repositories and controls the transaction boundary:

```csharp
public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    
    // The single commit point for the entire business operation!
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

#### Step 3: Implementation with EF Core

```csharp
// Concrete Customer Repository
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context) => _context = context;

    public async Task<Customer?> GetActiveCustomerWithOrdersAsync(int customerId)
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId && c.IsActive);
    }

    public void Update(Customer customer) => _context.Customers.Update(customer);
}

// Concrete Order Repository
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Order order) => await _context.Orders.AddAsync(order);
}

// Concrete Unit of Work
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICustomerRepository Customers { get; }
    public IOrderRepository Orders { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
        Orders = new OrderRepository(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Executes everything in a single atomic SQL transaction!
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _context.Dispose();
}
```

#### Step 4: Clean Business Logic
Look at how pure and testable `OrderService` becomes:

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    // Injects ONLY the Unit of Work!
    public OrderService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task PlaceOrderAsync(int customerId, List<OrderItem> items)
    {
        var customer = await _unitOfWork.Customers.GetActiveCustomerWithOrdersAsync(customerId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found or inactive.");

        customer.LoyaltyPoints -= 100;
        _unitOfWork.Customers.Update(customer);

        var order = new Order { CustomerId = customerId, Items = items };
        await _unitOfWork.Orders.AddAsync(order);

        // ONE SINGLE ATOMIC SAVE:
        await _unitOfWork.SaveChangesAsync();
    }
}
```

---

### 4. The Architect's Debate: The EF Core Controversy âš ï¸

If you participate in modern .NET architecture discussions, you will hear this famous debate:

> **"Wait! Isn't EF Core's `DbContext` already a Unit of Work, and `DbSet<T>` already a Repository?"**

**Yes, technically it is!** 
- `DbSet<Customer>` has `.Add()`, `.Remove()`, `.Find()`. (It acts like a Repository).
- `DbContext.SaveChangesAsync()` tracks changes and writes them in one transaction. (It acts like a Unit of Work).

#### So why do architects still wrap EF Core in custom Repositories?

| When to SKIP custom Repository (Use `DbContext` directly) | When to USE custom Repository & Unit of Work |
| :--- | :--- |
| Simple CRUD applications or internal admin tools. | **Clean Architecture / Domain-Driven Design (DDD)** where the Core Domain layer must NOT reference EF Core DLLs. |
| The team wants to use raw EF Core LINQ capabilities (`.Select()`, `.GroupBy()`) freely in API queries. | Complex business applications where you must **mock data access in pure unit tests** without database emulators. |
| Small projects where extra abstraction layers add unnecessary boilerplate. | Applications integrating multiple data stores (e.g., SQL + MongoDB + Redis) under a single unified domain contract. |

#### The Architect's Golden Rule:
Never write a **"Generic Repository"** like `IRepository<T>` that just exposes `.GetAll()`, `.Add(T)`. That just creates a useless, crippled wrapper around `DbSet<T>`. 
Instead, write **Domain-Specific Repositories** (`IOrderRepository`, `ICustomerRepository`) with specialized business query methods (`GetPendingOrdersForApproval()`).

---

## .NET Pattern 2: The Options Pattern

### 1. What is the Problem? (The Magic String Disaster)

Imagine you need to configure an **Email Notification Service** or a **Payment Gateway** from `appsettings.json`:

```json
{
  "SmtpSettings": {
    "Host": "smtp.mailgun.org",
    "Port": 587,
    "EnableSsl": true,
    "ApiKey": "secret-key-123"
  }
}
```

#### The Junior Developer's Approach:
They inject `IConfiguration` directly into the service:

```csharp
// âŒ BAD: Fragile, untestable, filled with magic strings
public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string to, string body)
    {
        // Magic strings everywhere!
        string host = _configuration["SmtpSettings:Host"]; 
        int port = int.Parse(_configuration["SmtpSettings:Port"]); // Crashes if null or string!
        bool ssl = bool.Parse(_configuration["SmtpSettings:EnableSsl"]);
        string apiKey = _configuration["SmtpSettings:ApiKey"];

        // Connect and send...
    }
}
```

#### Why does an Architect reject this code?
1. **Magic Strings & Typos:** If someone types `SmtpSettings:Hst` instead of `Host`, the code compiles fine and **crashes in production at runtime**.
2. **Manual Parsing & Fragility:** Having to write `int.Parse()` or `bool.Parse()` manually everywhere is error-prone. What if `Port` is missing or someone types `"five-eight-seven"`?
3. **Violates Interface Segregation Principle (ISP):** Why does `EmailService` have access to the entire `IConfiguration` dictionary (including Database connection strings and JWT secrets)?
4. **Testing Nightmare:** To unit-test `EmailService`, you have to mock the entire `IConfiguration` key-value tree instead of just passing a simple settings object.

---

### 2. The Core Concept (Plain English)

> **"Use strongly-typed classes to provide strongly-typed access to groups of related settings."**

Instead of passing the entire dictionary of raw configuration keys, you bind a specific section of `appsettings.json` to a **strongly-typed C# class (POCO)**. 
Your service only receives the exact settings it needs, fully typed, with auto-completion and compile-time safety.

---

### 3. Implementation in Modern C#

#### Step 1: The Strongly-Typed Options Class
Create a clean C# class that mirrors the JSON structure:

```csharp
public class SmtpOptions
{
    public const string SectionName = "SmtpSettings";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587; // Default value
    public bool EnableSsl { get; set; } = true;
    public string ApiKey { get; set; } = string.Empty;
}
```

#### Step 2: Register in `Program.cs`
In modern .NET, you bind and configure it with one line in your DI container:

```csharp
// In Program.cs
var builder = WebApplication.CreateBuilder(args);

// Binds the "SmtpSettings" section from appsettings.json to SmtpOptions
builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection(SmtpOptions.SectionName));

// Or the modern .NET 8+ syntax:
// builder.Services.AddOptions<SmtpOptions>()
//     .BindConfiguration(SmtpOptions.SectionName);
```

#### Step 3: Consume via `IOptions<T>` in your Service
Notice how clean the service becomes:

```csharp
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
    private readonly SmtpOptions _options;

    // .NET automatically injects the bound SmtpOptions!
    public EmailService(IOptions<SmtpOptions> options)
    {
        // The .Value property holds your strongly-typed object
        _options = options.Value;
    }

    public void SendEmail(string to, string body)
    {
        // Pure strongly-typed access:
        Console.WriteLine($"Connecting to {_options.Host}:{_options.Port} (SSL: {_options.EnableSsl})...");
        Console.WriteLine($"Authenticating with ApiKey: {_options.ApiKey}");
        Console.WriteLine($"Sending email to {to}...");
    }
}
```

---

### 4. The Architect's Deep Dive: The 3 Flavors of Options

This is one of the **most famous Senior .NET Interview questions**:

> *"What is the difference between `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>`?"*

Microsoft provides 3 different interfaces, and choosing the wrong one can cause bugs or crashes:

| Interface | Service Lifetime | Reloads on `appsettings.json` change? | Best Used For... |
| :--- | :--- | :--- | :--- |
| **`IOptions<T>`** | **Singleton** | âŒ **No.** Reads config once at startup. Never changes. | Background workers, Singletons, or static settings that never change without restarting the app. |
| **`IOptionsSnapshot<T>`** | **Scoped** | âœ… **Yes.** Recomputed on every new HTTP request. | Standard Web APIs and Controllers where you want live config changes per request. *(Cannot be injected into Singletons!)* |
| **`IOptionsMonitor<T>`** | **Singleton** | âœ… **Yes.** Live updates via `CurrentValue` and has an `OnChange` event. | Singletons or background jobs that need real-time hot-reloading when `appsettings.json` is edited on disk. |

#### Example of `IOptionsMonitor<T>` (Hot-Reloading without App Restart):
```csharp
public class PaymentGatewayService
{
    private readonly IOptionsMonitor<PaymentOptions> _monitor;

    public PaymentGatewayService(IOptionsMonitor<PaymentOptions> monitor)
    {
        _monitor = monitor;

        // Triggers instantly whenever an admin edits appsettings.json in production!
        _monitor.OnChange(newOptions =>
        {
            Console.WriteLine($"[HOT RELOAD] Payment timeout changed to: {newOptions.TimeoutSeconds}s");
        });
    }

    public void Charge()
    {
        // Always gets the latest, live value from disk!
        var currentTimeout = _monitor.CurrentValue.TimeoutSeconds;
    }
}
```

---

### 5. Architect Superpower: Startup Validation (`ValidateOnStart`)

What if a developer deploys to production, but forgets to set `ApiKey` in the production environment?
Without validation, the app boots up fine, and **fails 3 hours later** when a user tries to checkout.

In modern .NET (6/7/8/9), you can enforce **Compile/Startup Validation** with Data Annotations:

```csharp
using System.ComponentModel.DataAnnotations;

public class SmtpOptions
{
    [Required(ErrorMessage = "SMTP Host is mandatory!")]
    public string Host { get; set; } = string.Empty;

    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "API Key must be at least 10 characters.")]
    public string ApiKey { get; set; } = string.Empty;
}
```

And in `Program.cs`:
```csharp
builder.Services.AddOptions<SmtpOptions>()
    .BindConfiguration("SmtpSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart(); // <-- THE MAGIC LINE!
```

**What happens now?**
If `ApiKey` is missing or `Port` is 0, the application **refuses to start and crashes immediately at startup** with a crystal-clear error message during deployment. 
You catch configuration bugs in seconds rather than discovering them in production logs at midnight!

---

### Summary Checklist

| Without Options Pattern | With Options Pattern |
| :--- | :--- |
| Magic strings: `_config["Section:Key"]` | Strongly-typed properties: `_options.Host` |
| Runtime crashes when types fail to parse | Automatic binding with defaults |
| Service has access to entire config database | Service only receives its own sliced settings |
| No validation; errors happen when code runs | `ValidateOnStart()` catches invalid configs at boot |
| App must be restarted to change configs | `IOptionsSnapshot` / `IOptionsMonitor` support live hot-reload |

---

## Deep Dive: .NET Garbage Collection (GC) Internals

### The Critical Distinction: Managed vs. Unmanaged Memory

```
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚                       THE .NET RUNTIME                      â”‚
â”‚                                                             â”‚
â”‚  [ Managed Resources ]          [ Unmanaged Resources ]     â”‚
â”‚  â€¢ C# Class instances           â€¢ OS File Handles (IntPtr)  â”‚
â”‚  â€¢ Strings                      â€¢ Network Sockets           â”‚
â”‚  â€¢ Arrays & Lists               â€¢ Database Connections      â”‚
â”‚  â€¢ Integers, Booleans           â€¢ C/C++ DLL pointers        â”‚
â”‚                                                             â”‚
â”‚  (GC tracks and cleans          (GC knows NOTHING           â”‚
â”‚   this automatically!)           about these!)              â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

* **Managed Resources:** Memory allocated on the managed heap. The GC tracks object references and automatically frees memory when objects are no longer reachable.
* **Unmanaged Resources:** Operating system resources that live outside the CLR (open files on disk, TCP sockets, database connection pool slots, OS window handles).
  > **The Big Problem:** The Garbage Collector **only knows about RAM**. It does **not** know that your 20-byte C# object is holding a lock on a 10-Gigabyte file on disk! If the GC doesn't collect your object, the file remains locked forever.

### How the GC Cleans Up: Generations

The .NET GC is a generational, mark-and-sweep collector:
```
[ Gen 0 ] â”€â”€(Survives GC)â”€â”€> [ Gen 1 ] â”€â”€(Survives GC)â”€â”€> [ Gen 2 ]
(Short-lived:                 (Buffer zone:               (Long-lived:
 method variables)             survived 1 collection)      Singletons, Caches)
   âš¡ Blazing Fast                 ðŸŸ¡ Fast                     ðŸ¢ Slow & Heavy
   Runs frequently               Runs occasionally           Runs rarely
```
1. **Generation 0:** Short-lived objects (method local variables). Collected in microseconds.
2. **Generation 1:** Buffer zone for objects surviving one collection cycle.
3. **Generation 2:** Long-lived objects (Singletons, static caches). Expensive full collection cycle.

---

## Deep Dive: Dispose vs. Finalize

### A. Can a class have both Dispose and a Finalizer?
**Yes!** This is the classic Microsoft **"Seatbelt and Airbag"** model:
* `Dispose()` is the **Seatbelt** (explicit, fast, deterministic). It cleans up immediately and calls `GC.SuppressFinalize(this)` to disable the airbag.
* The Finalizer `~MyClass()` is the **Airbag** (safety net). If the developer forgot to call `Dispose()`, the GC calls the finalizer as a last resort on a background thread.

### B. What happens if a class implements neither?
**For 95% of C# classes (pure managed data), this is ideal!** The GC cleans them up in Gen 0 in microseconds without finalizer queue overhead.

### C. What happens if a class has unmanaged resources and implements neither?
**A Permanent Native Resource Leak.**
1. The GC reclaims the 24-byte C# wrapper object.
2. The native pointer (`IntPtr`) is lost forever.
3. The 50MB of native memory or OS file handle is orphaned in Kernel memory.
4. Causes **Invisible OutOfMemory crashes**, **File is locked errors**, or **OS Handle Exhaustion (10,000 limit)**.
5. The resource is **only** freed when the Operating System kills the entire process.

### D. Can we skip Dispose and rely only on the Finalizer?
**NO! Fatal consequences of skipping Dispose:**
1. *Zero Memory Pressure Trap:* A 32-byte SQL connection creates zero GC pressure. The GC takes a nap while your database connection pool (max 100) exhausts and crashes the website.
2. *File Remains Locked:* The file on disk cannot be read or emailed until the GC decides to run minutes later.
3. *Single Finalizer Thread:* The entire .NET runtime has **only 1 finalizer thread**. Unfinalized objects queue up and stall the engine.
4. *Finalizers cannot touch managed objects:* Referencing managed children inside a finalizer crashes the process with `ObjectDisposedException`.

### Comparison: Dispose vs. Finalize

| Feature | `Dispose()` | `Finalize()` (`~Destructor`) |
| :--- | :--- | :--- |
| **Interface / Syntax** | Implements `IDisposable` | Declares `~ClassName()` |
| **Who calls it?** | **You** (the developer or `using` statement) | **The GC** (on a background thread) |
| **When does it run?** | **Immediately** (Deterministic) | **Whenever the GC feels like it** (Non-deterministic) |
| **Cleans up...** | Both **Managed** AND **Unmanaged** resources | **ONLY Unmanaged** resources |
| **Performance Impact** | âš¡ Zero penalty (Clean & Fast) | ðŸ¢ Heavy penalty (Promotes object to Gen 2) |
| **Real-world Analogy** | Returning your hotel room key at the front desk when checking out. | Leaving the hotel without checking out; the maid eventually clears your room 3 days later. |

---

## .NET Pattern 3: The Complete Dispose Pattern

### Standard Implementation (`IDisposable` & `IAsyncDisposable`)

```csharp
public class FileManager : IDisposable, IAsyncDisposable
{
    // 1. Unmanaged resource (e.g., OS file handle or native buffer pointer)
    private IntPtr _unmanagedBuffer;

    // 2. Managed disposable resource (e.g., a .NET FileStream or HttpClient)
    private FileStream? _managedStream;

    // 3. Track whether Dispose has already been called (Idempotency)
    private bool _disposed = false;

    public FileManager(string filePath)
    {
        _managedStream = new FileStream(filePath, FileMode.OpenOrCreate);
        _unmanagedBuffer = Marshal.AllocHGlobal(1024); // Allocate native memory
    }

    // ====================================================================
    // A. Public IDisposable implementation (Deterministic Cleanup)
    // ====================================================================
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this); // Tell GC: Skip the slow finalizer!
    }

    // ====================================================================
    // B. The Core Virtual Cleanup Engine (Handles inheritance & safety)
    // ====================================================================
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

    // ====================================================================
    // C. The Finalizer / Destructor (The Safety Net for lazy callers)
    // ====================================================================
    ~FileManager()
    {
        // disposing: false means: "ONLY clean unmanaged resources!"
        Dispose(disposing: false);
    }

    // ====================================================================
    // D. Modern Asynchronous Disposal (await using)
    // ====================================================================
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

#### How callers consume it with `await using`:
```csharp
public async Task ProcessDataAsync()
{
    await using (var manager = new FileManager("data.bin"))
    {
        // Do async file work...
    } // Calls await manager.DisposeAsync() without blocking thread pool threads!
}
```

---

## .NET Pattern 4: The Result Pattern (Railway-Oriented Programming)

### 1. What is the Problem? (The Exception Anti-Pattern)

```csharp
// âŒ ANTI-PATTERN: Using exceptions to control normal business flow
public class BankAccountService
{
    public void Withdraw(int accountId, decimal amount)
    {
        var account = _repository.GetById(accountId);

        if (account == null)
            throw new AccountNotFoundException("Account does not exist");

        if (account.IsLocked)
            throw new AccountLockedException("Account is locked");

        if (account.Balance < amount)
            throw new InsufficientFundsException("Not enough balance");

        account.Balance -= amount;
        _repository.Update(account);
    }
}
```

#### Why is throwing exceptions here an architectural disaster?
1. **Business Failures are NOT Exceptional:** Insufficient balance or invalid coupon codes are expected everyday user outcomes.
2. **The "Silent Contract" Problem:** C# methods don't declare thrown exceptions. Callers don't know what exceptions to catch without inspecting internal code.
3. **Massive Stack Trace Performance Penalty:** Constructing stack traces is **100x to 1000x slower** than returning an object.
4. **Controller `try/catch` Spaghetti:** Web API controllers get bloated with 5 nested catch blocks translating exceptions to HTTP status codes.

---

### 2. The Core Concept: Railway-Oriented Programming ðŸš‚

```
                â”Œâ”€â”€â”€ [Validate Account] â”€â”€â”€ [Check Balance] â”€â”€â”€ [Deduct Balance] â”€â”€â”€â–º [SUCCESS TRACK (Green)]
                â”‚           â”‚                     â”‚
[START REQUEST] â”¤           â–¼ (If null)           â–¼ (If < amount)
                â”‚      Switch to Red         Switch to Red
                â””â”€â”€â”€â–º [FAILURE TRACK (Red)] â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â–º [RETURN ERROR]
```

* **Success Track (Green):** Carries the data value (`TValue`).
* **Failure Track (Red):** The moment any check fails, execution switches to the failure track and returns an `Error` object.

---

### 3. Implementation in Modern C#

#### Step 1: The Domain Error Model
```csharp
public enum ErrorType { Failure, NotFound, Validation, Conflict }

public record Error(string Code, string Description, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error NotFound(string code, string desc) => new(code, desc, ErrorType.NotFound);
    public static Error Validation(string code, string desc) => new(code, desc, ErrorType.Validation);
    public static Error Conflict(string code, string desc) => new(code, desc, ErrorType.Conflict);
}
```

#### Step 2: The `Result` and `Result<TValue>` Classes
```csharp
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            throw new InvalidOperationException("Invalid result state");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static Result<TValue> Success(TValue value) => new(value, true, Error.None);
    public static new Result<TValue> Failure(Error error) => new(default, false, error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
}
```

#### Step 3: Rewriting Business Logic with the Result Pattern
```csharp
public static class BankErrors
{
    public static readonly Error AccountNotFound = 
        Error.NotFound("Account.NotFound", "The requested bank account does not exist.");
    public static readonly Error AccountLocked = 
        Error.Conflict("Account.Locked", "Account is temporarily locked.");
    public static readonly Error InsufficientFunds = 
        Error.Validation("Account.InsufficientFunds", "Account balance is insufficient for this withdrawal.");
}

public class BankAccountService
{
    public Result<decimal> Withdraw(int accountId, decimal amount)
    {
        var account = _repository.GetById(accountId);

        if (account == null) return BankErrors.AccountNotFound;
        if (account.IsLocked) return BankErrors.AccountLocked;
        if (account.Balance < amount) return BankErrors.InsufficientFunds;

        account.Balance -= amount;
        _repository.Update(account);

        return account.Balance; // Implicit conversion to Result<decimal>.Success!
    }
}
```

#### Step 4: Consuming in an ASP.NET Core API Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly BankAccountService _accountService;

    public AccountsController(BankAccountService accountService) => _accountService = accountService;

    [HttpPost("{id}/withdraw")]
    public IActionResult Withdraw(int id, [FromBody] decimal amount)
    {
        Result<decimal> result = _accountService.Withdraw(id, amount);

        if (result.IsSuccess)
            return Ok(new { RemainingBalance = result.Value });

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(result.Error),
            ErrorType.Validation => BadRequest(result.Error),
            ErrorType.Conflict => Conflict(result.Error),
            _ => StatusCode(500, result.Error)
        };
    }
}
```

---

### 4. The Architect's Decision Matrix: Result vs. Exception

| Scenario | Use **Result Pattern** | Use **Exception** |
| :--- | :---: | :---: |
| Invalid password, user not found, expired credit card | âœ… **YES** | âŒ NO |
| Order quantity exceeds stock in warehouse | âœ… **YES** | âŒ NO |
| SQL Server database connection timed out / server down | âŒ NO | âœ… **YES** |
| Out of Memory / Disk I/O Failure | âŒ NO | âœ… **YES** |
| Code bug: Null reference where it should never be null | âŒ NO | âœ… **YES** |

#### Popular Production .NET Libraries:
* **`Ardalis.Result`** (Steve Smith - widely used in Clean Architecture)
* **`ErrorOr`** (Amichai Mantinband - fluent, modern, popular)
* **`FluentResults`** (Great for accumulating multiple validation errors)


---

## .NET Pattern 5: The Specification Pattern

### 1. What is the Problem? (The Real-World Pain)

#### Pain Point A: The "Fat Repository" Explosion
Consider a typical Entity Framework Core `CustomerRepository`. Over years of feature requests, your repository interface turns into an unmaintainable monster:

```csharp
// âŒ The Repository Bloat Anti-Pattern (Violates Open/Closed Principle)
public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(Guid id);
    Task<List<Customer>> GetActiveCustomersAsync();
    Task<List<Customer>> GetActiveCustomersWithOrdersAsync();
    Task<List<Customer>> GetCustomersByRegionAsync(string region);
    Task<List<Customer>> GetVipCustomersWithPendingInvoicesAsync(int minOrders, decimal minSpend);
    Task<List<Customer>> GetPagedActiveCustomersOrderedByNameAsync(int page, int pageSize);
    // ... dozens more ad-hoc query methods!
}
```
Every time product owners request a new combination of filters, you must edit both the `ICustomerRepository` interface and its concrete implementation, creating massive merge conflicts and breaking the Open/Closed Principle.

#### Pain Point B: Leaking `IQueryable<T>` Everywhere
To avoid repository bloat, developers often return `IQueryable<T>` directly from repositories into controllers or services:
```csharp
// âŒ Leaking Data Access into Business / API Layer
var customers = await _customerRepo.GetAll()
    .Where(c => c.IsActive && c.Orders.Any(o => o.Status == OrderStatus.Completed))
    .Include(c => c.Orders)
    .ToListAsync();
```
**Why this hurts:**
1. **Scattered Business Rules:** What constitutes a "Qualified Customer"? If multiple developers write that `.Where(...)` clause across multiple handlers, the logic inevitably diverges when business requirements evolve.
2. **Untestable Code:** Mocking `IQueryable` requires complex expression-tree mocking or setting up an in-memory database.
3. **Lazy Loading & N+1 Performance Traps:** Queries execute outside data layers without clear execution boundaries.

#### Pain Point C: In-Memory Validation vs. Database Query Mismatch
If a customer applies for a VIP promotion, you have to verify: *"Is this customer eligible?"*  
Without the Specification pattern, you must write the same business logic **twice**:
- Once in LINQ for SQL translation: `db.Customers.Where(c => c.Orders.Count > 10 && c.TotalSpent > 1000)`
- Once in C# domain memory: `if (customer.Orders.Count > 10 && customer.TotalSpent > 1000)`

---

### 2. The Core Concept (Plain English)

> **"A Specification encapsulates a domain rule, query condition, or criteria into a standalone, reusable object."**  
> *(Eric Evans & Martin Fowler - Domain-Driven Design)*

A **Specification** solves both sides with a single piece of code:
1. **For Database Queries (EF Core):** Translates your business criteria, eager loading (`.Include()`), sorting, and pagination into clean SQL expression trees.
2. **For In-Memory Domain Objects:** Validates candidate entities directly in RAM (`spec.IsSatisfiedBy(customer)`).

---

### 3. Implementation in Modern C# (.NET 8/9)

#### Step 1: The Specification Interface
```csharp
using System.Linq.Expressions;

public interface ISpecification<T>
{
    // The filter expression (translated to SQL WHERE)
    Expression<Func<T, bool>>? Criteria { get; }

    // Eager loading (translated to SQL JOIN / INCLUDE)
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }

    // Ordering
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }

    // Pagination
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }

    // In-memory evaluation
    bool IsSatisfiedBy(T entity);
}
```

#### Step 2: The Base Specification Class
```csharp
using System.Linq.Expressions;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification() { }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression) 
        => Includes.Add(includeExpression);

    protected void AddInclude(string includeString) 
        => IncludeStrings.Add(includeString);

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) 
        => OrderBy = orderByExpression;

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression) 
        => OrderByDescending = orderByDescExpression;

    // Evaluates in-memory by compiling the expression tree into an executable delegate
    public virtual bool IsSatisfiedBy(T entity)
    {
        if (Criteria == null) return true;
        Func<T, bool> predicate = Criteria.Compile();
        return predicate(entity);
    }
}
```

#### Step 3: The Specification Evaluator (The EF Core Bridge)
This component takes any raw `IQueryable<T>` and dynamically applies the specification's filters, joins, sorting, and paging:

```csharp
using Microsoft.EntityFrameworkCore;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> spec) where T : class
    {
        var query = inputQuery;

        // 1. Apply Filter (WHERE)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // 2. Apply Includes (JOINs)
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        // 3. Apply Ordering
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // 4. Apply Paging (SKIP / TAKE)
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}
```

#### Step 4: Generic Repository Using Specifications
The generic repository interface remains clean, concise, and never requires changes when adding new queries:

```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default);
    Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default);
}

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;

    public EfRepository(DbContext context) => _context = context;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) 
        => await _context.Set<T>().FindAsync(new object[] { id }, ct);

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), spec);
        return await query.AsNoTracking().ToListAsync(ct);
    }

    public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), spec);
        return await query.CountAsync(ct);
    }
}
```

---

### 4. Defining Concrete Domain Specifications

New query requirements are satisfied simply by adding a new focused class:

```csharp
// 1. Business query encapsulating filter + join + sorting:
public class HighValueActiveCustomersSpec : BaseSpecification<Customer>
{
    public HighValueActiveCustomersSpec(decimal minimumSpendThreshold) 
        : base(c => c.IsActive && c.Orders.Any(o => o.TotalAmount >= minimumSpendThreshold))
    {
        AddInclude(c => c.Orders);
        ApplyOrderByDescending(c => c.Orders.Sum(o => o.TotalAmount));
    }
}

// 2. Pagination query:
public class PagedOrdersByCustomerSpec : BaseSpecification<Order>
{
    public PagedOrdersByCustomerSpec(Guid customerId, int pageIndex, int pageSize)
        : base(o => o.CustomerId == customerId)
    {
        AddInclude(o => o.OrderItems);
        ApplyOrderByDescending(o => o.OrderDate);
        ApplyPaging(skip: pageIndex * pageSize, take: pageSize);
    }
}
```

---

### 5. Consuming Specifications in Application Services

```csharp
public class CustomerService
{
    private readonly IRepository<Customer> _customerRepo;

    public CustomerService(IRepository<Customer> customerRepo) 
        => _customerRepo = customerRepo;

    public async Task<IReadOnlyList<CustomerDto>> GetVipCustomersAsync()
    {
        // Zero changes to the repository!
        var spec = new HighValueActiveCustomersSpec(minimumSpendThreshold: 1000m);
        var customers = await _customerRepo.ListAsync(spec);

        return customers.Select(c => new CustomerDto(c.Id, c.Name)).ToList();
    }

    public bool CheckPromoEligibility(Customer customer)
    {
        // Reusing the exact same specification for in-memory domain validation:
        var spec = new HighValueActiveCustomersSpec(minimumSpendThreshold: 1000m);
        return spec.IsSatisfiedBy(customer);
    }
}
```

---

### 6. Summary Comparison: Plain LINQ vs. Specification Pattern

| Feature | Raw LINQ / Fat Repository | Specification Pattern |
| :--- | :--- | :--- |
| **Repository Size** | Bloated with dozens of ad-hoc methods (`GetBy...`). | Small, fixed interface (`ListAsync(spec)`). |
| **Open/Closed Principle** | âŒ Violates OCP every time a new query is required. | âœ… Adding a query is just creating a new class. |
| **Testability** | Hard to mock complex expression trees. | Trivial to unit test with plain in-memory objects. |
| **Reusability** | Query logic duplicated across handlers. | One specification used in DB queries, domain validation, and tests. |
| **Industry Standard NuGet** | None (Home-grown ad-hoc code). | **`Ardalis.Specification`** (by Steve "Ardalis" Smith). |

---

## .NET Pattern 6: Resilience Patterns (Circuit Breaker & Retry)

### 1. What is the Problem? (The Real-World Pain)
In cloud environments, microservices, and third-party integrations, network connectivity is inherently unreliable. Servers restart, DNS entries blip, and third-party APIs (Stripe, Twilio, SendGrid) experience transient throttling.

* **The Naive Approach (Fail Immediately):** A 50ms transient network timeout triggers a 500 internal server error, throwing away a customer's cart and losing revenue.
* **The Blind While-Loop Approach (Hammering):** A developer writes a while-loop retrying 10 times immediately. If the downstream service has an actual outage, 5,000 concurrent users will bombard the dying service with **50,000 requests/second**, triggering a catastrophic **cascading outage (Thundering Herd Problem)**.

---

### 2. The Core Concept (Plain English)

Resilience is structured as a two-stage defensive perimeter:

```
[ Incoming Request ]
         â”‚
         â–¼
 â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”        Trip if error rate > 50%
 â”‚Circuit Breakerâ”‚ â”€â”€> [ OPEN / Short-Circuit ] â”€â”€> Fast Fail (Zero network calls)
 â””â”€â”€â”€â”€â”€â”€â”€â”¬â”€â”€â”€â”€â”€â”€â”€â”˜
         â”‚ (CLOSED - Normal operation)
         â–¼
 â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
 â”‚     Retry     â”‚ â”€â”€> Exponential Backoff + Jitter (Wait 2s, 4s, 8s + random ms)
 â””â”€â”€â”€â”€â”€â”€â”€â”¬â”€â”€â”€â”€â”€â”€â”€â”˜
         â”‚
         â–¼
 [ Downstream API ]
```

#### A. Exponential Backoff with Jitter
* **Exponential Backoff:** If attempt 1 fails, wait $2^1 = 2$ seconds. If attempt 2 fails, wait $2^2 = 4$ seconds, then 8 seconds.
* **Jitter:** Adds randomized milliseconds ($2.14\text{s}$, $4.08\text{s}$) so hundreds of concurrent retries do not strike the target at the exact same microsecond.

#### B. The Circuit Breaker (Electrical Fuse Analogy)
Just like a household circuit breaker trips to prevent electrical fires:
* **CLOSED (Healthy):** Normal traffic flows through. Successes keep the circuit closed.
* **OPEN (Tripped):** If the failure rate crosses a threshold (e.g., 50% errors over 10 seconds), the circuit **trips open**. For the next 30 seconds, all outgoing calls fail fast immediately without touching the network, allowing the downstream service to recover.
* **HALF-OPEN (Trial):** After the duration expires, the breaker lets a single probe request through. If it succeeds, it resets to **CLOSED**; if it fails, it trips back to **OPEN**.

---

### 3. Implementation in Modern C# (.NET 8/9 with `Microsoft.Extensions.Resilience`)

In .NET 8/9, Microsoft integrated Polly v8 natively into the runtime.

#### Production Configuration in `Program.cs`:
```csharp
builder.Services.AddHttpClient<IPaymentService, StripePaymentService>(client =>
{
    client.BaseAddress = new Uri("https://api.stripe.com/v1/");
    client.Timeout = TimeSpan.FromSeconds(5);
})
// Native .NET 8/9 Resilience Pipeline (Rate Limiter -> Total Timeout -> Retry -> Circuit Breaker):
.AddStandardResilienceHandler(options =>
{
    // 1. Retry Strategy
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.UseJitter = true;
    options.Retry.Delay = TimeSpan.FromSeconds(1);

    // 2. Circuit Breaker Strategy
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(10);
    options.CircuitBreaker.FailureRatio = 0.5; // Trip if >= 50% fail
    options.CircuitBreaker.MinimumThroughput = 8; // Minimum sample size
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30); // Cool-off time
});
```

---

## .NET Pattern 7: The Transactional Outbox Pattern

### 1. What is the Problem? (The Real-World Pain)
In event-driven architectures and microservices, systems frequently suffer from the **Dual-Write Problem**:

```csharp
// âŒ The Dual-Write Bug
public async Task PlaceOrder(Order order)
{
    // Step 1: Save to relational database
    await _dbContext.Orders.AddAsync(order);
    await _dbContext.SaveChangesAsync(); // <-- Succeeds!

    // Step 2: Publish event to Kafka / RabbitMQ / Azure Service Bus
    // ðŸ’¥ WHAT IF: Broker is restarting, network drops, or the server crashes right here?
    await _messageBus.PublishAsync(new OrderPlacedEvent(order.Id));
}
```
* If Step 1 succeeds and Step 2 fails: The order exists in the DB, but inventory is never reserved, billing is never notified, and the warehouse never ships the order.
* If you reverse the operations (publish first, then save to DB): A database rollback creates **phantom events**, causing external services to charge money for an order that was never saved!
* Distributed 2-Phase Commit (2PC / MSDTC) is not supported across modern cloud databases and message queues.

---

### 2. The Core Concept (Plain English)

> **"If you cannot atomically write to two different systems, write both actions to ONE system inside a single ACID database transaction."**

Instead of publishing directly to the message broker during the HTTP request:
1. Save the `Order` **AND** save an `OutboxMessage` record inside the **same database transaction**.
2. A background worker continuously reads pending messages from `OutboxMessages` and publishes them to the broker with guaranteed **at-least-once delivery**.

```
[ HTTP Request ]
       â”‚
       â–¼
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚       SINGLE ATOMIC SQL DB TRANSACTION       â”‚
â”‚  1. INSERT INTO Orders (...)                 â”‚
â”‚  2. INSERT INTO OutboxMessages (...)         â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
       â”‚ (100% Guaranteed Commit or Rollback)
       â–¼
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚  Background Worker (IHostedService / Quartz) â”‚
â”‚  - Polls unprocessed OutboxMessages          â”‚
â”‚  - Publishes to RabbitMQ / Kafka / Bus       â”‚
â”‚  - Sets ProcessedOnUtc = DateTime.UtcNow     â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

---

### 3. Implementation in Modern C# (.NET 8/9)

#### Step 1: The Outbox Message Entity
```csharp
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Serialized JSON payload
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}
```

#### Step 2: Atomic Persistence in the Command Handler
```csharp
public async Task Handle(CreateOrderCommand cmd, CancellationToken ct)
{
    var order = new Order(Guid.NewGuid(), cmd.CustomerId, cmd.TotalAmount);
    var domainEvent = new OrderCreatedEvent(order.Id, order.TotalAmount);

    var outboxMessage = new OutboxMessage
    {
        Id = Guid.NewGuid(),
        EventType = typeof(OrderCreatedEvent).FullName!,
        Content = JsonSerializer.Serialize(domainEvent),
        CreatedOnUtc = DateTime.UtcNow
    };

    // Both entities written in the EXACT same DB transaction!
    await _dbContext.Orders.AddAsync(order, ct);
    await _dbContext.OutboxMessages.AddAsync(outboxMessage, ct);
    await _dbContext.SaveChangesAsync(ct); 
}
```

#### Step 3: The Background Publisher Service
```csharp
public class OutboxPublisherService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherService> _logger;

    public OutboxPublisherService(IServiceScopeFactory scopeFactory, ILogger<OutboxPublisherService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            // 1. Fetch pending batch
            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.CreatedOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    // 2. Publish to Broker
                    await messageBus.PublishAsync(message.EventType, message.Content, stoppingToken);
                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish Outbox message {Id}", message.Id);
                    message.Error = ex.Message;
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }
}
```

> [!IMPORTANT]
> **Production Gotcha: Idempotent Consumers**  
> The Outbox pattern guarantees **At-Least-Once Delivery**, NOT *Exactly-Once Delivery*. If the worker crashes immediately after publishing to RabbitMQ but before marking `ProcessedOnUtc`, the message will be republished upon restart. Subscribers must be built to be **idempotent** (using deduplication IDs in Redis or database unique constraints).

---

## .NET Pattern 8: CQRS (Command Query Responsibility Segregation)

> ðŸ’¡ **Architectural Note:** MediatR is commonly used to route Commands and Queries in CQRS. For an architectural deep dive on distinguishing CQRS from pure Mediator, Command, and Observer patterns, see [Deep Dive: MediatR Multi-Pattern Architecture](#deep-dive-mediatr-multi-pattern-architecture-mediator-vs-observer-vs-cqrs-vs-command-vs-chain-of-responsibility).

### 1. What is the Problem? (The Real-World Pain)
In traditional CRUD applications, a single model and ORM mapping are used for both reading and writing:

```csharp
// Traditional Monolithic Model:
public class Order
{
    public Guid Id { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; }
    public List<Payment> Payments { get; set; }
    public ShippingDetails Shipping { get; set; }

    public void AddItem(Product p) { /* 20 lines of domain invariant rules */ }
}
```
**Why this fails at enterprise scale:**
1. **Conflicting Architectural Forces:**
   - **Writes (5% of traffic):** Require strict validation, transactional boundaries, normalization, and concurrency checks.
   - **Reads (95% of traffic):** Require wide denormalized joins, pagination, full-text search, and zero change-tracking overhead.
2. **Performance Drag:** Loading an EF Core aggregate with 6 `.Include()` joins just to display a list on a web page wastes memory and generates slow SQL.

---

### 2. The Core Concept (Plain English)

> **"Asking a question should not change the answer."** *(Bertrand Meyer)*

CQRS splits the application into two completely independent architectural pipelines:

```
                      â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
                      â”‚                 USER ACTION                  â”‚
                      â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”¬â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
                                             â”‚
                    â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”´â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
                    â–¼                                                 â–¼
          [ COMMAND (Write) ]                                 [ QUERY (Read) ]
    Does an action / Alters State                      Fetches data / Zero side-effects
                    â”‚                                                 â”‚
                    â–¼                                                 â–¼
        Rich Domain Aggregate                             Flat DTO Projection
          (Entity Framework)                                (Dapper / Raw SQL)
                    â”‚                                                 â”‚
                    â–¼                                                 â–¼
        [ Transactional DB ]                               [ High-Speed Read Replica ]
```

---

### 3. Implementation in Modern C# (.NET 8/9 with MediatR)

#### The Write Side (Command): Encapsulated, Validated, Transactional
```csharp
// 1. Command
public record PlaceOrderCommand(Guid CustomerId, List<CartItemDto> Items) : IRequest<Result<Guid>>;

// 2. Command Handler (Enforces Domain Logic via EF Core)
public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<Guid>>
{
    private readonly AppDbContext _db;
    public PlaceOrderCommandHandler(AppDbContext db) => _db = db;

    public async Task<Result<Guid>> Handle(PlaceOrderCommand cmd, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync(new object[] { cmd.CustomerId }, ct);
        if (customer == null) return Error.NotFound("Customer.Missing", "Customer not found.");

        var order = Order.Create(customer);
        foreach (var item in cmd.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        await _db.Orders.AddAsync(order, ct);
        await _db.SaveChangesAsync(ct);

        return order.Id;
    }
}
```

#### The Read Side (Query): Bypasses EF Core; Direct SQL with Dapper
```csharp
// 1. Query
public record GetCustomerOrdersSummaryQuery(Guid CustomerId) : IRequest<IReadOnlyList<OrderSummaryDto>>;

// 2. Flat Read-Optimized DTO
public record OrderSummaryDto(Guid OrderId, DateTime OrderDate, decimal TotalAmount, string Status, int ItemCount);

// 3. Query Handler (Zero tracking, direct SQL with Dapper)
public class GetCustomerOrdersSummaryHandler : IRequestHandler<GetCustomerOrdersSummaryQuery, IReadOnlyList<OrderSummaryDto>>
{
    private readonly IDbConnectionFactory _connectionFactory;
    public GetCustomerOrdersSummaryHandler(IDbConnectionFactory connectionFactory) 
        => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<OrderSummaryDto>> Handle(GetCustomerOrdersSummaryQuery query, CancellationToken ct)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = """
            SELECT 
                o.Id AS OrderId, 
                o.OrderDate, 
                o.TotalAmount, 
                o.Status, 
                COUNT(i.Id) AS ItemCount
            FROM Orders o
            LEFT JOIN OrderItems i ON o.Id = i.OrderId
            WHERE o.CustomerId = @CustomerId
            GROUP BY o.Id, o.OrderDate, o.TotalAmount, o.Status
            ORDER BY o.OrderDate DESC
            """;

        var summaries = await connection.QueryAsync<OrderSummaryDto>(sql, new { query.CustomerId });
        return summaries.ToList();
    }
}
```

---

# 6. Tier 2: High-Impact Enterprise & Distributed Architecture Patterns

> **Architectural Level:** Senior / Staff Software Architect  
> **Target Scope:** Distributed Systems, Microservices, Cloud Resilience, and Enterprise Modernization.

While Tier 1 patterns form the daily bread-and-butter of clean in-process application design, **Tier 2 patterns govern how modern systems behave across networks, databases, and architectural boundaries**. You won't use these in every local CRUD controller, but they are the non-negotiable tools required when scaling out to microservices, asynchronous messaging, and legacy migrations.

```
                       ┌─────────────────────────────────────────────────────────┐
                       │               TIER 2: "GOOD TO KNOW"                    │
                       │     High-Impact Architecture & Senior Interview Tools   │
                       └────────────────────────────┬────────────────────────────┘
                                                    │
         ┌──────────────────┬───────────────────────┼──────────────────────┬──────────────────┐
         ▼                  ▼                       ▼                      ▼                  ▼
   1. SAGA PATTERN    2. IDEMPOTENCY        3. TEMPLATE METHOD          4. ACL         5. STRANGLER FIG
 (Distributed Tx)   (Safe Retries)         (Algorithm Skeleton)     (Clean Boundary)   (Legacy Rewrite)
```

---

## Tier 2 Pattern 1: The Saga Pattern (Distributed Transactions)

### 1. What is the Problem? (The Death of ACID across Networks)

In a monolithic architecture, data consistency is trivial. You open a database transaction and execute operations atomically:
```sql
BEGIN TRANSACTION;
  INSERT INTO Orders ...
  UPDATE Accounts SET Balance = Balance - 100 ...
  UPDATE Inventory SET Stock = Stock - 1 ...
COMMIT TRANSACTION; -- If anything fails, rollback everything automatically!
```

In a distributed microservices environment, **each microservice owns its private database**:
1. **Order Service:** Writes to an `OrdersDb` (PostgreSQL).
2. **Payment Service:** Calls an external Stripe API and updates `PaymentsDb` (SQL Server).
3. **Inventory Service:** Writes to `WarehouseDb` (MongoDB).

```
[Order Service] ──► [Payment Service] ──► [Inventory Service]
   (OrdersDb)          (PaymentsDb)             (WarehouseDb)
```

#### The Distributed Disaster:
What happens if Step 1 succeeds, Step 2 charges the customer's credit card, but **Step 3 crashes** because the warehouse is out of stock?
* You cannot run a SQL `ROLLBACK` across 3 isolated databases on different servers.
* Traditional Two-Phase Commit (2PC / MSDTC) protocols are slow, fragile, hold distributed locks, and fail in modern cloud environments.
* If you do nothing, the customer has paid money for an item that will never arrive.

---

### 2. The Core Concept (Plain English)

A **Saga** is an architectural pattern that coordinates a sequence of independent **local transactions**. 
Instead of relying on database-level locking, each step in a Saga updates data within a single service and publishes an event or message. 

If any intermediate step fails, the Saga coordinates a series of **Compensating Transactions** that run backward to undo the changes made by preceding steps.

```
HAPPY PATH:
[Create Order] ────────► [Charge Card] ────────► [Reserve Stock] ────────► (Complete)

FAILURE PATH (Stock Reservation Fails):
[Create Order] ────────► [Charge Card] ────────► [Reserve Stock Fails! ❌]
      ▲                        │
      │ Compensate             │ Compensate
      │                        ▼
[Cancel Order] ◄──────── [Refund Card]
```

> ⚠️ **Key Rule of Compensation:** A compensating transaction does *not* magically revert the database like `ROLLBACK`. It applies a **new business action that logically cancels the previous action** (e.g., if step 2 charged $100, the compensating transaction is *issuing a $100 refund*).

---

### 3. The Two Saga Topologies

#### A. Choreography (Event-Driven / Decentralized)
Services react autonomously to domain events published to a message broker (RabbitMQ/Kafka). There is no central boss.
* `OrderService` emits `OrderCreated`.
* `PaymentService` listens, charges card, emits `PaymentCompleted`.
* `InventoryService` listens, fails to reserve stock, emits `StockReservationFailed`.
* `PaymentService` listens to `StockReservationFailed`, refunds the card, emits `PaymentRefunded`.
* `OrderService` listens, marks order as `Cancelled`.

* **When to use:** Small workflows (2–3 services).
* **Downside:** As workflows grow (6+ services), choreography becomes a tangled "pinball machine" where tracking the overall state of an order is nearly impossible.

#### B. Orchestration (Centralized State Machine Coordinator)
A dedicated orchestrator tells each participant what operation to perform and tracks the workflow state in a persistent database.
* The Orchestrator commands `PaymentService`: *"Charge payment for Order 42"*.
* It awaits the result. Upon success, it commands `InventoryService`: *"Reserve stock"*.
* If `InventoryService` replies with failure, the Orchestrator commands `PaymentService`: *"Refund payment for Order 42"*.

* **When to use:** Enterprise workflows with 3+ services, complex branching, timeouts, or auditing requirements.

---

### 4. Implementation in Modern .NET (MassTransit State Machine)

In production .NET, architects use **MassTransit Saga State Machines** (`MassTransit.Automatonymous`):

```csharp
// 1. The Persistent Saga State (Saved to database via EF Core)
public class OrderSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; } // Unique Order ID
    public string CurrentState { get; set; } = string.Empty;
    public decimal OrderTotal { get; set; }
    public Guid? PaymentId { get; set; }
}

// 2. The Orchestrator State Machine Engine
public class OrderStateMachine : MassTransitStateMachine<OrderSagaData>
{
    // Define States
    public State Submitted { get; private set; } = null!;
    public State Paid { get; private set; } = null!;
    public State Cancelled { get; private set; } = null!;

    // Define Events
    public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; } = null!;
    public Event<PaymentCompletedEvent> PaymentCompleted { get; private set; } = null!;
    public Event<StockReservationFailedEvent> StockFailed { get; private set; } = null!;

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        // Correlate incoming events by OrderId
        Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockFailed, x => x.CorrelateById(m => m.Message.OrderId));

        // Define Flow: Initial -> Submitted
        Initially(
            When(OrderSubmitted)
                .Then(ctx => ctx.Saga.OrderTotal = ctx.Message.Total)
                .Publish(ctx => new ProcessPaymentCommand(ctx.Saga.CorrelationId, ctx.Saga.OrderTotal))
                .TransitionTo(Submitted)
        );

        // When Paid -> Try to Reserve Stock
        During(Submitted,
            When(PaymentCompleted)
                .Then(ctx => ctx.Saga.PaymentId = ctx.Message.PaymentId)
                .Publish(ctx => new ReserveStockCommand(ctx.Saga.CorrelationId))
                .TransitionTo(Paid),

            // Compensate if stock fails!
            When(StockFailed)
                .Publish(ctx => new RefundPaymentCommand(ctx.Saga.CorrelationId, ctx.Saga.PaymentId!.Value))
                .Publish(ctx => new CancelOrderCommand(ctx.Saga.CorrelationId))
                .TransitionTo(Cancelled)
        );
    }
}
```

---

## Tier 2 Pattern 2: The Idempotency Pattern (Idempotent Consumer & API Key)

### 1. What is the Problem? (The Accidental Double-Charge)

Networks are inherently unreliable. Consider a standard mobile checkout:
1. A customer clicks **"Pay $100"**.
2. The server charges their credit card and successfully inserts the order.
3. As the server sends HTTP `200 OK` back to the mobile phone, the user drives into an elevator—the cellular connection drops.
4. The mobile app receives an HTTP Timeout error.
5. Built-in network retry policies kick in and resend the identical HTTP request 3 seconds later.
6. **Catastrophe:** Without idempotency, the server charges the customer **another $100**.

In asynchronous messaging (RabbitMQ, Kafka, Azure Service Bus), **At-Least-Once Delivery** guarantees that network rebalances, worker crashes, or unacknowledged messages will occasionally cause the exact same message to be delivered multiple times.

---

### 2. The Core Concept (Plain English)

An operation is **Idempotent** if applying it once has the exact same effect as applying it $N$ times:
$$\text{Operation}(\text{Operation}(x)) = \text{Operation}(x)$$

* HTTP `GET`, `PUT`, and `DELETE` are semantically idempotent by standard design.
* HTTP `POST` is **NOT** idempotent by default (each POST creates a new resource or triggers a new mutation).

---

### 3. Modern .NET Implementation (Idempotency Key via Distributed Cache)

The standard enterprise solution passes an **`Idempotency-Key`** HTTP header (a client-generated UUID).

```csharp
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly IPaymentProcessor _paymentProcessor;

    public PaymentsController(IDistributedCache cache, IPaymentProcessor paymentProcessor)
    {
        _cache = cache;
        _paymentProcessor = paymentProcessor;
    }

    [HttpPost("charge")]
    public async Task<IActionResult> ChargePayment(
        [FromHeader(Name = "X-Idempotency-Key")] Guid idempotencyKey,
        [FromBody] ChargePaymentRequest request)
    {
        string cacheKey = $"idempotency:payment:{idempotencyKey}";

        // 1. Check if we already processed this unique request:
        var cachedResponse = await _cache.GetStringAsync(cacheKey);
        if (cachedResponse != null)
        {
            // Return the cached result immediately! No second charge!
            var previousResult = JsonSerializer.Deserialize<PaymentResult>(cachedResponse);
            return Ok(previousResult);
        }

        // 2. Acquire a distributed lock (e.g., via Redis / RedLock) to prevent race conditions
        // ... (Execute payment gateway call) ...
        var paymentResult = await _paymentProcessor.ExecuteChargeAsync(request.Amount, request.CardToken);

        // 3. Cache the successful result with a 24-hour expiration window:
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(paymentResult), cacheOptions);

        return Ok(paymentResult);
    }
}
```

---

## Tier 2 Pattern 3: Template Method Pattern (GoF Behavioral)

### 1. What is the Problem? (The Duplicate Pipeline Anti-Pattern)

Imagine you are building a data ingestion engine that processes financial statements in three formats: **CSV, JSON, and Excel**.
Every format must follow the **exact same 6-step lifecycle**:
1. Authenticate with cloud source.
2. Download data stream.
3. **Parse raw bytes into record models.** *(Different for CSV vs. JSON vs. Excel!)*
4. Validate account numbers and currency codes.
5. Save records into SQL Server.
6. Publish notification to Slack.

If you write 3 separate classes (`CsvProcessor`, `JsonProcessor`, `ExcelProcessor`) without an overarching template, the orchestration logic (steps 1, 2, 4, 5, 6) is duplicated 3 times. If step 4 (validation) changes, you must remember to update all 3 classes.

---

### 2. The Core Concept (Plain English)

The **Template Method Pattern** defines the **skeleton of an algorithm** in an abstract base class, but defers specific implementation steps to subclasses. Subclasses can override individual steps without changing the algorithm's overarching structure.

```csharp
// 1. The Abstract Base Engine
public abstract class DataImportPipeline
{
    // The Template Method: Sealed so subclasses cannot alter the execution sequence!
    public async Task ExecutePipelineAsync(string sourceUri)
    {
        Authenticate();
        var rawData = await DownloadStreamAsync(sourceUri);
        
        // Abstract hook: Subclasses provide their unique parsing logic
        var records = ParseRecords(rawData);

        ValidateRecords(records);
        await PersistToDatabaseAsync(records);
        
        // Optional hook method (has default implementation, can be overridden)
        OnPipelineCompleted(records.Count);
    }

    private void Authenticate() => Console.WriteLine("[AUTH] Verified cloud token.");
    private async Task<string> DownloadStreamAsync(string uri) 
    {
        Console.WriteLine($"[DOWNLOAD] Fetching stream from {uri}");
        return await Task.FromResult("dummy raw stream");
    }
    
    // Abstract step: MUST be implemented by subclasses
    protected abstract List<string> ParseRecords(string rawData);

    private void ValidateRecords(List<string> records) 
        => Console.WriteLine($"[VALIDATE] Validating {records.Count} records against schema.");

    private async Task PersistToDatabaseAsync(List<string> records)
        => await Task.Delay(10); // Batch SQL write

    // Virtual hook: CAN be overridden by subclasses if needed
    protected virtual void OnPipelineCompleted(int totalRecords)
        => Console.WriteLine($"[NOTIFY] Import successfully completed for {totalRecords} records.");
}

// 2. Concrete Subclass: Only handles its unique format
public class CsvDataImportPipeline : DataImportPipeline
{
    protected override List<string> ParseRecords(string rawData)
    {
        Console.WriteLine("[PARSE] Splitting comma-separated values into domain models.");
        return rawData.Split('\n').ToList();
    }
}
```

#### Everyday .NET Framework Examples:
* **`Microsoft.Extensions.Hosting.BackgroundService`**: Has a template method `StartAsync()` that manages cancellation tokens and calls your protected abstract `ExecuteAsync(CancellationToken stoppingToken)`.
* **ASP.NET Core `AuthenticationHandler<TOptions>`**: Implements the security authentication pipeline and calls your protected abstract `HandleAuthenticateAsync()`.

---

## Tier 2 Pattern 4: Anti-Corruption Layer (ACL - Domain-Driven Design)

### 1. What is the Problem? (Pollution by Legacy Schemas)

You are building a clean, modern .NET 9 greenfield project using strict Domain-Driven Design (DDD). Your core domain contains pristine entities:
```csharp
public record Customer(CustomerId Id, EmailAddress Email, CustomerStatus Status);
```

However, your application must integrate with an external 25-year-old SAP Mainframe SOAP API where:
* Variable names are cryptic German abbreviations (`KUNNR` for Customer Number, `NAME1` for Full Name).
* Status codes are unreadable integers (`1 = Active`, `4 = Soft Deleted`, `9 = Bankrupt`).
* Error messages arrive as nested XML inside HTTP 200 responses.

If your core domain services directly reference the SOAP client DLLs or models, **legacy rot bleeds directly into your clean architecture**.

---

### 2. The Core Concept (Plain English)

An **Anti-Corruption Layer (ACL)** acts as a strict diplomatic border checkpoint. It sits between two different subsystems (often a clean domain and a legacy or external third-party API) and **translates between the two domain models**, ensuring the internal domain remains untainted.

```
┌──────────────────────────────────────┐                     ┌────────────────────────────────────────┐
│     CLEAN DOMAIN (Modern .NET 9)     │                     │     LEGACY SYSTEM / 3RD-PARTY SOAP     │
│                                      │                     │                                        │
│  public record Customer(             │     ┌─────────┐     │  <CUSTOMER_REC>                        │
│      CustomerId Id,                  │ ◄───┤   ACL   ├──── │    <KUNNR>102938</KUNNR>               │
│      string Email,                   │     │ ADAPTER │     │    <STAT_CD>4</STAT_CD>                │
│      CustomerStatus Status)          │     └─────────┘     │  </CUSTOMER_REC>                       │
└──────────────────────────────────────┘                     └────────────────────────────────────────┘
                                           Translates Legacy
                                           into Pure Domain
```

#### Implementation in Modern C#:
```csharp
// 1. The Clean Domain Contract (Lives inside Domain Core Layer)
public interface ICustomerDirectory
{
    Task<Customer?> FindCustomerAsync(CustomerId id);
}

// 2. The Anti-Corruption Layer (Lives inside Infrastructure Layer)
public class SapAntiCorruptionLayer : ICustomerDirectory
{
    private readonly LegacySapSoapClient _sapClient; // The dirty external client

    public SapAntiCorruptionLayer(LegacySapSoapClient sapClient) => _sapClient = sapClient;

    public async Task<Customer?> FindCustomerAsync(CustomerId id)
    {
        // A. Call the external legacy service
        SapCustomerRecord? rawRecord = await _sapClient.FetchKUNNRAsync(id.Value);
        if (rawRecord == null) return null;

        // B. Translate & Sanitize: Legacy schema -> Clean Domain Aggregate
        return new Customer(
            Id: new CustomerId(rawRecord.KUNNR),
            Email: rawRecord.EMAIL_ADDR,
            Status: TranslateSapStatus(rawRecord.STAT_CD)
        );
    }

    private static CustomerStatus TranslateSapStatus(int statCd) => statCd switch
    {
        1 => CustomerStatus.Active,
        4 => CustomerStatus.Suspended,
        9 => CustomerStatus.Terminated,
        _ => CustomerStatus.Unknown
    };
}
```

---

## Tier 2 Pattern 5: The Strangler Fig Pattern (Legacy Modernization)

### 1. What is the Problem? (The Fatal "Big Bang" Rewrite)

Your organization runs a 15-year-old monolithic enterprise application written in .NET Framework 4.8 / ASP.NET MVC with 800,000 lines of code. Management wants to modernize to .NET 9 running on Linux Docker containers in Kubernetes.

The traditional approach is the **"Big Bang Rewrite"**:
* Freeze feature development or maintain two parallel versions for 2 years.
* Build the new system from scratch.
* Cut over all traffic on a fateful Sunday night.

**Why this fails 90% of the time:**
* Business requirements change over 2 years, turning the new system obsolete before launch.
* Countless undocumented edge cases in the old system are forgotten, causing catastrophic downtime.
* Cost and timeline overruns cause management to cancel the project halfway through.

---

### 2. The Core Concept (Plain English)

Named after the Australian rainforest *Strangler Fig* tree. A strangler fig seed germinates in the high canopy of an old host tree. Over decades, it grows roots down around the trunk into the soil, slowly enveloping the host tree until the host tree rots away, leaving a hollow, majestic new tree in its place.

In software architecture, you place an **API Gateway or Reverse Proxy (like Microsoft YARP)** in front of the legacy monolith:
1. **Day 1:** 100% of traffic passes straight through YARP to the old monolith.
2. **Month 1:** You extract *one single endpoint* (e.g., `/api/orders`) and write it as a high-performance .NET 9 microservice.
3. You configure YARP to route `/api/orders` to the new service, while all other routes continue hitting the monolith.
4. **Month 12:** You incrementally migrate features one-by-one.
5. **Final Day:** When zero routes point to the old monolith, you simply turn off the old Windows server.

```
                          Incoming HTTP Traffic
                                   │
                                   ▼
                   ┌───────────────────────────────┐
                   │     YARP (REVERSE PROXY)      │
                   └───────┬───────────────┬───────┘
                           │               │
     Route: /api/orders/*  │               │ Fallback: All other routes (/**)
                           ▼               ▼
                 ┌──────────────────┐   ┌───────────────────────┐
                 │  NEW MICROSERVICE│   │    OLD MONOLITH       │
                 │     (.NET 9)     │   │  (.NET Framework 4.8) │
                 └──────────────────┘   └───────────────────────┘
```

---

### 3. Implementation with Microsoft YARP (`appsettings.json`)

Microsoft provides **YARP (*Yet Another Reverse Proxy*)**, a toolkit built directly into ASP.NET Core for Strangler Fig migrations.

#### `Program.cs`:
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.MapReverseProxy();
app.Run();
```

#### `appsettings.json` Configuration:
```json
{
  "ReverseProxy": {
    "Routes": {
      "orders-migrated-route": {
        "ClusterId": "modernDotNet9Cluster",
        "Match": {
          "Path": "/api/orders/{**catch-all}"
        }
      },
      "legacy-monolith-fallback": {
        "ClusterId": "legacyMonolithCluster",
        "Order": 1000,
        "Match": {
          "Path": "/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "modernDotNet9Cluster": {
        "Destinations": {
          "dotnet9App": {
            "Address": "https://orders-service.internal.net/"
          }
        }
      },
      "legacyMonolithCluster": {
        "Destinations": {
          "iisServer": {
            "Address": "https://legacy-iis.internal.net/"
          }
        }
      }
    }
  }
}
```

---

## Summary Comparison: When to Use Which Tier 2 Pattern

| Pattern | Architectural Scenario | Key Trade-off / Consideration | Go-To Modern .NET Tool |
| :--- | :--- | :--- | :--- |
| **Saga** | Data consistency across multiple microservices without distributed locks | Eventual consistency; must write compensating transactions for every state mutation | **MassTransit State Machines**, Temporal, Azure Durable Functions |
| **Idempotency** | Preventing duplicate operations on retried HTTP requests or message queues | Requires fast distributed storage (Redis) for key caching and atomic distributed locks | **StackExchange.Redis**, Custom ASP.NET Core Action Filter |
| **Template Method** | Fixed algorithm pipeline where only individual steps vary by format | Uses class inheritance; prefer Strategy pattern if algorithms must change dynamically at runtime | Abstract base class with `sealed` template method |
| **Anti-Corruption Layer** | Protecting clean Domain-Driven Design code from ugly third-party/legacy schemas | Extra mapping boilerplate; must maintain translation adapters when external APIs update | Explicit boundary Adapter & Domain Model Mappers |
| **Strangler Fig** | Migrating legacy monoliths incrementally without stopping business feature delivery | Temporary latency overhead of reverse proxy hop; maintaining shared session state during migration | **Microsoft YARP** (*Yet Another Reverse Proxy*) |


---

## Complete Pattern Catalog Summary

| Category | Patterns Covered |
| :--- | :--- |
| **Creational** | Singleton, Factory Method, Abstract Factory, Builder (Step Builder), Prototype |
| **Foundational** | Properties vs Fields, `internal` vs `public` Encapsulation |
| **Structural** | Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight |
| **Behavioral** | Strategy, Observer, Command, Mediator (Classic & MediatR), Chain of Responsibility, State (Finite State Machine) |
| **.NET Enterprise (Tier 1)** | Repository & Unit of Work, Options Pattern (`IOptions`), GC Foundations, Dispose vs Finalize, Complete Dispose Pattern, Result Pattern (ROP), Specification Pattern, Resilience (Circuit Breaker & Retry with Polly), Transactional Outbox, CQRS |
| **Distributed Architecture (Tier 2)** | Saga Pattern (Distributed Transactions), Idempotency Pattern, Template Method, Anti-Corruption Layer (ACL), Strangler Fig Pattern (YARP) |

*Generated as part of the C# .NET Software Architecture Masterclass.*
