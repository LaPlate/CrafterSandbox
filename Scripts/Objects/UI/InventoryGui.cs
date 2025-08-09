using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryGui : Control
{
    public PlayerInventory InventoryGUI = new();
    public RichTextLabel ScrapCountText = new();
    public RichTextLabel WiringCountText = new();

    public override void _Ready()
    {
        ScrapCountText = GetNode<RichTextLabel>("HBoxContainer/ScrapTextBox");
        WiringCountText = GetNode<RichTextLabel>("HBoxContainer/WiringTextBox");
        InventoryGUI.Inventory.Add(Yields.Yield.TechScrap, 0);
        InventoryGUI.Inventory.Add(Yields.Yield.Wiring, 0);

    }


    public override void _Process(double delta)
    {
        ScrapCountText.Text = InventoryGUI.Inventory[Yields.Yield.TechScrap].ToString();
        WiringCountText.Text = InventoryGUI.Inventory[Yields.Yield.Wiring].ToString();
    }

}
