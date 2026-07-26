using System;
using Godot;

[Tool]
public partial class Weapon : Node3D
{
    [Export]
    Marker3D spawn = null!;
    public Vector3 SpawnPos { get => spawn.GlobalPosition; }

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
            return;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;
    }

    public override string[] _GetConfigurationWarnings() =>
        new ConfigWarnBuilder().NotNull(spawn).Build();
}
