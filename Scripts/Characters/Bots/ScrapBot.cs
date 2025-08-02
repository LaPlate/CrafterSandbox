using Godot;
using System;
using System.Collections.Generic;

public partial class ScrapBot : Node2D, IHarvester
{
    private enum State
    {
        Idle,
        Harvesting
    }
    State currentState = State.Idle;

    [Export]
    public HarvestManager harvestManager;
    [Export]
    public ScrapPile1 sampleNode;

    public override void _Ready()
    {
        SkillDict = new Dictionary<Skills.SkillTypes, int>();
        foreach (var Entry in SkillMap) { SkillDict[Entry.ActorSkill] = Entry.ActorSkillRank; } //Makes it a non-shit dict instead of an array
    }

    public int GetSkillLevel(Skills.SkillTypes skill)
    {
        return SkillDict.TryGetValue(skill, out int level) ? level : 0; //Returns 0 if the skill doesn't exist.
    }


    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Toggle_Test_Action"))
        {
            switch (currentState)
            {
                case State.Idle:
                    GD.Print("Toggling Harvest ON");
                    currentState = State.Harvesting;
                    {
                        harvestManager.CreateSession(this, sampleNode);
                    }
                    break;
                case State.Harvesting:
                    GD.Print("Toggling Harvest OFF");
                    currentState = State.Idle;
                    harvestManager.CancelAllSessionsForHarvester(this);
                    break;
            }
        }


    }

    [Export] public Godot.Collections.Array<ActorSkillSet> SkillMap { get; set; }
    private Dictionary<Skills.SkillTypes, int> SkillDict;
}
