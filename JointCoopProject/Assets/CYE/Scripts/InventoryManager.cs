using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : TempSingleton<InventoryManager>
{
    private Inventory inventory = new();
    public Inventory _inventory => inventory;
}
