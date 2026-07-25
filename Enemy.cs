using System;
using Godot;

public partial class Enemy : PathFollow3D
{
    [Export]
    float speed = 1.0f;

    [Export]
    EnemyHealth health = null!; // TODO: warning message

    public override void _Ready()
    {
        Loop = false;
        health.Depleted += OnDeath;
    }

    public override void _Process(double delta)
    {
        Progress += speed * (float)delta;

        bool dest_reached = ProgressRatio >= 1.0f;

        if (dest_reached)
            QueueFree();
    }

    void OnDeath(float overkill_amount)
    {
        health.Depleted -= OnDeath;
        // TODO: expose signal (e.g. dying sound, kill points)
        QueueFree();
    }

    public override void _ExitTree()
    {
        health.Depleted -= OnDeath;
        base._ExitTree();
    }
}
