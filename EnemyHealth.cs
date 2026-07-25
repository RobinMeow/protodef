using System;
using Godot;

public partial class EnemyHealth : Node
{
    const float default_health = 2.0f;

    [Export]
    float max_health = default_health;

    float current_health = default_health;

    ///<summary>overkill amount is either zero or positive.</summary>
    [Signal]
    public delegate void DepletedEventHandler(float overkill_amount);

    public override void _Ready()
    {
        current_health = max_health;
    }

    public override void _Process(double delta) { }

    public void Substract(float damage)
    {
        // TODO: multiple hits could be taken within the same frame
        // could exit, but than we loose the overkill amount

        current_health -= damage;

        if (current_health <= 0)
            EmitSignal(SignalName.Depleted, Math.Abs(current_health)); // overkill_amount e.g. -2 abs: 2
    }
}
