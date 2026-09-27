using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DesignPatterns_Practice.Structural
{
    // 1. Subject Interface
    public interface IDocumentService 
    {
        void DisplayDocument(string documentId);        
    }

    // 2. Real Subject (Heavy/Sensitive)
    public class RealDocumentService : IDocumentService
    {
        public RealDocumentService()
        {
            Console.WriteLine("[RealDocumentService] Initializing PDF rendering engine...");
            Thread.Sleep(2000);//heavy operation
            Console.WriteLine("[RealDocumentService] Initialization complete");

        }

        public void DisplayDocument(string documentId)
        {
            Console.WriteLine($"[RealDocumentService] Rendering document contents for: '{documentId}'");
        }
    }


    // 2. Real Subject (Heavy/Sensitive)
    public class DocumentServiceProxy : IDocumentService
    {
        private readonly string _currentUserRole;
        private RealDocumentService? _realService;
        public DocumentServiceProxy(string currentUserRole)
        {
            _currentUserRole = currentUserRole;
        }

        public void DisplayDocument(string documentId)
        {
            Console.WriteLine($"[Audit Log] User with role '{_currentUserRole}' requested doc '{documentId}'.");

            // Protection / Authorization check
            if (_currentUserRole != "Admin" && _currentUserRole != "HRManager")
            {
                Console.WriteLine($"[Access Denied] User '{_currentUserRole}' is unauthorized to view confidential docs!\n");
                return;
            }

            // Virtual Proxy: Lazy loading
            _realService ??= new RealDocumentService();

            // Delegation
            _realService.DisplayDocument(documentId);
            Console.WriteLine("[Audit Log] Document successfully rendered.\n");
        }
    }


    public class ProxyExample
    {
        public static void Run()
        {
            Console.WriteLine("=== PROXY PATTERN ===");

            Console.WriteLine("--- Scenario 1: Regular Employee attempts access ---");
            IDocumentService regularUserProxy = new DocumentServiceProxy("Employee");
            regularUserProxy.DisplayDocument("Salary_Q3_2026.pdf");

            Console.WriteLine("--- Scenario 2: HR Manager attempts access ---");
            IDocumentService hrProxy = new DocumentServiceProxy("HRManager");
            hrProxy.DisplayDocument("Salary_Q3_2026.pdf");

            Console.WriteLine();
        }
    }
}
