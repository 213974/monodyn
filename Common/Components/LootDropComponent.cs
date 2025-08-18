// Common/Components/LootDropComponent.cs
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class LootDropComponent : Node
{
    [Export]
    private PackedScene _runeDropScene; // The scene for the rune on the ground

    [Export(PropertyHint.Range, "0,1")]
    private float _baseDropChance = 0.5f; // 50% chance to drop any rune

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public void OnDeath(Node owner, StatsComponent playerStats)
    {
        if (_rng.Randf() > _baseDropChance)
        {
            return; // No drop this time
        }

        // --- Dynamic Balancing Logic ---
        // Find the player's lowest-ranked stats to prioritize them.
        var lowestRank = playerStats.AllStats.Values.Min(s => s.Rank);
        var lowestRankStats = playerStats.AllStats
            .Where(kvp => kvp.Value.Rank == lowestRank)
            .ToList();
        
        // From those, find the lowest value.
        var lowestValue = lowestRankStats.Min(kvp => kvp.Value.Value);
        var candidates = lowestRankStats
            .Where(kvp => kvp.Value.Value == lowestValue)
            .Select(kvp => kvp.Key)
            .ToList();

        // Randomly pick one of the lowest stats to drop a rune for.
        StatType droppedRuneType = candidates[_rng.RandiRange(0, candidates.Count - 1)];
        
        // --- Instancing the Drop ---
        if (_runeDropScene != null && owner is Node2D owner2D)
        {
            var runeInstance = _runeDropScene.Instantiate<Node2D>();
            
            // Create the rune data and pass it to the instanced scene
            var runeData = new RuneResource { StatType = droppedRuneType, Value = 1 };
            runeInstance.Set("RuneData", runeData);

            owner.GetParent().AddChild(runeInstance);
            runeInstance.GlobalPosition = owner2D.GlobalPosition;
            GD.Print($"Dropped a {droppedRuneType} rune!");
        }
    }
}