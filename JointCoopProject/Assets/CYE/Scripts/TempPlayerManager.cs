using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerManager : TempSingleton<TempPlayerManager>
{
    private PlayerStatus playerStatus = new();
    public PlayerStatus _playerStatus => playerStatus;

    
}
