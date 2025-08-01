using Godot;
using System;

public partial class ScrapPile1 : Node2D, IHarvestable
{
    [Export] public Godot.Collections.Array<SkillRequirements> HarvestSkillsRequired { get; set; }
    [Export] public Godot.Collections.Array<HarvestYieldData> HarvestYield { get; set; }
}
