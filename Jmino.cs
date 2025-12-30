using Godot;
using System;

public partial class Jmino : Mino
{
 /* [Signal]
  public delegate void DroppedBlockEventHandler();*/
  
  private float _gravity;

  /*public override void _PhysicsProcess(double delta)
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
  }*/

  public override void Init(float gravity, float originDropSpeed, int droppedBlockNum)
  {
	  base.Init(gravity, originDropSpeed, droppedBlockNum);
	  Position = new Vector3(-0.5f, 19, 0);
  }
}
