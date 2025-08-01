using Godot;

[GlobalClass]
public partial class ActorSkillSet : Resource
{
    [Export] public Skills.SkillTypes ActorSkill;
    [Export] public int ActorSkillRank;
}