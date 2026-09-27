using System;

namespace DesignPatterns_Practice.Structural;

public class TreeType
{
    public string Name { get; set; }
    public string Color { get; set; }
    public byte[] Texture3DData { get; set; } // Simulating a large texture data

    public TreeType(string name, string color)
    {
        this.Name = name;
        this.Color = color;
        this.Texture3DData = new byte[5000 * 1024]; // 5 MB simulation ;
    }
    public void Draw(int x, int y)
    {
        Console.WriteLine($"Drawing tree {Name} of color {Color} at ({x}, {y})");
    }
}

public class TreeFactory
{
    private static readonly Dictionary<string, TreeType> treeTypes = new Dictionary<string, TreeType>();

    public static TreeType GetTreeType(string name, string color)
    {
        string key = $"{name}_{color}";
        if (!treeTypes.ContainsKey(key))
        {
            treeTypes[key] = new TreeType(name, color);
        }
        return treeTypes[key];
    }
}


public class Tree
{
   
    private  readonly int x;    private readonly int y;
    private readonly TreeType treeType;

    public Tree(int x, int y, TreeType treeType)
    {
        this.x = x;
        this.y = y;
        this.treeType = treeType;       
    }
    
    public void Render()
    {
        treeType.Draw(x, y);
    }
}


public class FlyweightExample
{
    public static void Run()
    {
        for (int i = 0; i < 10; i++)
        {
            var treeType = TreeFactory.GetTreeType("Oak", "Green");
            var tree = new Tree(i, i, treeType);
            tree.Render();
        }
        for (int i = 0; i < 10; i++)
        {
            var treeType = TreeFactory.GetTreeType("Pine", "Dark Green");
            var tree = new Tree(i, i, treeType);
            tree.Render();
        }
    }
}
