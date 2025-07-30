using Godot;
[GlobalClass]
public partial class SkillRequirements : Resource
{
    [Export] public Skills.SkillTypes Skill;
    [Export] public int RequiredLevel;
}