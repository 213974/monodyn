// Common/StatusEffects/StatusEffectResource.cs
using Godot;

// An enum to identify different effects if needed later
public enum EffectType { Poison, Fire }

[GlobalClass]
public partial class StatusEffectResource : Resource
{
	[Export]
	public EffectType Type { get; set; }

	[Export(PropertyHint.Range, "0, 60, 0.1")]
	public float Duration { get; set; } = 2.0f; // Total time the effect lasts

	[Export(PropertyHint.Range, "0, 10, 0.1")]
	public float TickInterval { get; set; } = 1.0f; // Time between each damage tick

	[Export]
	public float DamagePerTick { get; set; } = 2.5f; // Base damage per tick

	[Export]
	public bool IsPercentageBased { get; set; } = true; // Is damage a % of max health?

	// If IsPercentageBased is true, DamagePerTick is treated as a percentage (e.g., 2.5 = 2.5%)
}
