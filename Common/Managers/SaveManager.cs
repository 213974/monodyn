// Common/Managers/SaveManager.cs
using Godot;
using System.Text.Json;

// A simple data container to hold all the information we want to save.
public class PlayerData
{
    public float CurrentHealth { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    // You can add stats, inventory, etc., here later.
}

public partial class SaveManager : GodotObject
{
    private const string SavePath = "user://savegame.json";

    public void SaveGame(PlayerData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(data, options);
        
        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
        file.StoreString(jsonString);
        GD.Print("Game saved successfully.");
    }

    public PlayerData LoadGame()
    {
        if (!FileAccess.FileExists(SavePath))
        {
            GD.Print("No save file found.");
            return null;
        }

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
        string jsonString = file.GetAsText();
        
        var data = JsonSerializer.Deserialize<PlayerData>(jsonString);
        GD.Print("Game loaded successfully.");
        return data;
    }
}