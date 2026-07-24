using System;
using System.Diagnostics;
using Godot;

public partial class EnemySpawn : Node
{
    ///<summary>The enemy to spawn.</summary>
    [Export]
    public PackedScene Enemy = null!;

    [Export]
    public Path3D Path = null!;

    Timer timer = null!;

    public override void _Ready()
    {
        Assert.NotNull(Enemy, nameof(Enemy));
        Assert.NotNull(Path, nameof(Path));

        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnSpawn;
    }

    public override void _Process(double delta) { }

    private void OnSpawn()
    {
        Node enemy = Enemy.Instantiate();
        Path.AddChild(enemy);
    }

    public override void _ExitTree()
    {
        timer.Timeout -= OnSpawn;
        base._ExitTree();
    }
}
