using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempManager
{
    // public static PlayerStatManager playerStat = PlayerStatManager.Instance; 
    public static InventoryManager inventory => InventoryManager.GetInstance();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        InventoryManager.CreateInstance();
        // PlayerStatManager.CreatePlayerStatManager(); // Awake할때 실행됨
    }
}
