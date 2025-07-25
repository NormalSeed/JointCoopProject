using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Laser Skill")]
public class LaserSkillSO : SkillDataSO
{
    [Header("레이저 설정")]
    public float searchRadius = 4f;    
    public int laserCount = 3;         
    public float laserDelay = 0.2f;    
    public float laserDamage = 10f;

    public override void UseSkill(Transform caster)
    {
        LaserSkillController laserController = caster.GetComponent<LaserSkillController>();
        
        if (laserController == null)
        {
            laserController = caster.gameObject.AddComponent<LaserSkillController>();
        }
        
        laserController.FireLasers(this, caster);
    }
}