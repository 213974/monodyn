// Common/Components/StatusApplicatorComponent.cs
using Godot;

public partial class StatusApplicatorComponent : Area2D
{
	[Export]
	public StatusEffectResource EffectToApply { get; set; }

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (EffectToApply == null) return;

		// Find the StatusEffectComponent on the body that entered
		var statusComponent = body.GetNode<StatusEffectComponent>("StatusEffectComponent");
		if (statusComponent != null)
		{
			statusComponent.ApplyEffect(EffectToApply);
		}
	}
}
