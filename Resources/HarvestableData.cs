using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class HarvestableData : Resource
{
    public Godot.Collections.Array<SkillRequirements> SkillsRequired { get; set; } = new();
    public Godot.Collections.Array<TechRequirements> TechRequired { get; set; }
    public Godot.Collections.Array<HarvestYieldData> Yield { get; set; }
    public int BaseYieldSpeed { get; set; }
}