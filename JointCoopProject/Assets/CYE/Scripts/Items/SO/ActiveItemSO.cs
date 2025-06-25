using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자가 키 입력을 통해 능동적으로 사용할 수 있는 아이템
/// </summary>
[CreateAssetMenu(menuName = "Items/Active Item", order = 0)]
public class ActiveItemSO : ItemDataSO
{
    [Header("Skill Settings")]
    public SkillDataSO _skillData;

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
    public override void Act(Transform currentPosition)
    {
        _skillData.UseSkill(currentPosition);
    }
}
