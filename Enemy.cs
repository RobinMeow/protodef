using Godot;
using System;

public partial class Enemy : PathFollow3D
{
    [Export]
    public float Speed = 1.7f;

	public override void _Ready()
	{
        Loop = false;
	}

	public override void _Process(double delta)
	{
        Progress += Speed * (float)delta;

        bool end_reached = ProgressRatio >= 1.0f;

        if (end_reached)
            QueueFree();
	}
}
