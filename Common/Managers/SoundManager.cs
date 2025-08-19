// Common/Managers/SoundManager.cs
using Godot;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    private AudioStreamPlayer _musicPlayer;
    private Dictionary<string, AudioStream> _sfxCache = new Dictionary<string, AudioStream>();

    public override void _Ready()
    {
        // Create a persistent music player
        _musicPlayer = new AudioStreamPlayer();
        AddChild(_musicPlayer);
    }

    /// <summary>
    /// Plays a piece of music, looping it. Stops any previously playing music.
    /// </summary>
    /// <param name="path">The path to the music file (e.g., "res://Assets/Music/dungeon_theme.ogg").</param>
    public void PlayMusic(string path)
    {
        var stream = ResourceLoader.Load<AudioStream>(path);
        _musicPlayer.Stream = stream;
        _musicPlayer.Play();
    }

    /// <summary>
    /// Plays a one-shot sound effect.
    /// </summary>
    /// <param name="path">The path to the sound effect file (e.g., "res://Assets/Sounds/sword_slash.wav").</param>
    public void PlaySfx(string path)
    {
        if (!_sfxCache.ContainsKey(path))
        {
            _sfxCache[path] = ResourceLoader.Load<AudioStream>(path);
        }

        var sfxPlayer = new AudioStreamPlayer();
        sfxPlayer.Stream = _sfxCache[path];
        AddChild(sfxPlayer);
        sfxPlayer.Play();

        // The player will automatically be removed from the scene once it finishes playing.
        sfxPlayer.Finished += () => sfxPlayer.QueueFree();
    }
}