using System;
using Godot;

namespace Fish;

public partial class Fish : CharacterBody3D
{
    public Vector3 Forward = Vector3.Right;
    public float DirectionDuration = 5.0f;
    private readonly float _speed = Random.Shared.NextSingle() + 0.5f;
    private const float Angle = 0.1f;

    [Export] public Node3D WorldNode;
    [Export] public bool IsCameraFish;

    private Label3D _labelForward;
    private Label3D _labelDistance;
    private Label3D _labelCurrentTime;
    private float _currentTime;

    public override void _Ready()
    {
        base._Ready();
        _currentTime = DirectionDuration;
        _labelForward = GetNode<Label3D>("./LabelForward");
        _labelDistance = GetNode<Label3D>("./LabelDistance");
        _labelCurrentTime = GetNode<Label3D>("./LabelCurrentTime");

        if (!IsCameraFish)
        {
            _labelForward.Hide();
            _labelDistance.Hide();
            _labelCurrentTime.Hide();
        }

        var planetCentre = WorldNode.GlobalPosition;
        var planetVector = (GlobalPosition - planetCentre);
        LookAt((GlobalPosition + Forward), planetVector);
    }

    public override void _PhysicsProcess(double delta)
	{
        var planetCentre = WorldNode.GlobalPosition;
        var planetVector = (GlobalPosition - planetCentre);

        _currentTime -= (float)delta;
        if (_currentTime < 0.0f)
        {
            _currentTime = DirectionDuration;
            Forward = Forward.Rotated(planetVector.Normalized(), Random.Shared.NextSingle() * Mathf.Tau);
        }

        var rotationAxis = planetVector.Cross(Forward).Normalized();
        var newVelocity = planetVector.Rotated(rotationAxis, Angle) * _speed;
        var offset = newVelocity * (float)delta;
        var newPos = (GlobalPosition + offset);

        var posOnCircle = ((newPos - planetCentre).Normalized() * planetVector.Length()) + planetCentre;
        var correctedOffset = posOnCircle - GlobalPosition;

        var newPlanetVector = posOnCircle - planetCentre;
        UpDirection = newPlanetVector;

        Forward = newPlanetVector.Cross(Forward).Cross(newPlanetVector).Normalized();
        LookAt((GlobalPosition + Forward), planetVector);

        if (IsCameraFish)
        {
            _labelCurrentTime.Text = $"{_currentTime:F}";
            _labelForward.Text = ForwardLabelText(ref Forward);
            _labelDistance.Text = $"{newPlanetVector.Length():F}";
        }

        MoveAndCollide(correctedOffset);
    }

    private static string ForwardLabelText(ref Vector3 vec)
        => $"({vec.X:F}, {vec.Y:F}, {vec.Z:F})";
}
