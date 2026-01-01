using Godot;
using System.Collections.Generic;

public partial class AudioVisualizer : Node2D
{
	[Export] public float PointsNum = 64;
	[Export] public int MaxHertz = 20000;
	
	private AudioEffectSpectrumAnalyzerInstance _analyzer;

	private List<List<Line2D>> _lines;

	private List<float> _originPointsX;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_analyzer = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(
			AudioServer.GetBusIndex("BGM"),
			0
		);

		var viewportSize = GetViewport().GetVisibleRect().Size;
		var pointY = viewportSize.Y - viewportSize.Y / PointsNum;
		var leftLines = new List<Line2D>();
		var rightLines = new List<Line2D>();

		_originPointsX = new List<float> { 5, viewportSize.X - 5 };

		var originLineWidth = 10;
		for (var i = 0; i < PointsNum; i++)
		{
			pointY -= viewportSize.Y / PointsNum;
			var newLeftLine = new Line2D();
			leftLines.Add(newLeftLine);
			newLeftLine.Width = originLineWidth * 64 / PointsNum;
			newLeftLine.AddPoint(new Vector2(-50, pointY));
			newLeftLine.AddPoint(new Vector2(_originPointsX[0], pointY));

			var newRightLine = new Line2D();
			rightLines.Add(newRightLine);
			newRightLine.Width = originLineWidth * 64 / PointsNum;
			newRightLine.AddPoint(new Vector2(viewportSize.X + 50, pointY));
			newRightLine.AddPoint(new Vector2(_originPointsX[1], pointY));

			var color = Color.FromHsv(360.0f * (i / PointsNum), 1.0f, 1.0f);
			newLeftLine.DefaultColor = color;
			newRightLine.DefaultColor = color;
			GD.Print(360.0f * (i / PointsNum));
			
			AddChild(newLeftLine);
			AddChild(newRightLine);
		}

		_lines = new List<List<Line2D>> { leftLines, rightLines };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var spectrums = new List<Vector2>((int)PointsNum);
		var minRange = 0.0f;
		var maxRange = MaxHertz / PointsNum;

		for (var i = 0; i < PointsNum; i++)
		{
			spectrums.Add(_analyzer.GetMagnitudeForFrequencyRange(minRange, maxRange));
			minRange = maxRange + 1;
			maxRange += MaxHertz / PointsNum;
		}

		for (var i = 0; i < spectrums.Count; i++)
		{
			_lines[0][i].SetPointPosition(
				1,
				new Vector2(
					_originPointsX[0] + spectrums[i].X * 1500,
					_lines[0][i].GetPointPosition(0).Y
				)
			);

			_lines[1][i].SetPointPosition(
				1,
				new Vector2(
					_originPointsX[1] - spectrums[i].Y * 1500,
					_lines[1][i].GetPointPosition(0).Y
				)
			);
		}
	}
}
