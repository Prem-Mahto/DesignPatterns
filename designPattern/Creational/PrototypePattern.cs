namespace designPattern.Creational;

public interface IPrototype<T>
{
    T Clone();
}

// Composition class
public class ClassA : IPrototype<ClassA>
{
    public string Department { get; set; }
    public int OfficeNumber { get; set; }

    public ClassA(string department, int officeNumber)
    {
        Department = department;
        OfficeNumber = officeNumber;
    }

    public ClassA Clone()
    {
        return (ClassA)this.MemberwiseClone();
    }
}

// Complex object implementing deep clone
public class EmploymentContract : IPrototype<EmploymentContract>
{
    public string EmployeeName { get; set; }
    public decimal BaseSalary { get; set; }
    public List<string> Benefits { get; set; }
    public ClassA A { get; set; }

    public EmploymentContract(string employeeName, decimal baseSalary, List<string> benefits, ClassA a)
    {
        EmployeeName = employeeName;
        BaseSalary = baseSalary;
        Benefits = benefits;
        A = a;
    }

    // Heavy initialization simulation
    public static EmploymentContract CreateMasterTemplate()
    {
        Console.WriteLine("--> [Template] Loading 50 pages of legal clauses and standard benefits into memory...");

        return new EmploymentContract(
            employeeName: "Template",
            baseSalary: 50000m,
            benefits: new List<string> { "Health Insurance", "401k Matching", "Gym Pass" },
            a: new ClassA("Engineering", 101)
        );
    }

    public EmploymentContract Clone()
    {
        // 1. Shallow copy primitive values
        var clone = (EmploymentContract)this.MemberwiseClone();

        // 2. Deep copy the reference type list
        clone.Benefits = new List<string>(this.Benefits);

        // 3. Deep copy the composed object
        clone.A = this.A.Clone();

        return clone;
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class PrototypeExample
{
    public static void Run()
    {
        Console.WriteLine("=== PROTOTYPE PATTERN ===");

        // 1. Load expensive master template once
        var masterTemplate = EmploymentContract.CreateMasterTemplate();

        // 2. Clone in microseconds for Employee #1
        var john = masterTemplate.Clone();
        john.EmployeeName = "John Doe";
        john.BaseSalary = 85000m;
        john.Benefits.Add("Company Car");
        john.A.OfficeNumber = 205;

        // 3. Clone in microseconds for Employee #2
        var jane = masterTemplate.Clone();
        jane.EmployeeName = "Jane Smith";
        jane.BaseSalary = 95000m;

        Console.WriteLine($"John: {john.EmployeeName}, Salary: ${john.BaseSalary}, Office: {john.A.OfficeNumber}, Benefits: {string.Join(", ", john.Benefits)}");
        Console.WriteLine($"Jane: {jane.EmployeeName}, Salary: ${jane.BaseSalary}, Office: {jane.A.OfficeNumber}, Benefits: {string.Join(", ", jane.Benefits)}");
        Console.WriteLine($"Master Template Office remains: {masterTemplate.A.OfficeNumber}, Benefits count: {masterTemplate.Benefits.Count}");
        Console.WriteLine();
    }
}
