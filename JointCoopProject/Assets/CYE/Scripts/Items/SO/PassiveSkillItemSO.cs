// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [CreateAssetMenu(menuName = "Items/Skill Item", order = 1)]
// public class PassiveSkillItemSO : ItemDataSO
// {
//     #region // Serialize field
//     [Header("Additional Settings")]
//     public SkillItemType _skillType;
//     public int _itemGrade;
//     public float _itemCooldown;

//     [Header("Skill Settings")]
//     public SkillDataSO _itemSkill;
//     #endregion

//     public override void Act(Transform currentPosition)
//     {
//         _itemSkill.UseSkill(currentPosition);
//     }
// }
