using System;
using Godot;

public partial class Hitbox : Area3D
{
    [Export]
    EnemyHealth health = null!;

    public EnemyHealth Health { get => health; }

    public override void _Ready()
    {
        Assert.NotNull(health, nameof(health)); // TODO: move to edit warn
    }

    public override void _Process(double delta) { }

    public void DelegateHit(float damage)
    {
        health.Substract(damage);
    }
}
