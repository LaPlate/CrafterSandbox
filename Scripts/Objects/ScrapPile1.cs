using Godot;
using System;

public partial class ScrapPile1 : Node2D, IHarvestable
{
    [Export] public HarvestableData HarvestData { get; set; }

    public void YieldHarvest(IHarvester Harvester)
    {

    }

    public ScrapPile1()
    {

    }
}
