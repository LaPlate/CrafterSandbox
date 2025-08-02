using Godot;
using System;

public partial class HarvestProgressBar : Node2D
{
    public ColorRect ProgressBar;
    public NinePatchRect NPR;

    public override void _Ready()
    {
        ProgressBar = GetNode<ColorRect>("CRCompletedFill");
        NPR = GetNode<NinePatchRect>("9PRBorder");
    }


    public void UpdateProgressBar(float CompletedPct, Node2D Caller)
    {
        float computedLength = CompletedPct * NPR.Size.X;
        ProgressBar.Size = new Vector2(computedLength, NPR.Size.Y);
        ProgressBar.Position = new Vector2(Caller.Position.X, Caller.Position.Y + 5);
    }
}

