using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/Dash Attack")]
public class DashAttackSO : SkillDataSO
{
    [SerializeField] private float _skillDamageRate; // 대미지 배율
    [SerializeField] private float _skillRange; // 사거리
    [SerializeField] private float _HealthRecovery; // 회복량
    [SerializeField] private int _cost; // 소모량

    public override void UseSkill(Transform caster)
    {
        PlayerStatManager.Instance._canDash = true;

        // GameObject dashTrigger = caster.GetChild(0).gameObject;
        // if (dashTrigger != null)
        // { 
        //     dashTrigger.SetActive(true);
        //     dashTrigger.GetComponent<DashAttackController>().enabled = true;
        //     dashTrigger.GetComponent<DashAttackController>()._dashDamage = PlayerStatManager.Instance._attackDamage * _skillDamageRate;
        // }
    }
    public override void ReleaseSkill(Transform caster)
    {
        // GameObject dashTrigger = caster.GetChild(0).gameObject;
        // if (dashTrigger != null)
        // { 
        //     dashTrigger.GetComponent<DashAttackController>().enabled = false;  
        //     dashTrigger.SetActive(false);
        // }
        PlayerStatManager.Instance._canDash = false;
    }
}
