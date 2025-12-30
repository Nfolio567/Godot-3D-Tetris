using Godot;
using System;

public partial class Main : Node
{
	[Export] public PackedScene JMinoScene { get; set; }
	[Export] public float Gravity = 20.0f;
	[Export] public float OriginDropSpeed = 5;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnBlockTimerTimeout()
	{
		var jMino = JMinoScene.Instantiate<Jmino>();
		jMino.DroppedBlock += OnBlockDropped;
		
		jMino.Init(Gravity, OriginDropSpeed);
		
		AddChild(jMino);
	}

	public void NewGame()
	{
		GetNode<AudioStreamPlayer>("MainBGM").Play();
	}

	private void OnBlockDropped()
	{
		var lockDelay = GetNode<Timer>("LockDelay");
		lockDelay.Start();
		lockDelay.Timeout += OnLockDelayTimerTimeout;
	}

	private void OnLockDelayTimerTimeout()
	{
		GetNode<Mino>("ActiveMino").LockDelay(GetNode<Timer>("LockDelay"));
	}
}
