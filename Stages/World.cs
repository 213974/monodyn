// Stages/World.cs
using Godot;
using System;

public partial class World : Node2D
{
	[Export]
	public PackedScene PlayerScene { get; set; }
	[Export]
	public PackedScene SlimeScene { get; set; } 

	public override void _Ready()
	{
		// Spawn the player (still only one)
		if (PlayerScene != null)
		{
			SpawnSingleNode(PlayerScene, "SpawnPoint", "LordMoto");
		}

		// Spawn slimes at all designated spawn points
		if (SlimeScene != null)
		{
			SpawnGroupNodes(SlimeScene, "slime_spawn_points", "Slime");
		}
	}

	// Spawns one instance at a single, named node
	private void SpawnSingleNode(PackedScene scene, string spawnPointName, string nodeName)
	{
		Node2D instance = scene.Instantiate<Node2D>();
		instance.Name = nodeName;
		
		Marker2D spawnPoint = GetNode<Marker2D>(spawnPointName);
		instance.Position = spawnPoint.Position;
		AddChild(instance);
		PrintSpawnMessage(instance);
	}

	// Spawns instances at every node belonging to a group
	private void SpawnGroupNodes(PackedScene scene, string groupName, string baseName)
	{
		var spawnPoints = GetTree().GetNodesInGroup(groupName);
		int count = 1;
		foreach (Node spawnPoint in spawnPoints)
		{
			if (spawnPoint is Marker2D marker)
			{
				Node2D instance = scene.Instantiate<Node2D>();
				instance.Name = $"{baseName}{count++}"; // Name them Slime1, Slime2, etc.
				instance.Position = marker.Position;
				AddChild(instance);
				PrintSpawnMessage(instance);
			}
		}
	}

	private void PrintSpawnMessage(Node2D instance)
	{
		var healthComponent = instance.GetNode<HealthComponent>("HealthComponent");
		if (healthComponent != null)
		{
			GD.Print($"{instance.Name} has spawned with {healthComponent.CurrentHealth}/{healthComponent.MaxHealth} HP.");
		}
		else
		{
			GD.Print($"{instance.Name} has spawned.");
		}
	}
}
