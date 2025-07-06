using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/Resurrection")]
public class ResurrectionSkillSO : SkillDataSO
{
    public override void UseSkill(Transform caster, out bool useResult)
    {
        PlayerStatManager.Instance._canResurrect = true;
        useResult = true;
    }
}
