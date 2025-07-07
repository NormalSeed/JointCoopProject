using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/Parry")]
public class ParrySkillSO : SkillDataSO
{
    [SerializeField] private float _skillDamageRate; // 대미지 배율
    [SerializeField] private float _skillRange; // 사거리
    [SerializeField] private float _HealthRecovery; // 회복량
    [SerializeField] private int _cost; // 소모량

    public override void UseSkill(Transform caster)
    {
        Debug.Log("Parry");
        // 플레이어 반격 상태
        PlayerStatManager.Instance._isParry = true;
    }
    public override void ReleaseSkill()
    {
        Debug.Log("Parry End");
        PlayerStatManager.Instance._attackBonus = PlayerStatManager.Instance._attackDamage;
        PlayerStatManager.Instance._isParry = false;
    }
}
