// Entities/Player/Components/HealthComponent.cs
using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export]
    public float MaxHealth { get; private set; } = 100.0f;
    public float CurrentHealth { get; private set; }

    // C# Signals are the way to go for event-driven logic
    [Signal]
    public delegate void HealthChangedEventHandler(float newHealth);
    [Signal]
    public delegate void DiedEventHandler();

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        EmitSignal(SignalName.HealthChanged, CurrentHealth);

        if (CurrentHealth <= 0)
        {
            EmitSignal(SignalName.Died);
        }
    }
}