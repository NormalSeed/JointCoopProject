using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerManager : TempSingleton<TempPlayerManager>
{
    // private PlayerStatus status = new();
    // public PlayerStatus _status => status;    
    public PlayerStatManager _status;
    
    private Inventory inventory = new();
    public Inventory _inventory => inventory;
}
