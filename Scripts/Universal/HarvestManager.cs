using Godot;
using System;
using System.Collections.Generic;

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
