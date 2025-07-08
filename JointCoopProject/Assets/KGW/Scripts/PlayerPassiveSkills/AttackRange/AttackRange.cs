using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Player Passive/Attack Range")]
public class AttackRange : SkillDataSO
{
    // 스킬 이름 : 사거리 증가 
    // 기본 공격 사거리 증가
    // Level +1 == 0.3m 증가
    public int _skillLevel = 1;
    public float _upgradeRange = 0.3f;
    public float _scaleUpRange = 0.6f;

    public override void UseSkill(Transform caster, Vector3 dir)
    {
        float totalRange = PlayerStatManager.Instance._attackRange + _upgradeRange;
        PlayerStatManager.Instance._attackRange = totalRange;
        Debug.Log($"[기본 공격] {_upgradeRange} 사거리 증가");

        // Range가 증가한 만큼 프리팹의 Scale.x도 증가하여 시각적 증가 효과
        Vector3 scaleUp = skillPrefab.transform.localScale;
        scaleUp.x += _scaleUpRange;
        skillPrefab.transform.localScale = scaleUp;
    }

    private void OnDisable()
    {
        Vector3 scaleReset = skillPrefab.transform.localScale;
        scaleReset.x = 1.5f;
        skillPrefab.transform.localScale = scaleReset;
    }
}
