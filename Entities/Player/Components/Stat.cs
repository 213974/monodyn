// Entities/Player/Components/Stat.cs
using Godot;
using System;

/// <summary>
/// Represents a single character statistic, handling its value and rank progression.
/// The progression system is based on the Reincarnator wiki:
/// - Basic rank is an integer from 0-99.
/// - At 100, the rank advances (e.g., Basic -> Colorless) and the value resets.
/// </summary>
public partial class Stat : RefCounted
{
    public StatRank Rank { get; private set; }
    public int Value { get; private set; }

    public Stat(int baseValue = 10, StatRank baseRank = StatRank.Basic)
    {
        Value = baseValue;
        Rank = baseRank;
    }

    /// <summary>
    /// Adds points to the stat and handles ranking up if the value exceeds 99.
    /// </summary>
    public void AddValue(int amount)
    {
        Value += amount;
        while (Value >= 100)
        {
            // Check if we are already at the highest rank.
            if (Rank >= StatRank.Violet) 
            {
                Value = 99; // Cap at 99 for the highest rank
                break;
            }
            
            Value -= 100;
            Rank++; // Move to the next rank in the enum
        }
    }
}