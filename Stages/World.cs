// Stages/World.cs
using Godot;
using System;

public partial class World : Node2D
{
	// By using [Export], you can assign the LordMoto scene
	// directly from the Godot Editor's Inspector.
	[Export]
	public PackedScene PlayerScene { get; set; }

	public override void _Ready()
	{
		// Ensure a player scene has been assigned in the editor.
		if (PlayerScene != null)
		{
			SpawnPlayer();
		}
		else
		{
			GD.PrintErr("PlayerScene has not been set in the World script.");
		}
	}

	private void SpawnPlayer()
	{
		// Create an instance of the player character.
		CharacterBody2D playerInstance = PlayerScene.Instantiate<CharacterBody2D>();

		// Find the spawn point in the scene.
		Marker2D spawnPoint = GetNode<Marker2D>("SpawnPoint");

		// Set the player's position to the spawn point's position.
		playerInstance.Position = spawnPoint.Position;
		
		// Add the player instance to the world scene.
		AddChild(playerInstance);

		GD.Print("Lord Moto has spawned.");
	}
}
