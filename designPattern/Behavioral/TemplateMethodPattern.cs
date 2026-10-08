namespace designPattern.Behavioral;

// 1. The Abstract Base Engine (Defines algorithm skeleton)
public abstract class DataImportPipeline
{
    // The Template Method: Sealed so subclasses cannot alter the mandatory execution sequence
    public void ExecutePipeline(string sourcePath)
    {
        Console.WriteLine($"\n=== Starting Ingestion Pipeline for: {sourcePath} ===");
        Authenticate();
        string rawData = ReadSource(sourcePath);
        
        // Abstract hook: subclasses provide custom parsing
        List<string> records = ParseRecords(rawData);

        Validate(records);
        SaveToDatabase(records);
        
        // Virtual hook with default implementation
        OnCompleted(records.Count);
    }

    private void Authenticate() => Console.WriteLine("  [1/5 AUTH] Authenticating with secure storage endpoint...");
    
    protected virtual string ReadSource(string path) 
    {
        Console.WriteLine($"  [2/5 READ] Reading raw stream from {path}...");
        return "SAMPLE_RAW_STREAM_DATA";
    }

    // Abstract Step: Must be implemented by concrete format processors
    protected abstract List<string> ParseRecords(string rawData);

    private void Validate(List<string> records) 
        => Console.WriteLine($"  [4/5 VALIDATE] Validated {records.Count} records against enterprise schema.");

    private void SaveToDatabase(List<string> records) 
        => Console.WriteLine($"  [5/5 PERSIST] Batch-inserted {records.Count} records into SQL Server.");

    protected virtual void OnCompleted(int count) 
        => Console.WriteLine($"  [NOTIFY] Pipeline execution finished. {count} records successfully ingested.");
}

// 2. Concrete Subclasses
public class CsvDataImportPipeline : DataImportPipeline
{
    protected override List<string> ParseRecords(string rawData)
    {
        Console.WriteLine("  [3/5 PARSE-CSV] Parsing comma-separated rows into typed records.");
        return new List<string> { "Row1: Customer A, $100", "Row2: Customer B, $250" };
    }
}

public class JsonDataImportPipeline : DataImportPipeline
{
    protected override List<string> ParseRecords(string rawData)
    {
        Console.WriteLine("  [3/5 PARSE-JSON] Deserializing JSON array payload into typed records.");
        return new List<string> { "{ \"id\": 1, \"amount\": 400 }", "{ \"id\": 2, \"amount\": 120 }" };
    }

    protected override void OnCompleted(int count)
    {
        base.OnCompleted(count);
        Console.WriteLine("  [CUSTOM HOOK] Dispatched Slack notification for JSON real-time batch.");
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class TemplateMethodExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- Template Method Pattern ---");

        DataImportPipeline csvPipeline = new CsvDataImportPipeline();
        csvPipeline.ExecutePipeline("s3://finance/invoices_2026.csv");

        DataImportPipeline jsonPipeline = new JsonDataImportPipeline();
        jsonPipeline.ExecutePipeline("https://api.partner.com/transactions.json");
    }
}
