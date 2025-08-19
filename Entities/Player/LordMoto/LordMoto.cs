// Entities/Player/LordMoto/LordMoto.cs
using Godot;
using System;

public partial class LordMoto : CharacterBody2D
{
	[Export] public float JogSpeed { get; set; } = 150.0f;
	[Export] public float RunSpeed { get; set; } = 300.0f;
	[Export] public float ZoomSpeed { get; set; } = 0.1f;
	[Export] public float MinZoom { get; set; } = 0.8f;
	[Export] public float MaxZoom { get; set; } = 2.5f;

	// Component References
	private AnimatedSprite2D _animatedSprite;
	private HealthComponent _healthComponent;
	private StatsComponent _statsComponent;
	private Camera2D _camera;
	private Timer _attackCooldownTimer;
	private SoundManager _soundManager;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_statsComponent = GetNode<StatsComponent>("StatsComponent");
		_camera = GetNode<Camera2D>("Camera2D");
		_attackCooldownTimer = GetNode<Timer>("AttackCooldownTimer");

		_soundManager = GetNode<SoundManager>("/root/SoundManager");

		_healthComponent.Died += OnDied;
		_healthComponent.HealthChanged += OnHealthChanged;
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed())
		{
			if (mouseEvent.ButtonIndex == MouseButton.WheelUp) ZoomCamera(-ZoomSpeed);
			else if (mouseEvent.ButtonIndex == MouseButton.WheelDown) ZoomCamera(ZoomSpeed);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleInputAndMovement();
		MoveAndSlide();
	}
	
	private void HandleInputAndMovement()
	{
		if (IsAnimationLocked())
		{
			Velocity = Velocity.MoveToward(Vector2.Zero, 300 * (float)GetPhysicsProcessDeltaTime());
			return;
		}

		if (Input.IsActionJustPressed("attack") && _attackCooldownTimer.IsStopped())
		{
			PlayAnimation("Attack");
			_attackCooldownTimer.Start();
			
			_soundManager.PlaySfx("res://Assets/Sounds/SwordSlash.wav");
			
			HurtFirstEnemyInRange();
			return;
		}
		
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		bool isRunning = Input.IsActionPressed("run");
		float targetSpeed = isRunning ? RunSpeed : JogSpeed;
		Velocity = direction * targetSpeed;
		
		UpdateAnimation(direction, isRunning);
	}
	
	private void UpdateAnimation(Vector2 direction, bool isRunning)
	{
		string newAnimation = "Idle";
		if (direction.Length() > 0)
		{
			newAnimation = isRunning ? "Run" : "Jog";
		}
		PlayAnimation(newAnimation);

		Vector2 lookDirection = GetGlobalMousePosition() - GlobalPosition;
		_animatedSprite.FlipH = lookDirection.X < 0;
	}

	private void OnAnimationFinished()
	{
		string anim = _animatedSprite.Animation;
		if (anim == "Attack" || anim == "Hurt")
		{
			PlayAnimation("Idle");
		}
	}
	
	private void OnHealthChanged(float newHealth)
	{
		GD.Print($"Lord Moto was hit! Current Health: {newHealth}/{_healthComponent.MaxHealth}");
		PlayAnimation("Hurt");
	}

	private void OnDied()
	{
		GD.Print("Lord Moto has been defeated.");
		QueueFree();
	}

	private void ZoomCamera(float amount)
	{
		float newZoom = Mathf.Clamp(_camera.Zoom.X - amount, MinZoom, MaxZoom);
		_camera.Zoom = new Vector2(newZoom, newZoom);
	}
	
	private void PlayAnimation(string animName)
	{
		if (_animatedSprite.Animation != animName)
		{
			_animatedSprite.Play(animName);
		}
	}
	
	private bool IsAnimationLocked()
	{
		string anim = _animatedSprite.Animation;
		return anim == "Attack" || anim == "Hurt";
	}

	private void HurtFirstEnemyInRange()
	{
		var enemies = GetTree().GetNodesInGroup("enemies");
		foreach (Node enemy in enemies)
		{
			if (enemy is Slime slime && this.Position.DistanceTo(slime.Position) < 100)
			{
				slime.Hurt(25);
				break;
			}
		}
	}
}
