using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/Berserk")]
public class BerserkSkillSO : SkillDataSO
{
    [SerializeField] private float _skillDamageRate; // 대미지 배율
    [SerializeField] private float _skillRange; // 사거리
    [SerializeField] private float _HealthRecovery; // 회복량
    [SerializeField] private int _cost; // 소모량
    private float _prevAttackSpeed;

    public override void UseSkill(Transform caster)
    {
        _prevAttackSpeed = PlayerStatManager.Instance._attackSpeed;

        PlayerStatManager.Instance._playerHp = (PlayerStatManager.Instance._playerHp > 2) ? (PlayerStatManager.Instance._playerHp - 2) : 1;
        PlayerStatManager.Instance._attackSpeed *= 2;
    }
    public override void ReleaseSkill()
    {
        PlayerStatManager.Instance._attackSpeed = _prevAttackSpeed;
    }
}
