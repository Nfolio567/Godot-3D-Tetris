using Godot;
using System;

public partial class Main : Node
{
	[Export] public PackedScene JMinoScene { get; set; }
	[Export] public float Gravity = 1.0f;
	
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
		
		jMino.Init(Gravity);
		
		AddChild(jMino);
	}

	public void NewGame()
	{
		GetNode<AudioStreamPlayer>("MainBGM").Play();
	}
}
