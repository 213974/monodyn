// Common/Items/RuneDrop.cs
using Godot;

public partial class RuneDrop : Area2D
{
    [Export]
    public RuneResource RuneData { get; set; }

    public override void _Ready()
    {
        // Connect the body_entered signal to our pickup method
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // Check if the body that entered is the player and has a stats component
        if (body is LordMoto player)
        {
            var statsComponent = player.GetNode<StatsComponent>("StatsComponent");
            if (statsComponent != null && RuneData != null)
            {
                statsComponent.ApplyRune(RuneData.StatType, RuneData.Value);
                QueueFree(); // Destroy the rune on the ground after pickup
            }
        }
    }
}