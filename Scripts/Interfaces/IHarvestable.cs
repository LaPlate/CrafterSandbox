using Godot;

public interface IHarvestable
{
    HarvestableData HarvestData { get; set; }

    bool TimeToDie { get; set; }
    public void YieldHarvest(HarvestSession Session);

    public void Die();
}