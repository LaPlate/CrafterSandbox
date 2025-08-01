using Godot;

public interface IHarvester
{
    public Godot.Collections.Array<ActorSkillSet> SkillMap { get; set; }
    public int GetSkillLevel(Skills.SkillTypes skill);
}