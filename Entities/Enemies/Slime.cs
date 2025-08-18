// Entities/Enemies/Slime.cs
using Godot;

public partial class Slime : CharacterBody2D
{
	private HealthComponent _healthComponent;
	private LootDropComponent _lootDropComponent;
	private AnimatedSprite2D _animatedSprite;
	private bool _isDead = false;
	private bool _isSpawning = true;

	public override async void _Ready()
	{
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_lootDropComponent = GetNode<LootDropComponent>("LootDropComponent");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		_healthComponent.Died += OnDied;
		_healthComponent.HealthChanged += OnHealthChanged;
		
		PlayAnimation("Summoned");
		await ToSignal(_animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);
		_isSpawning = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isDead || _isSpawning) return;

		if (Velocity.Length() > 0) PlayAnimation("Move");
		else PlayAnimation("Idle");
		
		if (Velocity.X != 0) _animatedSprite.FlipH = Velocity.X < 0;
	}

	private void OnHealthChanged(float newHealth)
	{
		if (_isDead || _isSpawning) return;
		PlayAnimation("Damaged");
	}

	private async void OnDied()
	{
		_isDead = true;
		GetNode<SlimeAI>("SlimeAI").SetProcess(false);
		Velocity = Vector2.Zero;
		
		PlayAnimation("Die");
		
		await ToSignal(_animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);

		GD.Print("Slime has died.");
		var player = GetTree().GetFirstNodeInGroup("player") as LordMoto;
		if (player != null)
		{
			var playerStats = player.GetNode<StatsComponent>("StatsComponent");
			if (playerStats != null)
			{
				// Get the result from the loot component
				var droppedRune = _lootDropComponent.OnDeath(this, playerStats);
				if (droppedRune != null)
				{
					GD.Print($"It dropped a {droppedRune.StatType} rune!");
				}
				else
				{
					GD.Print("It dropped nothing.");
				}
			}
		}
		
		QueueFree();
	}

	public void Hurt(float damage)
	{
		_healthComponent.TakeDamage(damage);
	}
	
	private void PlayAnimation(string animName)
	{
		if (_animatedSprite.Animation != animName)
		{
			_animatedSprite.Play(animName);
		}
	}
}
