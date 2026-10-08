using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace designPattern.Enterprise;

// =========================================================================
// WRITE MODEL (COMMANDS) - Enforces domain rules, transactional persistence
// =========================================================================
public record CreateProductCommand(string Name, decimal Price) : IRequest<Guid>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly ProductDatabase _db;
    public CreateProductCommandHandler(ProductDatabase db) => _db = db;

    public Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Price <= 0)
            throw new ArgumentException("Price must be positive.");

        var id = Guid.NewGuid();
        _db.Products[id] = new ProductEntity { Id = id, Name = request.Name, Price = request.Price };
        Console.WriteLine($"  [WRITE-COMMAND] Product created: '{request.Name}' (ID: {id.ToString()[..8]}, Price: ${request.Price})");
        return Task.FromResult(id);
    }
}

// =========================================================================
// READ MODEL (QUERIES) - Flat DTO, zero tracking overhead, optimized reads
// =========================================================================
public record ProductReadDto(Guid Id, string Name, decimal Price, string FormattedPrice);

public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductReadDto?>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductReadDto?>
{
    private readonly ProductDatabase _db;
    public GetProductByIdQueryHandler(ProductDatabase db) => _db = db;

    public Task<ProductReadDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (_db.Products.TryGetValue(request.ProductId, out var entity))
        {
            var dto = new ProductReadDto(entity.Id, entity.Name, entity.Price, $"${entity.Price:F2}");
            Console.WriteLine($"  [READ-QUERY] Fetched read-optimized DTO for product: '{dto.Name}'");
            return Task.FromResult<ProductReadDto?>(dto);
        }

        Console.WriteLine($"  [READ-QUERY] Product '{request.ProductId.ToString()[..8]}' not found.");
        return Task.FromResult<ProductReadDto?>(null);
    }
}

// Simulated Shared Store
public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class ProductDatabase
{
    public Dictionary<Guid, ProductEntity> Products { get; } = new();
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class CqrsExample
{
    public static async Task Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 8: CQRS with MediatR ---");

        var services = new ServiceCollection();
        var db = new ProductDatabase();
        services.AddSingleton(db);

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CqrsExample).Assembly));

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        // 1. Dispatch WRITE command
        Console.WriteLine("\n1. Dispatching CreateProductCommand (Write Side):");
        var productId = await mediator.Send(new CreateProductCommand("Mechanical Gaming Keyboard", 129.99m));

        // 2. Dispatch READ query
        Console.WriteLine("\n2. Dispatching GetProductByIdQuery (Read Side):");
        var productDto = await mediator.Send(new GetProductByIdQuery(productId));

        if (productDto != null)
        {
            Console.WriteLine($"  Result: {productDto.Name} costs {productDto.FormattedPrice}");
        }
    }
}
