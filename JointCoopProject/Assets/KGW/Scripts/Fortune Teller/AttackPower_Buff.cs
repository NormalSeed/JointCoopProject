using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPower_Buff : IBuff
{
    public string _buffName => throw new System.NotImplementedException();
    public string _buffDescription => throw new System.NotImplementedException();
    public BuffCategory _buffCategory {  get; private set; }
    public BuffType _buffType => throw new System.NotImplementedException();
    public int _buffLevel => throw new System.NotImplementedException();

    public AttackPower_Buff (BuffCategory category, int level)
    {
        _buffCategory = category;

    }

    public void BuffReceive(PlayerStatus playerStatus)
    {
        
    }

}
