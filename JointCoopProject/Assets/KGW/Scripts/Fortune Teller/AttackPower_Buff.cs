using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPower_Buff : IBuff
{
    public string _buffName => $"공격력 {(_buffCategory == BuffCategory.Blessing ? "증가 버프" : "감소 버프")}";
    public string _buffDescription => $"플레이어의 공격력을 {_buffStatus} {(_buffCategory == BuffCategory.Blessing ? "증가 시킵니다." : "감소 시킵니다.")}";
    public BuffCategory _buffCategory {  get; private set; }
    public BuffType _buffType => BuffType.AttackPower;
    public int _buffLevel {  get; private set; }

    int _buffStatus;

    public AttackPower_Buff(BuffCategory category, int level)
    {
        _buffCategory = category;
        _buffLevel = level;
    }

    public void BuffReceive(PlayerStatus playerStatus)
    {
        if(_buffCategory == BuffCategory.Blessing)
        {
            _buffStatus = _buffLevel;
        }
        if (_buffCategory == BuffCategory.Curs)
        {
            _buffStatus *= -1;   // 저주시 마이너스 스텟 적용
        }
        playerStatus._attackDamage += _buffStatus;
    }

}
