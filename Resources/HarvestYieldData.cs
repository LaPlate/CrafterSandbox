using Godot;
[GlobalClass]
public partial class HarvestYieldData : Resource
{
    [Export] public Yields.Yield ResourceYielded;
    [Export] public float YieldWeight;

    [Export] public int NumberYielded;
}