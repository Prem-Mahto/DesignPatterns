using DesignPatterns_Practice.Creational;
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
            //BuilderExample.Run();
            FactoryExample.Run();


            ///create a host builder
            //HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            //builder.Services.AddSingleton<ModernOrderService>();
            //builder.Services.AddKeyedScoped<IPaymentProcessor, CreditCardProcessor>(PaymentMethod.CreditCard);
            //builder.Services.AddKeyedScoped<IPaymentProcessor, PayPalProcessor>(PaymentMethod.PayPal);
            //builder.Services.AddKeyedScoped<IPaymentProcessor, ApplePayProcessor>(PaymentMethod.ApplePay);
            //builder.Services.AddKeyedScoped<IPaymentProcessor, CryptoProcessor>(PaymentMethod.Crypto);


            //Console.WriteLine("Modern Factory Example:");
            //var serviceProvider = builder.Services.BuildServiceProvider();
            //var modernOrderService = serviceProvider.GetRequiredService<ModernOrderService>();
            //var modernFactoryExample = new ModernFactoryExample(modernOrderService);
            //modernFactoryExample.Run();


            //AbstractFactoryExample.Run();
            //PrototypeExample.Run();
        }   
    }
}




