using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/Hp Up")]
public class HpUpSkillSO : SkillDataSO
{
    public override void UseSkill(Transform caster)
    {
        // base.UseSkill(caster);
        // Player Hp Up
        // PlayerStatManager.Instance._playerHp += randomvalue;
    }
}
