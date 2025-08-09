using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class PlayerInventory : Resource
{
    public Dictionary<Yields.Yield, int> Inventory = new();


}