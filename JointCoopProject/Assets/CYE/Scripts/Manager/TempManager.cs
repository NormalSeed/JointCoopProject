using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempManager
{
    public static InventoryManager inventory => InventoryManager.GetInstance();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        InventoryManager.CreateInstance();
    }
}
