// Entities/Player/Components/HealthComponent.cs
using Godot;
using System;

public partial class HealthComponent : Node
{
	[Export]
	public float MaxHealth { get; private set; } = 100.0f;
	public float CurrentHealth { get; private set; }

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
		float previousHealth = CurrentHealth;
		CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
		
		// Only print if health actually changed
		if (previousHealth != CurrentHealth)
		{
			// This line adds the damage print for ALL damage types
			GD.Print($"{GetParent().Name} takes {amount.ToString("0.##")} damage.");
		}

		EmitSignal(SignalName.HealthChanged, CurrentHealth);

		if (CurrentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}
}
