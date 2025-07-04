using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/Luck Up")]
public class LuckUpSkillSO : SkillDataSO
{
    
    public override void UseSkill(Transform caster)
    {
        // base.UseSkill(caster);
        // Player Luck Up
        // PlayerStatManager.Instance._playerLuck += randomvalue;
    }
}
