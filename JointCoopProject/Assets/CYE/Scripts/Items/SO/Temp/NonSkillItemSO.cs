using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSkillItemSO : ItemDataSO
{
    #region // Serialize field
    [Header("Additional Settings")]
    // 수치를 변경시키고자하는 타겟 
    // public T _changeTarget;
    // 변경할 수치
    public float _changeValue;
    // 수치 변화 방법(합연산-sum/곱연산-mlt)
    public StatChangeMethod _changeOperation;
    #endregion
}
