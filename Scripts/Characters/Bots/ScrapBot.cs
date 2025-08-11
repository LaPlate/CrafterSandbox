using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public partial class ScrapBot : Bot, IHarvester
{
    private enum State
    {
        Idle,
        Harvesting,
        Searching,
        Approaching
    }
    State currentState = State.Idle;

    [Export]
    public HarvestManager harvestManager;
    public ScrapPile1 targetNode;
    [Export]
    public Godot.Collections.Array<ActorSkillSet> SkillMap { get; set; }
    private Dictionary<Skills.SkillTypes, int> SkillDict;
    public PackedScene HarvestProgressBarScene = GD.Load<PackedScene>("res://Scenes/Objects/UI/harvest_progress_bar.tscn");
    public bool isApproaching;
    public IHarvestable HarvestTarget;

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
            GD.Print("Acting on State ", currentState.ToString());
            switch (currentState)
            {
                case State.Idle:
                    SearchForHarvestable();
                    break;
                case State.Harvesting:
                    BeginHarvesting();
                    break;
                case State.Searching:
                    SearchForHarvestable();
                    break;
                case State.Approaching:
                    GD.Print("Entered Approaching State");
                    if (!isApproaching)
                    {
                        _ = ApproachTarget(HarvestTarget as Node2D); // Explicitly Fire and Forget!
                    }
                    break;


            }
        }


    }

    public void BeginHarvesting()
    {
        GetNode<AnimatedSprite2D>("CharacterBody2D/AnimatedSprite2D").Play("Idle");
        GD.Print($"Attempting to start a harvest for {Name}");
        currentState = State.Harvesting;
        {
            GD.Print($"{Name} is Creating a Session with {HarvestTarget.ToString()}");
            harvestManager.CreateSession(this, HarvestTarget);
        }
    }

    public void StopHarvesting()
    {
        GD.Print("Toggling Harvest OFF");
        currentState = State.Idle;
        GD.Print($"Attempting to cancel all sessions for {Name}");
        harvestManager.CancelAllSessionsForHarvester(this);
    }

    public void SearchForHarvestable()
    {
        HarvestTarget = null;
        var shape = new CircleShape2D { Radius = 200 };
        var query = new PhysicsShapeQueryParameters2D
        {
            Shape = shape,
            Transform = GlobalTransform,
            CollisionMask = 1 << 1
        };

        var worldScan = GetWorld2D().DirectSpaceState;

        var scanResult = worldScan.IntersectShape(query);
        var harvestableTargets = scanResult
        .Select(result => result["collider"].AsGodotObject())
        .OfType<IHarvestable>()
        .ToList();

        List<IHarvestable> targetsVisible = new();
        foreach (var target in harvestableTargets)
        {
            if (PhysicsInteractions2D.HasLineOfSight(this, target as Node2D, 2))
            {
                targetsVisible.Add(target);
            }
        }

        var closest = harvestableTargets
        .Cast<Node2D>()
        .OrderBy(node => (node.GlobalPosition - GlobalPosition).LengthSquared())
        .FirstOrDefault();

        if (closest != null)
        {
            HarvestTarget = closest as IHarvestable;
            currentState = State.Approaching;
            GD.Print("Going to harvest ", HarvestTarget.ToString());
        }
        else
        {
            GD.Print("Couldn't find a node");
            currentState = State.Idle;
        }
    }

    public async Task ApproachTarget(Node2D target)
    {
        if (SkillDict.TryGetValue(Skills.SkillTypes.Floating, out int rank) && rank <= 0)
        {
            return;
        }

        Vector2 toTarget = target.GlobalPosition - GlobalPosition;
        Vector2 direction = toTarget.Normalized();
        float stopDistance = 48.0f;
        Vector2 stopPosition = target.GlobalPosition - direction * stopDistance;

        double duration = Mathf.Clamp(5.0f / rank, 0.1f, 10.0f);
        var tween = GetTree().CreateTween();
        GD.Print($"Get Tree: Tween is valid? {tween.IsRunning()}");
        tween.SetEase(Tween.EaseType.InOut);
        GD.Print($"Set Ease: Tween is valid? {tween.IsRunning()}");
        tween.SetTrans(Tween.TransitionType.Sine);
        GD.Print($"Set Trans: Tween is valid? {tween.IsRunning()}");

        GD.Print($"This is the target: {target.ToString()}");
        tween.TweenProperty(this, "position", stopPosition, duration);

       
        await ToSignal(tween, "finished");

        currentState = State.Harvesting;
        GD.Print($"{Name} is Now Harvesting ", target.ToString());
        isApproaching = false;
    }

}
