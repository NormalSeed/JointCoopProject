using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/Bomb Gain")]
public class BombGainSkillSO : SkillDataSO
{
    public override void UseSkill(Transform caster, out bool useResult)
    {
        ItemManager.inventory.GetBomb(1);
        useResult = true;
    }
}