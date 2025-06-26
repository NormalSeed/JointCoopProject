using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempManager
{
    public static TempPlayerManager _playerManager => TempPlayerManager.GetInstance();
    public static InventoryManager _InventoryManager => InventoryManager.GetInstance();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        TempPlayerManager.CreateInstance();
        InventoryManager.CreateInstance();
    }
}
