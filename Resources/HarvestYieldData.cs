using Godot;
[GlobalClass]
public partial class HarvestYieldData : Resource
{
    [Export] public Godot.Collections.Array<Yields.Yield> ResourceYielded;
    [Export] public float YieldWeight;
}