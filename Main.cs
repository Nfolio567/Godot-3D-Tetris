using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
	[Export] public PackedScene JMinoScene { get; set; }
	[Export] public PackedScene LMinoScene { get; set; }
	[Export] public PackedScene SMinoScene { get; set; }
	[Export] public PackedScene ZMinoScene { get; set; }
	[Export] public PackedScene OMinoScene { get; set; }
	[Export] public PackedScene TMinoScene { get; set; }
	[Export] public PackedScene IMinoScene { get; set; }
	
	[Export] public float Gravity = 20.0f;
	[Export] public float OriginDropSpeed = 5;

	private int _droppedBlockNum;

	private List<PackedScene> Minos;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
		Minos = new List<PackedScene>
			{ JMinoScene, LMinoScene, SMinoScene, ZMinoScene, OMinoScene, TMinoScene, IMinoScene };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnBlockTimerTimeout()
	{
		var rand = new Random();
		var mino = Minos[rand.Next(Minos.Count)].Instantiate<Mino>();
		mino.DroppedBlock += OnBlockDropped;
		
		mino.Init(Gravity, OriginDropSpeed, _droppedBlockNum);
		
		AddChild(mino);
	}

	public void NewGame()
	{
		GetNode<AudioStreamPlayer>("MainBGM").Play();
	}

	private void OnBlockDropped()
	{
		var lockDelay = GetNode<Timer>("LockDelay");
		lockDelay.Start();
	}

	private void OnLockDelayTimerTimeout()
	{
		GetNode<Mino>("ActiveMino").LockDelay(GetNode<Timer>("LockDelay"));
	}

	public void OnActiveBlockTreeExited(StaticBody3D staticBlock)
	{
		AddChild(staticBlock);
		_droppedBlockNum++;
	}
}
