// Common/Components/LootDropComponent.cs
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class LootDropComponent : Node
{
    [Export]
    private PackedScene _runeDropScene;

    [Export(PropertyHint.Range, "0,1")]
    private float _baseDropChance = 0.5f;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    // This method now returns the rune that was created, or null.
    public RuneResource OnDeath(Node owner, StatsComponent playerStats)
    {
        if (_rng.Randf() > _baseDropChance)
        {
            return null; // No drop this time
        }

        var lowestRank = playerStats.AllStats.Values.Min(s => s.Rank);
        var lowestRankStats = playerStats.AllStats.Where(kvp => kvp.Value.Rank == lowestRank).ToList();
        var lowestValue = lowestRankStats.Min(kvp => kvp.Value.Value);
        var candidates = lowestRankStats.Where(kvp => kvp.Value.Value == lowestValue).Select(kvp => kvp.Key).ToList();
        StatType droppedRuneType = candidates[_rng.RandiRange(0, candidates.Count - 1)];
        
        if (_runeDropScene != null && owner is Node2D owner2D)
        {
            var runeInstance = _runeDropScene.Instantiate<Node2D>();
            var runeData = new RuneResource { StatType = droppedRuneType, Value = 1 };
            runeInstance.Set("RuneData", runeData);

            owner.GetParent().AddChild(runeInstance);
            runeInstance.GlobalPosition = owner2D.GlobalPosition;
            
            // Return the data of the dropped rune
            return runeData;
        }

        return null;
    }
}