using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    // 버퍼의 이름
    string _buffName {  get; }
    // 버퍼의 효과 설명
    string _buffDescription { get; }
    // 버퍼의 카테고리 (축복, 저주, 꽝)
    BuffCategory _buffCategory { get; }
    // 버퍼의 종류 (공격력업, 스피드업, 공격속도업, 꽝(운세)
    BuffType _buffType { get; }
    // 버퍼의 레벨
    int _buffLevel { get; }

    // 플레이어의 버퍼를 받는 메서드
    public void BuffReceive(PlayerStatus playerStatus);
}

public enum BuffCategory
{ //  축복  , 저주, 꽝 
    Blessing, Curs, Nothing
}

public enum BuffType
{ // 공격력 상승 ,이동속도 상승, 아이템 데미지 증가, 꽝
    AttackPowerUp, MoveSpeedUp, AttackSpeedUp, Nothing
}
