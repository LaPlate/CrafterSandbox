using Godot;
using System;
using System.Collections.Generic;

public partial class ScrapBot : Bot, IHarvester
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
    [Export]
    public Godot.Collections.Array<ActorSkillSet> SkillMap { get; set; }
    private Dictionary<Skills.SkillTypes, int> SkillDict;
    public PackedScene HarvestProgressBarScene = GD.Load<PackedScene>("res://Scenes/Objects/UI/harvest_progress_bar.tscn");
    
    public HarvestProgressBar ProgressBar { get; set; }

    public override void _Ready()
    {
        SkillDict = new Dictionary<Skills.SkillTypes, int>();
        foreach (var Entry in SkillMap) { SkillDict[Entry.ActorSkill] = Entry.ActorSkillRank; } //Makes it a non-shit dict instead of an array
        GD.Print($"{Name} initialized with ID: {GetInstanceId()}");
        ProgressBar = HarvestProgressBarScene.Instantiate<HarvestProgressBar>();
        AddChild(ProgressBar);

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
                    GetNode<AnimatedSprite2D>("CharacterBody2D/AnimatedSprite2D").Play("Idle");
                    GD.Print($"Attempting to start a harvest for {Name}");
                    currentState = State.Harvesting;
                    {
                        harvestManager.CreateSession(this, sampleNode);
                    }
                    break;
                case State.Harvesting:
                    GD.Print("Toggling Harvest OFF");
                    currentState = State.Idle;
                    GD.Print($"Attempting to cancel all sessions for {Name}");
                    harvestManager.CancelAllSessionsForHarvester(this);
                    break;
            }
        }


    }

}
