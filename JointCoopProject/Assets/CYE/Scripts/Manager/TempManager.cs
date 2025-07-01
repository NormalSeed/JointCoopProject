using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempManager
{
    // For Test Code
    public const bool IS_ITEM_TEST = true;

    public static InventoryManager inventory => InventoryManager.GetInstance();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        InventoryManager.CreateInstance();
    }
}
