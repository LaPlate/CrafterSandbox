using Godot;

public interface IHarvestable
{
    HarvestableData HarvestData { get; set; }
    public void YieldHarvest(IHarvester Harvester);
}