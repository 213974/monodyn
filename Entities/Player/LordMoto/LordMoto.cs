// Entities/Player/LordMoto/LordMoto.cs
using Godot;
using System;

public partial class LordMoto : CharacterBody2D
{
	// These speeds will eventually be calculated based on stats from the StatsComponent.
	// For now, they are exported for easy tweaking.
	[Export]
	public float JogSpeed { get; set; } = 150.0f;
	[Export]
	public float RunSpeed { get; set; } = 300.0f;

	// Component References
	private AnimatedSprite2D _animatedSprite;
	private HealthComponent _healthComponent;
	private StatsComponent _statsComponent;

	public override void _Ready()
	{
		// Get references to all the component nodes.
		// This makes the player a "container" for its functionality.
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_statsComponent = GetNode<StatsComponent>("StatsComponent");

		// Connect to the HealthComponent's 'Died' signal using a C# event.
		_healthComponent.Died += OnDied;
	}

	public override void _PhysicsProcess(double delta)
	{
		// For now, we keep movement logic here. 
		// A state machine will manage this later.
		HandleMovement();
	}
	
	private void HandleMovement()
	{
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		bool isRunning = Input.IsActionPressed("run");
		float targetSpeed = isRunning ? RunSpeed : JogSpeed;

		Velocity = direction * targetSpeed;
		MoveAndSlide();
		UpdateAnimation(direction, isRunning);
	}

	private void UpdateAnimation(Vector2 direction, bool isRunning)
	{
		string newAnimation = "Idle";

		if (direction.Length() > 0)
		{
			newAnimation = isRunning ? "Run" : "Jog";
		}
		
		if (_animatedSprite.Animation != newAnimation)
		{
			_animatedSprite.Play(newAnimation);
		}

		// Flip sprite based on horizontal direction
		if (direction.X != 0)
		{
			_animatedSprite.FlipH = direction.X < 0;
		}
	}
	
	/// <summary>
	/// This method is called when the HealthComponent emits the 'Died' signal.
	/// </summary>
	private void OnDied()
	{
		GD.Print("Lord Moto has been defeated.");
		// In the future, this will trigger a death state/animation.
		QueueFree(); // For now, just remove the character from the game.
	}
}
