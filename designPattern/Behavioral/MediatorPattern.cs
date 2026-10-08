using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace designPattern.Behavioral;

// =============================================================
// 1. CLASSIC GoF MEDIATOR PATTERN (Air Traffic Control Tower)
// =============================================================

public interface IAirTrafficControl
{
    void RegisterFlight(Airplane plane);
    void SendMessage(string message, Airplane sender);
}

public abstract class Airplane
{
    protected readonly IAirTrafficControl _atc;
    public string CallSign { get; }

    protected Airplane(IAirTrafficControl atc, string callSign)
    {
        _atc = atc;
        CallSign = callSign;
    }

    public abstract void Receive(string message);

    public void Send(string message)
    {
        Console.WriteLine($"\n[Radio] {CallSign} broadcasts to Tower: \"{message}\"");
        _atc.SendMessage(message, this);
    }
}

public class PassengerFlight : Airplane
{
    public PassengerFlight(IAirTrafficControl atc, string callSign) : base(atc, callSign) { }

    public override void Receive(string message)
    {
        Console.WriteLine($"✈️ [Passenger {CallSign}] Heard from Tower: '{message}'");
    }
}

public class CargoFlight : Airplane
{
    public CargoFlight(IAirTrafficControl atc, string callSign) : base(atc, callSign) { }

    public override void Receive(string message)
    {
        Console.WriteLine($"📦 [Cargo {CallSign}] Heard from Tower: '{message}'");
    }
}

public class AirTrafficControlTower : IAirTrafficControl
{
    private readonly List<Airplane> _planes = new();

    public void RegisterFlight(Airplane plane)
    {
        _planes.Add(plane);
        Console.WriteLine($"[Tower] Flight {plane.CallSign} entered our airspace.");
    }

    public void SendMessage(string message, Airplane sender)
    {
        foreach (var plane in _planes)
        {
            if (plane != sender)
            {
                plane.Receive(message);
            }
        }
    }
}

// =============================================================
// 2. MODERN MediatR PATTERN (Commands, Notifications, Pipelines)
// =============================================================

// 1-to-1 Command
public record LandingClearance(bool IsApproved, string Runway, string Reason);
public record RequestLandingCommand(string FlightNumber, string AircraftType) : IRequest<LandingClearance>;

public class RequestLandingHandler : IRequestHandler<RequestLandingCommand, LandingClearance>
{
    public Task<LandingClearance> Handle(RequestLandingCommand request, CancellationToken ct)
    {
        Console.WriteLine($"[Tower Controller] Evaluating landing request for {request.FlightNumber} ({request.AircraftType})...");

        if (request.AircraftType == "HeavyBoeing777")
        {
            return Task.FromResult(new LandingClearance(
                IsApproved: true,
                Runway: "Runway-26L (Long)",
                Reason: "Cleared for landing on long runway. Wind 5 knots."));
        }

        return Task.FromResult(new LandingClearance(
            IsApproved: true,
            Runway: "Runway-08R",
            Reason: "Cleared to land."));
    }
}

// 1-to-Many Notification (Broadcast)
public record MaydayAlertNotification(string FlightNumber, string EmergencyType) : INotification;

public class EmergencyServicesHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification notification, CancellationToken ct)
    {
        Console.WriteLine($"🚨 [FIRE TRUCKS & RESCUE] Dispatched to runway for {notification.FlightNumber}! Emergency: {notification.EmergencyType}");
        return Task.CompletedTask;
    }
}

public class GroundOperationsHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification notification, CancellationToken ct)
    {
        Console.WriteLine($"🛑 [GROUND OPS] Halting all taxiing aircraft. Clearing perimeter for {notification.FlightNumber}.");
        return Task.CompletedTask;
    }
}

public class RadarAuditLoggerHandler : INotificationHandler<MaydayAlertNotification>
{
    public Task Handle(MaydayAlertNotification notification, CancellationToken ct)
    {
        Console.WriteLine($"📝 [FLIGHT RECORDER] Incident logged in permanent database: {notification.FlightNumber} at {DateTime.UtcNow}.");
        return Task.CompletedTask;
    }
}

// Pipeline Behavior (Blackbox Logger)
public class FlightTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        Console.WriteLine($"📡 [RADAR TELEMETRY] Intercepted message: {typeof(TRequest).Name}");
        var response = await next();
        Console.WriteLine($"📡 [RADAR TELEMETRY] Successfully processed response.");
        return response;
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class MediatorExample
{
    public static void Run()
    {
        Console.WriteLine("=== MEDIATOR PATTERN ===");

        // Part 1: Classic GoF Mediator
        Console.WriteLine("--- 1. Classic GoF Air Traffic Control Tower ---");
        IAirTrafficControl tower = new AirTrafficControlTower();

        var delta101 = new PassengerFlight(tower, "Delta-101");
        var united452 = new PassengerFlight(tower, "United-452");
        var fedexHeavy = new CargoFlight(tower, "FedEx-Heavy");

        tower.RegisterFlight(delta101);
        tower.RegisterFlight(united452);
        tower.RegisterFlight(fedexHeavy);

        delta101.Send("Descended to 10,000 feet, entering final approach.");
        fedexHeavy.Send("Holding at runway 26R, waiting for clearance.");

        // Part 2: Modern MediatR
        Console.WriteLine("\n--- 2. Modern MediatR ATC Implementation ---");
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatorExample).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FlightTelemetryBehavior<,>));
        });

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        // MediatR 1-to-1 Command
        var landingCommand = new RequestLandingCommand("Delta-101", "HeavyBoeing777");
        var clearance = mediator.Send(landingCommand).GetAwaiter().GetResult();
        Console.WriteLine($"✈️ [Cockpit] Clearance: Approved={clearance.IsApproved}, Assigned={clearance.Runway}. ({clearance.Reason})");

        // MediatR 1-to-Many Notification
        Console.WriteLine("\n⚠️ [Emergency Alert Broadcast]");
        var emergency = new MaydayAlertNotification("United-99", "Hydraulic Failure");
        mediator.Publish(emergency).GetAwaiter().GetResult();

        Console.WriteLine();
    }
}
