using Godot;
using System;
using System.Collections.Generic;

public partial class ScrapPile1 : RigidBody2D, IHarvestable
{
    [Export] public HarvestableData HarvestData { get; set; }

    public PackedScene YieldMessageScene = GD.Load<PackedScene>("res://Scenes/Objects/UI/context_message_popup.tscn");

    public InventoryGui InventoryGUI;

    [Export] public int MaxHarvests;

    public override void _Ready()
    {
        InventoryGUI = GetTree().CurrentScene.GetNode<InventoryGui>("InventoryGUI");

    }


    public void YieldHarvest(IHarvester Harvester)
    {
        var YieldMessage = YieldMessageScene.Instantiate<ContextMessagePopup>();
        Dictionary<Yields.Yield, int> TotalYield = new();
        foreach (HarvestYieldData YieldType in HarvestData.Yield)
        {
            float roll = GD.Randf();
            if (roll < YieldType.YieldWeight)
            {
                TotalYield.Add(YieldType.ResourceYielded, YieldType.NumberYielded);
            }
        }
        string message = "";
        foreach (var resourceAward in TotalYield)
        {
            InventoryGUI.InventoryGUI.Inventory[resourceAward.Key] += resourceAward.Value;

            message += "+";
            message += resourceAward.Value;
            message += " " + resourceAward.Key.ToString();
            message += "\r\n";
        }
        GD.Print(message);
        YieldMessage.SetText(message);
        GetTree().Root.AddChild(YieldMessage);
        Node2D Harvester2D = Harvester as Node2D;
        YieldMessage.GlobalPosition = Harvester2D.GlobalPosition + new Vector2(-30, -50);

        MaxHarvests--;
        GD.Print($"{MaxHarvests.ToString()} harvests remaining.");
        if (MaxHarvests == 0) QueueFree();

    }
}
