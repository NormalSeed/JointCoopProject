// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// [CreateAssetMenu(menuName = "Items/Non Skill Item", order = 2)]
// public class NonSkillItemSO : ItemDataSO
// {
//     #region // Serialize field
//     [Header("Additional Settings")]
//     // 수치를 변경시키고자하는 타겟 
//     // public UnityEvent<float, StatChangeMethod> _changeTargetFunction;
//     public ChangeTarget _changetarget;
//     // 변경할 수치
//     public float _changeValue;
//     // 수치 변화 방법(합연산-sum/곱연산-mlt)
//     public StatChangeMethod _changeOperation;
//     #endregion

//     // PlayerStatManager

//     public override void Act()
//     {
//         // _changeTargetFunction?.Invoke(_changeValue, _changeOperation);
//         // _changeTarget += _changeValue;
//         // _changeTarget += _changeValue * (_changeValue / 100);
//         switch (_changetarget)
//         {
//             case ChangeTarget.CurHp:
//                 TempManager._player._status._playerHp += (int)_changeValue;
//                 break;
//             case ChangeTarget.AttackPoint:
//                 TempManager._player._status._attackDamage += (int)_changeValue;
//                 break;
//             case ChangeTarget.AttackSpeed:
//                 TempManager._player._status._attackSpeed += (int)_changeValue;
//                 break;
//             case ChangeTarget.MoveSpeed:
//                 TempManager._player._status._moveSpeed += (int)_changeValue;
//                 break;
//             // case ChangeTarget.Coin:
//             //     TempManager._player._status._coin += (int)_changeValue;
//             //     break;
//             // case ChangeTarget.Bomb:
//             //     TempManager._player._status._bomb += (int)_changeValue;
//             //     break;
//             default:
//                 break;
//         }
//     }
// }
