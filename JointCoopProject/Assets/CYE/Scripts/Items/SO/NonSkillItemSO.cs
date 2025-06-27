using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Non Skill Item", order = 2)]
public class NonSkillItemSO : ItemDataSO
{
    #region // Serialize field
    [Header("Additional Settings")]
    // 수치를 변경시키고자하는 타겟 
    public UnityEvent<float, StatChangeMethod> _changeTargetFunction;
    // 변경할 수치
    public float _changeValue;
    // 수치 변화 방법(합연산-sum/곱연산-mlt)
    public StatChangeMethod _changeOperation;
    #endregion

    // PlayerStatManager

    public override void Act(Transform currentPosition)
    {
        _changeTargetFunction?.Invoke(_changeValue, _changeOperation);
        // _changeTarget += _changeValue;
        // _changeTarget += _changeValue * (_changeValue / 100);
    }
}
