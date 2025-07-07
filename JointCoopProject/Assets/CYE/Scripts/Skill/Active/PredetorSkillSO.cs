using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/Predetor")]
public class PredetorSkillSO : SkillDataSO
{
    [SerializeField] private float _skillDamageRate; // 대미지 배율
    [SerializeField] private float _skillRange; // 사거리
    // float _skillCoolTime; // SkillDataSO에 있음
    [SerializeField] private float _duration; // 지속시간
    [SerializeField] private float _HealthRecovery; // 회복량
    [SerializeField] private int _cost; // 소모량

    public override void UseSkill(Transform caster)
    {
        Collider2D[] detectedCollider = Physics2D.OverlapCircleAll(caster.position, _skillRange + 0.5f);
        foreach (Collider2D collider in detectedCollider)
        {
            IDamagable damagableComponent = collider.GetComponent<IDamagable>();
            if (damagableComponent != null && !collider.CompareTag("Player"))
            {
                damagableComponent.TakeDamage(PlayerStatManager.Instance._attackDamage, caster.position);
            }
        }
        PlayerStatManager.Instance._playerHp += detectedCollider.Length;

        ReleaseSkill();
    }
    public override void ReleaseSkill()
    {
        if (PlayerStatManager.Instance._playerHp > PlayerStatManager.Instance._playerMaxHp)
        {
            PlayerStatManager.Instance._playerHp = PlayerStatManager.Instance._playerMaxHp;
        }
    }
}
