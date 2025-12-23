using Godot;

public partial class Mino : CharacterBody3D
{
  [Signal]
  public delegate void DroppedBlockEventHandler();

  private float _gravity;

  public override void _PhysicsProcess(double delta)
  {
    if (IsOnFloor())
    {
      EmitSignalDroppedBlock();
    }
    else
    {
      Velocity += Vector3.Down * _gravity * (float)delta;
    }

    MoveAndSlide();
  }

  public virtual void Init(float gravity)
  {
    _gravity = gravity;
  }
}
