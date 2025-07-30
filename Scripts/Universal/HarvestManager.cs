using Godot;
using System;
using System.Collections.Generic;

public partial class HarvestManager : Node
{
    public List<HarvestSession> ActiveHarvests = new();

    public static bool CanHarvest(Node Harvester, Node Harvestee)
    {
        return true; //                                            STUB: Add Check for relevant Skills and Tech
    }
    public void CreateSession(Node harvester, Node harvestee)
    {
        var newHarvest = new HarvestSession
        {
            Harvester = harvester,
            Harvestee = harvestee
        };
    }
    public void CancelAllSessionsForHarvester(Node harvester)
        => ActiveHarvests.RemoveAll(s => s.Harvester == harvester);

    public void CancelAllSessionsForHarvestee(Node harvestee)
        => ActiveHarvests.RemoveAll(s => s.Harvestee == harvestee);

    public void CancelSession(Node harvester, Node harvestee)
        => ActiveHarvests.RemoveAll(s => s.Harvester == harvester && s.Harvestee == harvestee);

    public void CancelSession(HarvestSession session)
        => ActiveHarvests.Remove(session);

}
