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
		if (PlayerScene != null)
		{
			SpawnNode(PlayerScene, "SpawnPoint");
		}
		if (SlimeScene != null)
		{
			SpawnNode(SlimeScene, "SlimeSpawnPoint");
		}
	}

	private void SpawnNode(PackedScene scene, string spawnPointName)
	{
		Node2D instance = scene.Instantiate<Node2D>();
		Marker2D spawnPoint = GetNode<Marker2D>(spawnPointName);
		instance.Position = spawnPoint.Position;
		AddChild(instance);
		GD.Print($"{instance.Name} has spawned at {spawnPointName}.");
	}
}
