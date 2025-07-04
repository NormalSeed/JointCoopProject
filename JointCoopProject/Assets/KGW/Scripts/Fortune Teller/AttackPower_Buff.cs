using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPower_Buff : IBuff
{
    public string _buffName => $"공격력 {(_buffCategory == BuffCategory.Blessing ? "증가 버프" : "감소 버프")}";
    public string _buffDescription => $"공격력을 {(_buffCategory == BuffCategory.Blessing ? $"{_buffOffset}증가" : $"{_buffOffset}감소")}";
    public BuffCategory _buffCategory {  get; private set; }
    public BuffType _buffType => BuffType.AttackPower;
    public int _buffLevel {  get; private set; }

    int _buffOffset;

    public AttackPower_Buff(BuffCategory category, int level)
    {
        _buffCategory = category;
        _buffLevel = level;
    }

    public void BuffReceive(PlayerStatManager playerStatus)
    {
        _buffOffset = _buffLevel;

        if (_buffCategory == BuffCategory.Blessing)
        {
            playerStatus._attackDamage += _buffOffset;
        }
        if (_buffCategory == BuffCategory.Curs)
        {
            playerStatus._attackDamage -= _buffOffset;  // 저주시 마이너스 스텟 적용
        }
        
        Debug.Log($"띠링~~ 띠링~~!! 플레이어가 레벨 {_buffLevel}의 {_buffName}를 받았습니다!!.");
        Debug.Log($"플레이어의 {_buffDescription} 시킵니다.");
    }

}
