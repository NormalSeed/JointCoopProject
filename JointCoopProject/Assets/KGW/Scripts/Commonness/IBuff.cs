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
    // 버퍼의 종류 (공격력, 스피드, 공격속도, 오늘의 운세)
    BuffType _buffType { get; }
    // 버퍼의 레벨
    int _buffLevel { get; }

    // 플레이어의 버퍼를 받는 메서드
    public void BuffReceive(PlayerStatManager playerStatus);
}

// 버퍼의 종류
public enum BuffCategory
{ 
  //  축복  , 저주,   꽝 
    Blessing, Curs, Nothing
}

// 버퍼의 타입
public enum BuffType
{ 
  //   공격력  , 이동속도 ,  공격속도  , 오늘의 운세, 꽝
    AttackPower, MoveSpeed, AttackSpeed, Fortune, Nothing

}
