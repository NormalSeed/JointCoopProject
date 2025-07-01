using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Lightning Skill")]
public class LightningSkillSO : SkillDataSO
{
    [Header("번개 설정")]
    public float searchRadius = 4f;      // 탐지 반경
    public int lightningCount = 3;       // 번개 개수
    public float lightningDelay = 0.3f;  // 번개 간 간격
    public LayerMask enemyLayer;   // 적 레이어

    public override void UseSkill(Transform caster)
    {
        LightningSkillController lightningController = caster.GetComponent<LightningSkillController>();
        
        if (lightningController == null)
        {
            lightningController = caster.gameObject.AddComponent<LightningSkillController>();
        }
        
        lightningController.StrikeLightning(this, caster);
    }
}