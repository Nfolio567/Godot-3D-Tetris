using Godot;
using System;

public partial class OMino : Mino
{
  public override void Init(float gravity, float originDropSpeed, int droppedBlockNum)
  {
		base.Init(gravity, originDropSpeed, droppedBlockNum);
		Position = new Vector3(0, 19, 0);
  }
}
