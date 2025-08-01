using Godot;
using System;
using System.Collections.Generic;

public partial class ScrapBot : Node2D, IHarvester
{
    [Export] public Godot.Collections.Array<ActorSkillSet> SkillMap { get; set; }
    private Dictionary<Skills.SkillTypes, int> SkillDict;

    public override void _Ready()
    {
        SkillDict = new Dictionary<Skills.SkillTypes, int>();
        foreach (var Entry in SkillMap) { SkillDict[Entry.ActorSkill] = Entry.ActorSkillRank; } //Makes it a non-shit dict instead of an array
    }

    public int GetSkillLevel(Skills.SkillTypes skill)
    {
        return SkillDict.TryGetValue(skill, out int level) ? level : 0; //Returns 0 if the skill doesn't exist.
    }

}
