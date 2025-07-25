using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/All Stat Up")]
public class AllStatUpSO : SkillDataSO
{
    public override void UseSkill(Transform caster, out bool useResult)
    {
        PlayerStatManager.Instance._playerHp += 2;
        PlayerStatManager.Instance._moveSpeed += 0.2f;
        PlayerStatManager.Instance._playerLuck += 1;
        PlayerStatManager.Instance._attackSpeed += 0.2f;

        useResult = true;
    }
}
