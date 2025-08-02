using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class HarvestManager : Node
{
    public List<HarvestSession> ActiveHarvests = new();

    public static bool CanHarvest(IHarvester Harvester, IHarvestable Harvestee)
    {
        foreach (var SkillRequirement in Harvestee.HarvestData.SkillsRequired)
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
    public void CreateSession(IHarvester harvester, IHarvestable harvestee)
    {
        if (CanHarvest(harvester, harvestee))
        {
            var newHarvest = new HarvestSession
            {
                Harvester = harvester,
                Harvestee = harvestee,
                ProgressBar = new HarvestProgressBar()
            };
            ActiveHarvests.Add(newHarvest);
            GD.Print("New Harvest Session!");
            GD.Print(ActiveHarvests.Count);
        }

    }
    //Cancel all Harvests for a Harvester, in case they are stunned, destroyed, etc.
    public void CancelAllSessionsForHarvester(IHarvester harvester)
    {
        ActiveHarvests.RemoveAll(s => s.Harvester == harvester);
        GD.Print("Destroyed Active Sessions");
        GD.Print(ActiveHarvests.Count);
    }
    //Cancel all Harvests for a Harvestee, in case it's depleted, destroyed, etc.
    public void CancelAllSessionsForHarvestee(IHarvestable harvestee)
        => ActiveHarvests.RemoveAll(s => s.Harvestee == harvestee);

    //Cancel all Harvest sessions between a Harvester and Harvestee, in case of moving out of range or some other relationship change.
    public void CancelSession(IHarvester harvester, IHarvestable harvestee)
        => ActiveHarvests.RemoveAll(s => s.Harvester == harvester && s.Harvestee == harvestee);

    //Cancel a specific Harvest Session. STUB: How to get the session? Add Guids to each one and return them on create?
    public void CancelSession(HarvestSession session)
        => ActiveHarvests.Remove(session);

    public void TickHarvests(float delta)
    {
        foreach (HarvestSession Session in ActiveHarvests)
        {
            float tickProgress = HarvestManager.GetModifiedHarvestMultiplier(Session.Harvester, Session.Harvestee) * delta;
            if (CanHarvest(Session.Harvester, Session.Harvestee))
            {

                Session.Progress += tickProgress;
            }
            if (Session.Progress >= 1)
            {
                Session.Harvestee.YieldHarvest(Session.Harvester);
                ActiveHarvests.Remove(Session);
            }
            else
            {
                Session.ProgressBar.UpdateProgressBar(tickProgress, Session.Harvestee as Node2D);
            }
        }
    }

    public static float GetModifiedHarvestMultiplier(IHarvester Harvester, IHarvestable Harvestee)
    {
        int DUMMY = 1; //SHIM FOR JUST SO MUCH CALCULATION I'M SO SORRY
                       //return DUMMY * Harvestee.
        return DUMMY * Harvestee.HarvestData.BaseYieldSpeed;
    }

}
