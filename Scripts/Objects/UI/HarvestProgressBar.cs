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


    public void UpdateProgressBar(double CompletedPct, Node2D Caller)
    {
        double computedLength = CompletedPct * NPR.Size.X;
        ProgressBar.Size = new Vector2((float)computedLength, NPR.Size.Y);

        ProgressBar.GlobalPosition = Caller.GlobalPosition + new Vector2(-20, -35);
        NPR.GlobalPosition = Caller.GlobalPosition + new Vector2(-20, -35);
    }
}

