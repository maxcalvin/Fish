using System;
using System.Linq;
using Godot;

namespace Fish;

public partial class World : Node3D
{
    private PackedScene _fishScene;
    private Fish[] _fish;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

        _fishScene = GD.Load<PackedScene>("res://Fish.tscn");

        // Spawn more fish!
        _fish = Enumerable.Repeat(0, 100)
            .Select(_ => CreateFish())
            .ToArray();

        foreach (var fish in _fish)
        {
            AddChild(fish);
        }
    }

    private Fish CreateFish()
    {
        var fish = (Fish)_fishScene.Instantiate();
        fish.WorldNode = this;
        fish.Forward = RandomVector();
        fish.DirectionDuration = NextSingle() * 10;
        fish.GlobalPosition = RandomPosition();

        return fish;
    }

    private static Vector3 RandomVector()
        => new Vector3(NegOneToOneSingle(), NegOneToOneSingle(), NegOneToOneSingle()).Normalized();

    private Vector3 RandomPosition()
    {
        // Start location is based on world location
        var pole = new Vector3(0, 51 + NextSingle() * 5, 0);
        var randomPosition = pole.Rotated(RandomVector(), Mathf.Tau * NextSingle());

        return randomPosition + GlobalPosition;
    }

    private static float NegOneToOneSingle()
        => (NextSingle() * 2) - 1;

    private static float NextSingle()
        => Random.Shared.NextSingle();
}
