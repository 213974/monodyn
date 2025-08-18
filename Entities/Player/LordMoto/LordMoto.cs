// Entities/Player/LordMoto/LordMoto.cs
using Godot;
using System;

public partial class LordMoto : CharacterBody2D
{
	[Export]
	public float JogSpeed { get; set; } = 150.0f;
	[Export]
	public float RunSpeed { get; set; } = 300.0f;

	private AnimatedSprite2D _animatedSprite;
	private Vector2 _direction;

	[Signal]
	public delegate void HurtEventHandler();

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleInput();
		UpdateVelocity();
		MoveAndSlide();
		UpdateAnimation();
	}

	private void HandleInput()
	{
		_direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (Input.IsActionJustPressed("attack"))
		{
			PlayAnimation("Attack");
		}
	}

	private void UpdateVelocity()
	{
		// Check if the run action is being held
		bool isRunning = Input.IsActionPressed("run");
		
		// Select speed based on whether the player is running or jogging
		float targetSpeed = isRunning ? RunSpeed : JogSpeed;

		Velocity = _direction * targetSpeed;
	}

	private void UpdateAnimation()
	{
		// Don't interrupt attack or hurt animations
		if (_animatedSprite.Animation == "Attack" || _animatedSprite.Animation == "Hurt")
		{
			return;
		}

		bool isMoving = Velocity.Length() > 0;
		bool isRunning = Input.IsActionPressed("run");

		if (isMoving)
		{
			if (isRunning)
			{
				PlayAnimation("Run");
			}
			else
			{
				PlayAnimation("Jog");
			}
		}
		else
		{
			PlayAnimation("Idle");
		}

		// Flip the sprite based on horizontal movement direction
		if (_direction.X != 0)
		{
			_animatedSprite.FlipH = _direction.X < 0;
		}
	}

	// Helper function to prevent restarting the same animation every frame
	private void PlayAnimation(string animName)
	{
		if (_animatedSprite.Animation != animName)
		{
			_animatedSprite.Play(animName);
		}
	}

	public void TakeDamage()
	{
		PlayAnimation("Hurt");
		EmitSignal(SignalName.Hurt);
	}
}
