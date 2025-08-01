using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class HarvestManager : Node
{
    public List<HarvestSession> ActiveHarvests = new();

    public static bool CanHarvest(IHarvester Harvester, IHarvestable Harvestee)
    {
        foreach (var SkillRequirement in Harvestee.HarvestSkillsRequired)
        {
            if (Harvester.GetSkillLevel(SkillRequirement.Skill) >= SkillRequirement.RequiredLevel)
            {
                GD.Print("Rad!, the skillset for " + Harvester.ToString() + "is sufficient!");
                return true;
            }
            else
            {
                GD.Print("Boooo, this Harvester suuuucks");
                return false;
            }
        }
        GD.Print("Got this far with no skill match? That's nuts.");
        return false;

    }
    public void CreateSession(Node harvester, Node harvestee)
    {
        var newHarvest = new HarvestSession
        {
            Harvester = harvester,
            Harvestee = harvestee
        };
        ActiveHarvests.Add(newHarvest);
        GD.Print("Created a new Session");
        ActiveHarvests.Add(newHarvest);
        GD.Print("New Harvest Session!");
        GD.Print(ActiveHarvests.Count);
    }
    //Cancel all Harvests for a Harvester, in case they are stunned, destroyed, etc.
    public void CancelAllSessionsForHarvester(Node harvester)
    {
        ActiveHarvests.RemoveAll(s => s.Harvester == harvester);
        GD.Print("Destroyed Active Sessions");
        GD.Print(ActiveHarvests.Count);
    }
    //Cancel all Harvests for a Harvestee, in case it's depleted, destroyed, etc.
    public void CancelAllSessionsForHarvestee(Node harvestee)
        => ActiveHarvests.RemoveAll(s => s.Harvestee == harvestee);

    //Cancel all Harvest sessions between a Harvester and Harvestee, in case of moving out of range or some other relationship change.
    public void CancelSession(Node harvester, Node harvestee)
        => ActiveHarvests.RemoveAll(s => s.Harvester == harvester && s.Harvestee == harvestee);

    //Cancel a specific Harvest Session. STUB: How to get the session? Add Guids to each one and return them on create?
    public void CancelSession(HarvestSession session)
        => ActiveHarvests.Remove(session);

}
