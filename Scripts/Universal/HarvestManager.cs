using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class HarvestManager : Node
{
    public static List<HarvestSession> ActiveHarvests = new();
    public static List<HarvestSession> HarvestsToRemove { get; set; } = new();

    public static bool CanHarvest(IHarvester Harvester, IHarvestable Harvestee) //Check for sufficient Skill
    {
        foreach (var SkillRequirement in Harvestee.HarvestData.SkillsRequired)
        {
            if (Harvester.GetSkillLevel(SkillRequirement.Skill) >= SkillRequirement.RequiredLevel)
            {
                GD.Print("Rad!, the skillset for " + Harvester.ToString() + "is sufficient!");
            }
            else
            {
                GD.Print("Boooo, this Harvester suuuucks");
                return false;
            }
        }
        foreach (HarvestSession session in ActiveHarvests)  // Test if the node is busy or if the session is duplicate.
        {
            GD.Print(session.Harvester, " -->", session.Harvestee);

            if (session.Harvester == Harvester && session.Harvestee == Harvestee)
                {
                    GD.Print("Blocked Duplicate session.");
                    return false;
                }
                else
                {
                    foreach (HarvestSession Session in ActiveHarvests)
                    {
                        GD.Print(Session.Harvester.ToString(), " -->", Session.Harvestee.ToString());
                    }
                }
        }
        
        return true;
        


    }
    public void CreateSession(IHarvester harvester, IHarvestable harvestee)
    {
        if (CanHarvest(harvester, harvestee))
        {
            GD.Print("Node ", harvester.ToString(), " is attempting to begin harvesting");
            var newHarvest = new HarvestSession
            {
                Harvester = harvester,
                Harvestee = harvestee,
                IsActive = true

            };
            harvester.ProgressBar.Visible = true;
            ActiveHarvests.Add(newHarvest);
        }
        else
        {
            GD.Print("Node ", harvester.ToString(), "was Unable to harvest.");
        }

    }
    //Cancel all Harvests for a Harvester, in case they are stunned, destroyed, etc.
    public void CancelAllSessionsForHarvester(IHarvester harvester)
    {
        foreach (HarvestSession Session in ActiveHarvests)
        {
            if (Session.Harvester == harvester)
            {
                HarvestsToRemove.Add(Session);
                GD.Print("Removing the sessions between ", Session.Harvester.ToString(), " and ", Session.Harvestee);
            }
        }
        CancelSessions(HarvestsToRemove);
        HarvestsToRemove.Clear();

    }
    //Cancel all Harvests for a Harvestee, in case it's depleted, destroyed, etc.
    public static void InactivateAllSessionsForHarvestee(IHarvestable harvestee)
    {
        foreach (HarvestSession Session in ActiveHarvests)
        {
            if (Session.Harvestee == harvestee)
            {
                Session.IsActive = false;
                HarvestsToRemove.Add(Session);
            }
        }      
    }


    //Cancel all Harvest sessions between a Harvester and Harvestee, in case of moving out of range or some other relationship change.
    public void CancelSession(IHarvester harvester, IHarvestable harvestee)
    {
        foreach (HarvestSession Session in ActiveHarvests)
        {
            if (Session.Harvestee == harvestee && Session.Harvester == harvester)
            {
                HarvestsToRemove.Add(Session);
            }
        }
        CancelSessions(HarvestsToRemove);
        HarvestsToRemove.Clear();   
    }


    //Cancel a specific Harvest Session. STUB: How to get the session? Add Guids to each one and return them on create?
    public void CancelSessions(List<HarvestSession> toRemoveList)
    {
        foreach (HarvestSession Session in toRemoveList)
        {
            Session.Harvester.ProgressBar.Visible = false;
            GD.Print("Removed ", Session.Harvester.ToString(), "-->", Session.Harvestee.ToString());
            ActiveHarvests.Remove(Session);
        }
    }

    public void TickHarvests(double delta)
    {

        foreach (HarvestSession Session in ActiveHarvests)
        {

            GD.Print("Active Harvests: ", ActiveHarvests.Count.ToString());
            double tickProgress = GetModifiedHarvestMultiplier(Session.Harvester, Session.Harvestee) * delta;
            if (Session.IsActive == false)
            {
                HarvestsToRemove.Add(Session);
            }
            else
            {
                Session.Progress += tickProgress;
                if (Session.Progress >= 1)
                {
                    Session.Harvestee.YieldHarvest(Session);
                    Session.Progress = 0;
                }
                else
                {
                    if (Session.Harvester.ProgressBar == null) GD.Print("ProgressBar is null when calling update");

                    GD.Print($"Session Harvester: {Session.Harvester}. Session Progress is {Session.Progress}");
                    Session.Harvester.ProgressBar.UpdateProgressBar(Session.Progress, Session.Harvester as Node2D);
                }
            }
        }
        CancelSessions(HarvestsToRemove);
        foreach (HarvestSession Session in HarvestsToRemove)
        {
            if (Session.Harvestee.TimeToDie == true) Session.Harvestee.Die();
        }

    }

    public static double GetModifiedHarvestMultiplier(IHarvester Harvester, IHarvestable Harvestee)
    {
        double DUMMY = 1.0d; //SHIM FOR JUST SO MUCH CALCULATION I'M SO SORRY
                             //return DUMMY * Harvestee.
        GD.Print("Yield Speed is", (Harvestee.HarvestData.BaseYieldSpeed * DUMMY).ToString());
        return DUMMY * Harvestee.HarvestData.BaseYieldSpeed;
    }

    public override void _Process(double delta)
    {
        if (ActiveHarvests.Count > 0)
        {
            TickHarvests(delta);
        }
    }


}
