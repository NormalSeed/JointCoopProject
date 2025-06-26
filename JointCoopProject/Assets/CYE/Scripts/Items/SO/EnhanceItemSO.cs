using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 획득시 사용자의 수치를 변화시키는 아이템
/// </summary>
[CreateAssetMenu(menuName = "Items/Enhance Item", order = 3)]
public class EnhanceItemSO : ItemDataSO
{
    // 수치를 변경시키고자하는 타겟 
    // public T _changeTarget;
    // 변경할 수치
    public float _changeValue;
    // 수치 변화 방법(합연산-sum/곱연산-multiplication)
    public StatChangeMethod _changeOperation;

    // public override void PickedUp()
    // {
    //     // player의 수치 변화
    //     // event?
    // }
}
