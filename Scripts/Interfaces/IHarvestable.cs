using Godot;

public interface IHarvestable
{
    Godot.Collections.Array<SkillRequirements> HarvestSkillsRequired {get; set;}
    Godot.Collections.Array<HarvestYieldData> HarvestYield { get; set; }
}