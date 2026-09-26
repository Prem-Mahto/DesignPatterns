using System;

namespace DesignPatterns_Practice.Creational;

/// <summary>
/// shallow copy -> copy only value type but refrence will not be copied
/// deep copy -> copy value type and reference type as well :- two way to implement - 1. Clone method in each class 2. Serialization and Deserialization
/// </summary>
/// 
/// <typeparam name="T"></typeparam>
interface IPrototype<T>
{
    T Clone();
}

class DepartmentInfo : IPrototype<DepartmentInfo>
{
    public string Department { get; set; }
    public int OfficeNumber { get; set; }

    public DepartmentInfo(string department, int officeNumber)
    {
        Department = department;
        OfficeNumber = officeNumber;
    }
    public DepartmentInfo Clone()
    {
        return new DepartmentInfo(this.Department, this.OfficeNumber);
    }

}

class EmployementContract : IPrototype<EmployementContract>
{
    public string EmployeeName { get; set; }
    public decimal BaseSalary { get; set; }
    public List<string> Benefits { get; set; }
    public DepartmentInfo DepartmentInfo { get; set; }

    public EmployementContract(string employeeName, decimal baseSalary, List<string> benefits, DepartmentInfo departmentInfo)
    {
        Console.WriteLine("---> [Template] Loading 50 pages of legal clauses and standard benefits into memory... ");
        Thread.Sleep(3000); // Simulate loading time

        EmployeeName = employeeName;
        BaseSalary = baseSalary;
        Benefits = benefits;
        DepartmentInfo = departmentInfo;
    }

    private EmployementContract() { }

    public static EmployementContract CreateMasterTemplate()
    {
        Console.WriteLine("---> [Template] Loading 50 pages of legal clauses and standard benefits into memory... ");
        Thread.Sleep(3000); // Simulate loading time

        return new EmployementContract()
        {
            EmployeeName = "Template",
            BaseSalary = 50000m,
            Benefits = new List<string> { "Health Insurance" },
            DepartmentInfo = new DepartmentInfo("General", 101)
        };
    }

    public EmployementContract Clone()
    {
        return new EmployementContract
        {
            EmployeeName = this.EmployeeName,
            BaseSalary = this.BaseSalary,
            Benefits = new List<string>(this.Benefits),
            DepartmentInfo = this.DepartmentInfo.Clone()
        };
    }

    public void WriteInfo() => Console.WriteLine($"Instance created for {this.EmployeeName}");
}

public class PrototypeExample
{
    public static void Run()
    {
        BeforePrototype();
        AfterPrototype();
    }


    public static void BeforePrototype()
    {
        Console.WriteLine("before Prototype");

        var JaneContract = new EmployementContract(
            employeeName: "Jane Doe",
            baseSalary: 50000m,
            benefits: new List<string> { "Health Insurance" },
            departmentInfo: new DepartmentInfo("HR", 1)
        );
        JaneContract.WriteInfo();

        var JohnContract = new EmployementContract(
            employeeName: "John Doe",
            baseSalary: 60000m,
            benefits: new List<string> { "Health Insurance", "Gym Pass" },
            departmentInfo: new DepartmentInfo("Engineering", 2)
        );
        JohnContract.WriteInfo();

        var HarryContract = new EmployementContract(
            employeeName: "Harry Doe",
            baseSalary: 6000m,
            benefits: new List<string> { "Health Insurance", "Paid Time Off" },
            departmentInfo: new DepartmentInfo("HouseKeeping", 3)
        );
        HarryContract.WriteInfo();
    }

    public static void AfterPrototype()
    {
        Console.WriteLine("Prototype Example:");
        var templateContract = EmployementContract.CreateMasterTemplate();

        var JaneContractClone = templateContract.Clone();
        JaneContractClone.EmployeeName = "Jane Doe";
        JaneContractClone.BaseSalary = 50000m;
        JaneContractClone.DepartmentInfo = new DepartmentInfo("HR", 1);
        JaneContractClone.WriteInfo();

        var JohnContractClone = templateContract.Clone();
        JohnContractClone.EmployeeName = "John Doe";
        JohnContractClone.BaseSalary = 60000m;
        JohnContractClone.Benefits.AddRange(new List<string> { "Gym Pass" });
        JohnContractClone.DepartmentInfo = new DepartmentInfo("Engineering", 2);
        JaneContractClone.WriteInfo();

        var HarryContractClone = templateContract.Clone();
        HarryContractClone.EmployeeName = "Harry Doe";
        HarryContractClone.BaseSalary = 6000m;
        HarryContractClone.Benefits.AddRange(new List<string> { "Paid Time Off" });
        HarryContractClone.DepartmentInfo = new DepartmentInfo("HouseKeeping", 3);
        HarryContractClone.WriteInfo();
    }

}