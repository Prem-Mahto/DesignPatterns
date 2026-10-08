using System;
using designPattern.Creational;
using designPattern.Structural;
using designPattern.Behavioral;
using designPattern.Enterprise;
using designPattern.Distributed;

namespace designPattern;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("===================================================================");
        Console.WriteLine("           DESIGN PATTERNS IN C# / .NET ARCHITECTURE               ");
        Console.WriteLine("===================================================================\n");

        // ---------------------------------------------------------
        // 1. CREATIONAL PATTERNS (How objects are born)
        // ---------------------------------------------------------
        //SingletonExample.Run();
        //FactoryExample.Run();
        //BuilderExample.Run();
        //PrototypeExample.Run();

        // ---------------------------------------------------------
        // 2. STRUCTURAL PATTERNS (How objects fit together)
        // ---------------------------------------------------------
        //AdapterExample.Run();
        //DecoratorExample.Run();
        //FacadeExample.Run();
        //ProxyExample.Run();
        //CompositeExample.Run();
        //BridgeExample.Run();
        //FlyweightExample.Run();

        // ---------------------------------------------------------
        // 3. BEHAVIORAL PATTERNS (How objects communicate)
        // ---------------------------------------------------------
        //StrategyExample.Run();
        //ObserverExample.Run();
        //CommandExample.Run();
        //MediatorExample.Run();
        //ChainOfResponsibilityExample.Run();
        StateExample.Run();
        TemplateMethodExample.Run();

        // ---------------------------------------------------------
        // 4. .NET ENTERPRISE PATTERNS (Tier 1: Daily Production)
        // ---------------------------------------------------------
        RepositoryAndUnitOfWorkExample.Run();
        OptionsExample.Run();
        await DisposeExample.Run();
        ResultExample.Run();
        SpecificationExample.Run();
        ResilienceExample.Run();
        TransactionalOutboxExample.Run();
        await CqrsExample.Run();

        // ---------------------------------------------------------
        // 5. DISTRIBUTED ARCHITECTURE PATTERNS (Tier 2: Good to Know)
        // ---------------------------------------------------------
        SagaExample.Run();
        IdempotencyExample.Run();
        AntiCorruptionLayerExample.Run();
        StranglerFigExample.Run();

        Console.WriteLine("\n===================================================================");
        Console.WriteLine("              ALL DESIGN PATTERNS EXECUTED SUCCESSFULLY            ");
        Console.WriteLine("===================================================================");
    }
}
