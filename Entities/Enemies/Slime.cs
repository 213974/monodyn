// Entities/Enemies/Slime.cs
using Godot;

public partial class Slime : CharacterBody2D
{
	private HealthComponent _healthComponent;
	private LootDropComponent _lootDropComponent;

	public override void _Ready()
	{
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_lootDropComponent = GetNode<LootDropComponent>("LootDropComponent");

		_healthComponent.Died += OnDied;
	}

	// This is called by the HealthComponent when health reaches zero.
	private void OnDied()
	{
		// We need a reference to the player to calculate the dynamic drop.
		var player = GetTree().GetFirstNodeInGroup("player") as LordMoto;
		if (player != null)
		{
			var playerStats = player.GetNode<StatsComponent>("StatsComponent");
			if (playerStats != null)
			{
				_lootDropComponent.OnDeath(this, playerStats);
			}
		}
		
		QueueFree();
	}

	// A temporary way to test killing the enemy. You would call this from an attack script.
	public void Hurt(float damage)
	{
		_healthComponent.TakeDamage(damage);
	}
}
