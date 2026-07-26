using System;
using System.Collections.Generic;
using Godot;

[Tool]
public partial class Weapon : Node3D
{
    [Export]
    float aim_speed = 8.0f;

    [Export]
    float fire_rate = 1.6f;

    [Export]
    Timer fire_rate_timer = null!;

    [Export]
    Node3D pivot_yaw = null!;

    [Export]
    Node3D pivot_pitch = null!;

    [Export]
    PackedScene projectile = null!;

    [Export]
    Marker3D spawn = null!;

    [Export]
    Area3D range = null!;

    readonly List<Hitbox> targets = new();

    public override void _Ready()
    {
        range.AreaEntered += OnAreaEnter;
        range.AreaExited += OnAreaExit;

        fire_rate_timer.Autostart = true;
        fire_rate_timer.WaitTime = fire_rate;
        fire_rate_timer.Timeout += OnFire;

        fire_rate_timer.Start(); // TODO: should only start fireing after first time a target got in range
    }

    void OnAreaEnter(Area3D area)
    {
        if (area is Hitbox hitbox)
        {
            targets.Add(hitbox);
        }
    }

    void OnHitboxTreeExit(Hitbox hitbox)
    {
        targets.Remove(hitbox);
    }

    void OnFire()
    {
        InvalidateTargets();
        if (!TryGetTarget(out Hitbox target))
            return;

        FireAt(target);
    }

    /// <summary>
    /// Clean dangling targets which became invalid while being in range.
    /// e.g. by dying by a previous attack or external sources.
    /// </summary>
    void InvalidateTargets()
    {
        targets.RemoveAll(GodotObjectExt.IsInstanceInvalid);
    }

    bool TryGetTarget(out Hitbox target)
    {
        target = null!;
        if (targets.Count > 0)
        {
            target = targets[0];
            return true;
        }
        return false;
    }

    void FireAt(Hitbox target)
    {
        Projectile projectile = this.projectile.Instantiate<Projectile>();
        projectile.SetTarget(target);
        // TODO: initilisation at runtime seems to be lacking. perhaps use ProjectileInit struct pattern?
        GetTree().Root.AddChild(projectile);
        projectile.GlobalPosition = spawn.GlobalPosition;
    }

    void OnAreaExit(Node3D area)
    {
        if (area is Hitbox hitbox)
            targets.Remove(hitbox);
    }

    public override void _Process(double delta)
    {
        if (!TryGetTarget(out Hitbox target)) // TODO: cache curr target
            return;

        AimAt(target, (float)delta);
    }

    void AimAt(Hitbox target, double delta)
    {
        Vector3 simulate_same_height_target_pos = new Vector3(
            target.GlobalPosition.X,
            pivot_yaw.GlobalPosition.Y,
            target.GlobalPosition.Z
        );

        Transform3D target_transform = pivot_yaw.GlobalTransform.LookingAt(
            simulate_same_height_target_pos,
            Vector3.Up,
            useModelFront: true
        );

        Quaternion currentQuat = pivot_yaw.GlobalTransform.Basis.GetRotationQuaternion();
        Quaternion targetQuat = target_transform.Basis.GetRotationQuaternion();

        Quaternion smoothedQuat = currentQuat.Slerp(targetQuat, aim_speed * (float)delta);

        pivot_yaw.GlobalTransform = new Transform3D(new Basis(smoothedQuat), pivot_yaw.GlobalPosition);
    }

    public override void _ExitTree()
    {
        if (Engine.IsEditorHint())
            return;
        fire_rate_timer.Timeout -= OnFire;
        base._ExitTree();
    }

    public override string[] _GetConfigurationWarnings()
    {
        return new ConfigWarnBuilder()
            .NotNull(projectile)
            .HasScript<Projectile>(projectile)
            .NotNull(spawn)
            .NotNull(range)
            .NotNull(fire_rate_timer)
            .NotNull(pivot_yaw)
            .NotNull(pivot_pitch)
            .That(fire_rate > 0.0f)
            .That(aim_speed < 10.0f && aim_speed > 0.0f)
            .Build();
    }
}
