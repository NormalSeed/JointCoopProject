using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Skill Item", order = 10)]
public class SkillItemSO : ItemDataSO
{
    #region // Serialize field
    [Header("Additional Settings")]
    public SkillItemType _skillType;
    public int _itemGrade;
    public float _itemCooldown;

    [Header("Skill Settings")]
    public SkillDataSO[] _itemSkill;
    #endregion

    public override void PickedUp(GameObject player)
    {
        player.GetComponent<PlayerSkillManager>().AddSkill(_itemSkill[_itemGrade]);
        // 등급 아이템의 경우 스킬 리스트에서 기존 스킬 정보를 지우고 새 정보를 넣어야한다. 논의 필요.
    }

    public override void Act(Transform currentPosition)
    {
        switch (_skillType)
        {
            case SkillItemType.Active:
                _itemSkill[_itemGrade].UseSkill(currentPosition);
                break;
            default:
                Debug.Log("SkillItemSO >> Item do not have _skilltype.Active.");
                break;
        }
    }
}
