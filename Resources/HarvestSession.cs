using Godot;

[GlobalClass]
public partial class HarvestSession : Resource
{
    public Node Harvester { get; set; }
    public Node Harvestee { get; set; }
    public float Progress { get; private set; }  
}
