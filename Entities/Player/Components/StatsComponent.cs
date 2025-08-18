// Entities/Player/Components/StatsComponent.cs
using Godot;
using System;
using System.Collections.Generic;

// The defined progression of stat ranks/colors.
public enum StatRank
{
    Basic,
    Colorless,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}

// Enum to identify each stat type, crucial for the dynamic rune drop system.
public enum StatType
{
    Strength,
    Stamina,
    Agility,
    Perception,
    Magic,
    Mana,
    PhysicalResistance,
    MagicalResistance,
    SixthSense,
    Luck
}

public partial class StatsComponent : Node
{
    [Signal]
    public delegate void StatsChangedEventHandler();

    // We use a dictionary to easily access stats by their type.
    // This is vital for the dynamic loot system.
    public Dictionary<StatType, Stat> AllStats { get; private set; } = new Dictionary<StatType, Stat>();

    // Player Currency
    public int SoulEssence { get; private set; } = 0;

    public override void _Ready()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        // --- Standard Stats ---
        // These stats start with an average human value of 10.
        AllStats[StatType.Strength] = new Stat(10);    // Enhances power and speed
        AllStats[StatType.Stamina] = new Stat(10);     // Enhances endurance and recovery speed
        AllStats[StatType.Agility] = new Stat(10);     // Enhances reaction time
        AllStats[StatType.Perception] = new Stat(10);  // Enhances the five senses
        AllStats[StatType.Magic] = new Stat(10);        // Enhances a skill's power
        AllStats[StatType.Mana] = new Stat(10);        // Enhances quantity of mana for skill usage

        // --- Special Initialization Stats ---

        // Physical and Magical Resistance must sum to 10.
        var rng = new RandomNumberGenerator();
        int physicalRes = rng.RandiRange(1, 9);
        int magicalRes = 10 - physicalRes;
        AllStats[StatType.PhysicalResistance] = new Stat(physicalRes); // Reduces incoming physical damage
        AllStats[StatType.MagicalResistance] = new Stat(magicalRes);   // Reduces incoming magical damage

        // Sixth Sense and Luck must start at 1.
        AllStats[StatType.SixthSense] = new Stat(1);   // Allows one to sense things others cannot
        AllStats[StatType.Luck] = new Stat(1);         // Increases drop rate of rare runes, skills, and items
        
        GD.Print("Player stats initialized.");
    }
    
    /// <summary>
    /// Public method to apply a rune to the player's stats.
    /// </summary>
    public void ApplyRune(StatType statToUpgrade, int value)
    {
        if (AllStats.ContainsKey(statToUpgrade))
        {
            AllStats[statToUpgrade].AddValue(value);
            EmitSignal(SignalName.StatsChanged);
            GD.Print($"Applied Rune: {statToUpgrade} +{value}. New Value: {AllStats[statToUpgrade].Value} [{AllStats[statToUpgrade].Rank}]");
        }
    }

    public void AddSoulEssence(int amount)
    {
        SoulEssence += amount;
        EmitSignal(SignalName.StatsChanged);
    }
}