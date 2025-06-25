using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자가 임의로 사용이 불가능하며, 대신 활성화할 경우 자동으로 효과가 발동하는 아이템
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Item/Passive Item")]
public class PassiveItemSO : ItemDataSO
{
    
    [Header("Skill Settings")]
    public SkillDataSO _skillData;

    // 아이템 등급
    public int _grade;
    
    private bool _canUse;
    private float _skillTimer;
    private float _itemTimer;
    public float _itemCooldown;

    public override void PickedUp()
    {
        // 예상되는 문제 -> 사용자가 Active Item을 번갈아 착용할 경우 아이템 및 스킬 쿨타임이 초기화됨
        _canUse = true;
        _itemTimer = 0f;
        _skillTimer = 0f;
    }    
    public override void Act(Transform usePos)
    {
        _skillData.UseSkill(usePos);
    }
}
