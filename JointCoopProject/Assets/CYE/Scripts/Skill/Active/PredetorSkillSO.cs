using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/Predetor")]
public class PredetorSkillSO : SkillDataSO
{
    [SerializeField] private float _skillDamageRate; // 대미지 배율
    [SerializeField] private float _skillRange; // 사거리
    [SerializeField] private float _HealthRecovery; // 회복량
    [SerializeField] private int _cost; // 소모량

    public override void UseSkill(Transform caster)
    {
        Debug.Log("Predetor");
        Collider2D[] detectedCollider = Physics2D.OverlapCircleAll(caster.position, _skillRange + 0.5f);
        foreach (Collider2D collider in detectedCollider)
        {
            IDamagable damagableComponent = collider.GetComponent<IDamagable>();
            if (damagableComponent != null && collider.CompareTag("Enemy"))
            {
                Debug.Log("take Damage to Enemy!");
                damagableComponent.TakeDamage((int)(PlayerStatManager.Instance._attackDamage * _skillDamageRate), caster.position);
            }
        }

        PlayerStatManager.Instance._playerHp += detectedCollider.Length;
        if (PlayerStatManager.Instance._playerHp > PlayerStatManager.Instance._playerMaxHp)
        {
            PlayerStatManager.Instance._playerHp = PlayerStatManager.Instance._playerMaxHp;
        }
    }
    public override void ReleaseSkill()
    {
        Debug.Log("Predetor end");
    }
}
