using System;
using System.Diagnostics;
using Godot;

public partial class EnemySpawn : Node
{
    ///<summary>The enemy to spawn.</summary>
    [Export]
    public PackedScene Enemy;

    [Export]
    public Path3D Path;

    private Timer timer;

    public override void _Ready()
    {
        Debug.Assert(Path != null, $"{nameof(Path)} is required.");
        Debug.Assert(Enemy != null, $"{nameof(Enemy)} is required.");

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
