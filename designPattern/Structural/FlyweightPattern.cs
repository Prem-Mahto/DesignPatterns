namespace designPattern.Structural;

// 1. The Flyweight (Holds the heavy intrinsic shared state)
public class TreeType
{
    public string Name { get; }
    public string Color { get; }
    public byte[] Texture3DData { get; } // Heavy 50KB asset

    public TreeType(string name, string color, byte[] texture3DData)
    {
        Name = name;
        Color = color;
        Texture3DData = texture3DData;
    }

    // Extrinsic state (x, y) is passed in at runtime
    public void Draw(int x, int y)
    {
        Console.WriteLine($"Drawing '{Name}' tree ({Color}) at coordinates ({x}, {y})");
    }
}

// 2. The Flyweight Factory (Manages shared pool)
public class TreeFactory
{
    private static readonly Dictionary<string, TreeType> _treeTypes = new();

    public static TreeType GetTreeType(string name, string color)
    {
        string key = $"{name}_{color}";

        if (!_treeTypes.TryGetValue(key, out var type))
        {
            Console.WriteLine($"--> [CACHE MISS] Loading heavy 3D mesh for {name} ({color}) into RAM...");
            byte[] mockTexture = new byte[50 * 1024]; // 50 KB simulation
            type = new TreeType(name, color, mockTexture);
            _treeTypes[key] = type;
        }

        return type;
    }

    public static int TotalSharedFlyweightsLoaded => _treeTypes.Count;
}

// 3. The Context (Lightweight object referencing the flyweight)
public class Tree
{
    private readonly int _x;
    private readonly int _y;
    private readonly TreeType _type;

    public Tree(int x, int y, TreeType type)
    {
        _x = x;
        _y = y;
        _type = type;
    }

    public void Render()
    {
        _type.Draw(_x, _y);
    }
}

// 4. Forest Client
public class Forest
{
    private readonly List<Tree> _trees = new();

    public void PlantTree(int x, int y, string name, string color)
    {
        TreeType type = TreeFactory.GetTreeType(name, color);
        _trees.Add(new Tree(x, y, type));
    }

    public void Render()
    {
        foreach (var tree in _trees)
        {
            tree.Render();
        }
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class FlyweightExample
{
    public static void Run()
    {
        Console.WriteLine("=== FLYWEIGHT PATTERN ===");

        var forest = new Forest();

        forest.PlantTree(10, 20, "Oak", "Green");
        forest.PlantTree(15, 25, "Oak", "Green");
        forest.PlantTree(50, 80, "Oak", "Green");
        forest.PlantTree(100, 200, "Pine", "DarkGreen");
        forest.PlantTree(105, 205, "Pine", "DarkGreen");

        forest.Render();

        Console.WriteLine($"Total shared heavy assets in RAM: {TreeFactory.TotalSharedFlyweightsLoaded} (Oak and Pine)");
        Console.WriteLine();
    }
}
