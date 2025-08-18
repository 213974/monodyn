// Entities/Enemies/SlimeAI.cs
using Godot;

public partial class SlimeAI : Node
{
    [Export]
    public float MoveSpeed { get; set; } = 50.0f;

    private CharacterBody2D _body;
    private CharacterBody2D _player;

    public override void _Ready()
    {
        _body = GetParent<CharacterBody2D>();
        _player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsInstanceValid(_player))
        {
            Vector2 direction = _body.GlobalPosition.DirectionTo(_player.GlobalPosition);
            _body.Velocity = direction * MoveSpeed;
            _body.MoveAndSlide();
        }
        else
        {
            _body.Velocity = Vector2.Zero; // Stop if the player is gone
        }
    }
}