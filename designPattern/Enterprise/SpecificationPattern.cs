using System.Linq.Expressions;

namespace designPattern.Enterprise;

// 1. The Domain Entity
public class CustomerCandidate
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal TotalSpend { get; set; }
    public string Country { get; set; } = string.Empty;
}

// 2. The Specification Interface
public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    Expression<Func<T, bool>> ToExpression();
}

// 3. Composite Base Specification
public abstract class BaseSpecification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public BaseSpecification<T> And(BaseSpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }
}

public class AndSpecification<T> : BaseSpecification<T>
{
    private readonly BaseSpecification<T> _left;
    private readonly BaseSpecification<T> _right;

    public AndSpecification(BaseSpecification<T> left, BaseSpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

// 4. Concrete Domain Specifications
public class ActiveCustomerSpecification : BaseSpecification<CustomerCandidate>
{
    public override Expression<Func<CustomerCandidate, bool>> ToExpression()
        => c => c.IsActive;
}

public class HighSpendCustomerSpecification : BaseSpecification<CustomerCandidate>
{
    private readonly decimal _minSpend;
    public HighSpendCustomerSpecification(decimal minSpend) => _minSpend = minSpend;

    public override Expression<Func<CustomerCandidate, bool>> ToExpression()
        => c => c.TotalSpend >= _minSpend;
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class SpecificationExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 5: The Specification Pattern ---");

        var customers = new List<CustomerCandidate>
        {
            new() { Name = "Alice", IsActive = true, TotalSpend = 1500m, Country = "US" },
            new() { Name = "Bob", IsActive = false, TotalSpend = 3000m, Country = "UK" },
            new() { Name = "Charlie", IsActive = true, TotalSpend = 400m, Country = "CA" },
            new() { Name = "Diana", IsActive = true, TotalSpend = 2200m, Country = "US" }
        };

        var activeSpec = new ActiveCustomerSpecification();
        var vipSpec = new HighSpendCustomerSpecification(1000m);

        // Combine using LINQ expression trees
        var eligibleForPromotionSpec = activeSpec.And(vipSpec);

        Console.WriteLine("Customers qualifying for VIP Campaign (Active AND Spend >= $1000):");
        var predicate = eligibleForPromotionSpec.ToExpression().Compile();
        var qualified = customers.Where(predicate).ToList();

        foreach (var c in qualified)
        {
            Console.WriteLine($"  ⭐ {c.Name} (Active: {c.IsActive}, Spend: ${c.TotalSpend})");
        }
    }
}
