using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Player Passive/Money Drop")]
public class MoneyDropSkillSO : SkillDataSO
{
    
    public int _skillLevel = 1;

    [Range(0, 1)]
    public float _additionalDropRate = 0.1f;

    public override void UseSkill(Transform caster)
    {
        // 스킬 해제
    }
}
