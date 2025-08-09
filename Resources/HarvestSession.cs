using Godot;

[GlobalClass]
public partial class HarvestSession : Resource
{
    public IHarvester Harvester { get; set; }
    public IHarvestable Harvestee { get; set; }
    public double Progress { get; set; }
}


