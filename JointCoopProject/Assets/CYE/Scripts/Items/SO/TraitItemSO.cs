using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 획득시 단계별로 사용자의 기본 공격을 강화하는 아이템
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Item/Trait Item")]
public class TraitItemSO : ItemDataSO
{
    [Header("Skill Settings")]
    public SkillDataSO _skillData;

    // 아이템 등급
    public int _grade;

    public override void PickedUp()
    {
        
    }
    public override void Act(Transform usePos)
    {
        _skillData.UseSkill(usePos);
    }
}
