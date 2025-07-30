using Godot;
[GlobalClass]
public partial class TechRequirements : Resource
{
    [Export] public Techs.TechsUnlocked TechRequired;
    [Export] public int TechLevel; //Future ranked techs. I hate this.
}