using System;
using Godot;

public partial class Projectile : Node3D
{
    [Export]
    float speed = 10.0f;

    [Export]
    float damage = 1.0f;

    Hitbox target = null!;

    public override void _Ready()
    {
        Assert.NotNull(target, nameof(target));
    }

    public override void _Process(double delta)
    {
        if (!GodotObject.IsInstanceValid(target))
        {
            this.QueueFree(); // TODO: expose signal (could be used to display "miss" msgs on screen)
            return;
        }

        FlyAtTarget((float)delta);
    }

    void FlyAtTarget(float delta)
    {
        Vector3 dest = target.GlobalPosition;
        Vector3 direction = (dest - GlobalPosition).Normalized();
        float distance = GlobalPosition.DistanceTo(dest);
        float step = speed * delta;

        if (direction != Vector3.Zero)
        {
            // point the mesh in the flying direction
            LookAt(dest, Vector3.Up);
        }
        else
        {
            // Why can direction be Vector3.Zero?
            // If the projectile reaches the exact coordinates of target.GlobalPosition,
            // dest - GlobalPosition becomes (0, 0, 0). Normalizing Vector3.Zero returns Vector3.Zero.
            // Godot’s LookAt() will crash or log an error if you pass it a zero direction vector
            // (because it can't determine an angle to look at itself)
        }

        if (distance <= step)
        {
            GlobalPosition = dest;
            OnHit();
        }
        else
        {
            GlobalPosition += direction * step;
        }
    }

    void OnHit()
    {
        target.DelegateHit(damage);
        QueueFree();
        // TODO: expose signal (e.g. sound for hitting wooden arrow)
    }

    ///<summary>has to be called before being added to the scene.</summary>
    public void SetTarget(Hitbox target)
    {
        this.target = target;
    }
}
