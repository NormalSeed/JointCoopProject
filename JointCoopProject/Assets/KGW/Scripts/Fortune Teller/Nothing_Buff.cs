using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nothing_Buff : IBuff
{
    public string _buffName => "꽝";
    public string _buffDescription => "꽝입니다~~ 다음기회에...";
    public BuffCategory _buffCategory => BuffCategory.Nothing;
    public BuffType _buffType => BuffType.Nothing;
    public int _buffLevel => 0;

    public void BuffReceive(PlayerStatManager playerStatus)
    {
        Debug.Log("효과 없음");
    }
}

    
