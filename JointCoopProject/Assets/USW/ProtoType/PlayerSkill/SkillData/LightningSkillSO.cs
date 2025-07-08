using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Lightning Skill")]
public class LightningSkillSO : SkillDataSO
{
    [Header("번개 설정")]
    public float searchRadius = 4f;
    public int lightningCount = 3;
    public float lightningDelay = 0.3f;
    public int lightningDamage = 15;
    public LayerMask enemyLayer;

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