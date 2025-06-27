using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자가 임의로 사용이 불가능하며, 대신 활성화할 경우 자동으로 효과가 발동하는 아이템
/// </summary>
[CreateAssetMenu(menuName = "Items/Passive Item", order = 1)]
public class PassiveItemSO : ItemDataSO
{    
    [Header("Skill Settings")]
    public SkillDataSO _skillData;

    

    // 아이템 등급
    public int _grade;
    
    // public override void PickedUp()
    // {

    // }
    // public override void Act(Transform currentPosition)
    // {
    //     _skillData.UseSkill(currentPosition);
    // }
}
