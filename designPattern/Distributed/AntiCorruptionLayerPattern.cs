namespace designPattern.Distributed;

// =========================================================================
// 1. DIRTY EXTERNAL LEGACY SYSTEM (Legacy SAP Mainframe SOAP Schema)
// =========================================================================
public class LegacySapSoapRecord
{
    public string KUNNR { get; set; } = string.Empty; // Customer Number
    public string NAME1 { get; set; } = string.Empty; // Customer Name
    public string EMAIL_ADDR { get; set; } = string.Empty;
    public int STAT_CD { get; set; } // 1=Active, 4=Suspended, 9=Bankrupt
}

public class LegacySapClient
{
    public LegacySapSoapRecord? FetchRawCustomerByKunnr(string kunnr)
    {
        Console.WriteLine($"  [LEGACY-SAP-SOAP] Requesting raw XML payload for KUNNR: {kunnr}");
        return new LegacySapSoapRecord
        {
            KUNNR = kunnr,
            NAME1 = "Acme Corp International",
            EMAIL_ADDR = "billing@acme.corp",
            STAT_CD = 1 // Magic integer from 1998
        };
    }
}

// =========================================================================
// 2. CLEAN MODERN DOMAIN (.NET 9 Domain-Driven Design Model)
// =========================================================================
public enum CustomerStatus
{
    Active,
    Suspended,
    Terminated,
    Unknown
}

public record CustomerId(string Value);
public record CleanCustomer(CustomerId Id, string Name, string Email, CustomerStatus Status);

public interface ICustomerDirectory
{
    CleanCustomer? GetCustomer(CustomerId id);
}

// =========================================================================
// 3. THE ANTI-CORRUPTION LAYER (ACL Boundary Adapter)
// =========================================================================
public class SapAntiCorruptionLayer : ICustomerDirectory
{
    private readonly LegacySapClient _legacyClient;

    public SapAntiCorruptionLayer(LegacySapClient legacyClient) => _legacyClient = legacyClient;

    public CleanCustomer? GetCustomer(CustomerId id)
    {
        // Step 1: Call external legacy client
        var raw = _legacyClient.FetchRawCustomerByKunnr(id.Value);
        if (raw == null) return null;

        // Step 2: Sanitize & Translate legacy data into pristine domain models
        Console.WriteLine("  🛡️ [ACL-TRANSLATOR] Sanitizing dirty SAP record into pristine CleanCustomer domain aggregate.");
        return new CleanCustomer(
            Id: new CustomerId(raw.KUNNR),
            Name: raw.NAME1,
            Email: raw.EMAIL_ADDR,
            Status: MapLegacyStatusCode(raw.STAT_CD)
        );
    }

    private static CustomerStatus MapLegacyStatusCode(int statCd) => statCd switch
    {
        1 => CustomerStatus.Active,
        4 => CustomerStatus.Suspended,
        9 => CustomerStatus.Terminated,
        _ => CustomerStatus.Unknown
    };
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class AntiCorruptionLayerExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- Tier 2 Pattern 4: Anti-Corruption Layer (ACL) ---");

        var legacySapClient = new LegacySapClient();
        ICustomerDirectory directory = new SapAntiCorruptionLayer(legacySapClient);

        // Domain consumption: caller has ZERO knowledge of SAP or KUNNR
        Console.WriteLine("Core Domain service requesting customer via ICustomerDirectory:");
        var customer = directory.GetCustomer(new CustomerId("SAP-883920"));

        if (customer != null)
        {
            Console.WriteLine($"\nDomain Entity Retrieved:");
            Console.WriteLine($"  ID: {customer.Id.Value}");
            Console.WriteLine($"  Name: {customer.Name}");
            Console.WriteLine($"  Email: {customer.Email}");
            Console.WriteLine($"  Status: {customer.Status} (Clean Enum, no magic integers)");
        }
    }
}
