using DesignPatterns_Practice.Behavioral;
using DesignPatterns_Practice.Creational;
using DesignPatterns_Practice.Structural;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DesignPatterns_Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Design Patterns Examples");
            //SingletonExample.Run();

            //FactoryExample.Run();
            //ModernFactoryExample.Run();
            //AbstractFactoryExample.Run();

            //BuilderExample.Run();
            //PrototypeExample.Run();

            //AdaptorExample.Run();
            //DecoratorExample.Run();
            //FacadeExample.Run();
            //ProxyExample.Run();
            //CompositeExample.Run();
            //BridgeExample.Run();
            //FlyweightExample.Run();

            //StrategyExample.Run();
            ObserverExample.Run();
        }
    }
}
