using Godot;
using Vector3 = Godot.Vector3;

public partial class Mino : CharacterBody3D
{
  [Signal]
  public delegate void DroppedBlockEventHandler();

  [Export] public int MovementSpeed = 20;
  [Export] public int LockDelayLimit = 3;

  private float _gravity;
  private float _dropSpeed;
  private float _blockVelocityX;

  private bool _isMoving;

  private float _beforePosition;

  private bool _emitedSignal;

  private int _lockDelayExtensionsNum;

  public override void _PhysicsProcess(double delta)
  {
    var relativeVelocity = Vector3.Zero;
    
    if (IsOnFloor() && !_emitedSignal)
    {
      EmitSignalDroppedBlock();
      _emitedSignal = true;
    }
    else
    {
      relativeVelocity.Y -= _gravity * (float)delta * _dropSpeed;
    }

    if (!_isMoving)
    {
      if (Input.IsActionJustPressed("move_left") && !(Position.X == -3.5f))
      {
        AddLockDelayExtensionsNum();
        _blockVelocityX += BothSidesMovement(_blockVelocityX, 'L');
      }

      if (Input.IsActionJustPressed("move_right") && !(Position.X == 3.5f))
      {
        AddLockDelayExtensionsNum();
        _blockVelocityX += BothSidesMovement(_blockVelocityX, 'R');
      }
    }

    if (Input.IsActionJustPressed("block_rotate"))
    {
      AddLockDelayExtensionsNum();
      Rotation += new Vector3(0, 0, -Mathf.Pi / 2);
    }

    var totalDistanceTraveled = Position.X - _beforePosition;
    if (totalDistanceTraveled is >= 1 or <= -1)
    {
      _blockVelocityX = 0;
      Position = new Vector3(_beforePosition + (int)totalDistanceTraveled, Position.Y, Position.Z);
      _isMoving = false;
    }

    relativeVelocity.X = _blockVelocityX * MovementSpeed;
    
    Velocity = relativeVelocity;

    /*GD.Print(relativeVelocity.X);
    GD.Print(_isMoving);
    GD.Print(_beforePosition);
    GD.Print(Position.X);*/
    
    MoveAndSlide();
  }

  private float BothSidesMovement(float velocityX, char side)
  {
    _isMoving = true;
    //GD.Print("in");
    _beforePosition = Position.X;
    switch (side)
    {
      case 'L' :
        velocityX += -1.0f;
        break;
      
      case 'R' :
        velocityX += 1.0f;
        break;
    }

    return velocityX;
  }

  private void AddLockDelayExtensionsNum()
  {
    if (_lockDelayExtensionsNum < LockDelayLimit)
    {
      _lockDelayExtensionsNum++;
    }
  }

  public void LockDelay(Timer lockDelay)
  {
    if (_lockDelayExtensionsNum > 0)
    {
      lockDelay.Start();
    }
    else
    {
      var staticBlock = new StaticBody3D();

      staticBlock.AddChild();
    }
  }

  public virtual void Init(float gravity, float originDropSpeed)
  {
    _dropSpeed = originDropSpeed;
    _gravity = gravity;
    Name = "ActiveMino";
  }
}
