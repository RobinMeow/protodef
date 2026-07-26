using System.Collections.Generic;
using Godot;

[Tool]
public partial class Tower : Node3D
{
    [Export]
    float fire_rate = 1.6f;

    // TODO: if we dont let designers drag and drop the timer in here
    // we might as well just generate the timer from code
    Timer fire_rate_timer = null!;

    [Export]
    PackedScene projectile = null!;

    [Export]
    Weapon weapon = null!;

    readonly List<Hitbox> targets = new();

    public override void _Ready()
    {
        Area3D range = GetNode<Area3D>("RangeArea");
        range.AreaEntered += OnAreaEnter;
        range.AreaExited += OnAreaExit;

        fire_rate_timer = GetNode<Timer>("FireRateTimer");
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
        if (TryGetTarget(out Hitbox target))
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
        projectile.GlobalPosition = weapon.SpawnPos;
    }

    void OnAreaExit(Node3D area)
    {
        if (area is Hitbox hitbox)
            targets.Remove(hitbox);
    }

    public override void _Process(double delta) { }

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
            .NotNull(weapon)
            .HasScript<Projectile>(projectile)
            .NotNull(fire_rate_timer)
            .NotNull(
                GetNodeOrNull<Area3D>("RangeArea"),
                "Missing required child node: 'RangeArea' (Area3D)."
            )
            .NotNull(
                GetNodeOrNull<Timer>("FireRateTimer"),
                "Missing required child node: 'FireRateTimer' (Timer)."
            )
            .Build();
    }
}
