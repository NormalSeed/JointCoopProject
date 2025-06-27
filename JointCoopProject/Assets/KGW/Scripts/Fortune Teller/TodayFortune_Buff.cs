using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodayFortune_Buff : IBuff
{
    public string _buffName => throw new System.NotImplementedException();
    public string _buffDescription => throw new System.NotImplementedException();
    public BuffCategory _buffCategory { get; private set; }
    public BuffType _buffType => BuffType.Fortune;
    public int _buffLevel {  get; private set; }

    public TodayFortune_Buff(BuffCategory category, int level)
    {
        _buffCategory = category;
        _buffLevel = level;
    }

    public void BuffReceive(PlayerStatus playerStatus)
    {
        
    }
}
