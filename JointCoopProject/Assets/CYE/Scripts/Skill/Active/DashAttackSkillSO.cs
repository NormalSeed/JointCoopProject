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
        Debug.Log("Dash");
        PlayerStatManager.Instance._canDash = true;        
        // 실제 useSkill땐 플레이어의 충돌체를 대미지를 줄수있는 형태로 변ㄴ경해야함
    }
    public override void ReleaseSkill()
    {
        Debug.Log("Dash end");
        // 대미지를 줄 수 있는 형태에서 다시 돌아옴
        PlayerStatManager.Instance._canDash = false;
    }
}
