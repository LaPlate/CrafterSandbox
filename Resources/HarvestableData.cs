using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class HarvestableData : Resource
{
    [Export]public Godot.Collections.Array<SkillRequirements> SkillsRequired { get; set; } 
    [Export]public Godot.Collections.Array<TechRequirements> TechRequired { get; set; }
    [Export]public Godot.Collections.Array<HarvestYieldData> Yield { get; set; }
    [Export]public int BaseYieldSpeed { get; set; }
}