// Common/Items/RuneResource.cs
using Godot;

[GlobalClass]
public partial class RuneResource : Resource
{
    [Export]
    public StatType StatType { get; set; }

    [Export]
    public int Value { get; set; } = 1; // Most runes add 1 point
}