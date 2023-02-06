using Godot;
using System;

public partial class MovementControllerClamped : CharacterBody3D
{
    private const float Speed = 5.0f;
    [Export] private PackedScene _projectile;
    private bool _hasFired;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("fire") && !_hasFired)
        {
            GD.Print(_hasFired);
            Fire();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, 0)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void Fire()
    {
        _hasFired = true; 
        Projectile projectile = _projectile.Instantiate<Projectile>();
        Marker3D muzzle = GetNode<Marker3D>("Muzzle");
        Timer coolDown = GetNode<Timer>("Timer");
        projectile.Position = muzzle.GlobalPosition;
        GetTree().Root.GetNode("World").AddChild(projectile);
        coolDown.Start();
    }

    private void OnTimeOut()
    {
        _hasFired = false;
    }
}