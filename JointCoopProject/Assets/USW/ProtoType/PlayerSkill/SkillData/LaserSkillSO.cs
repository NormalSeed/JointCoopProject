using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Laser Skill System/Skill Data")]
public class LaserSkillSO : SkillDataSO
{
    public GameObject laserEffectPrefab;
    public float duration = 0.5f;

    [SerializeField]
    public float laserOffset = 20.0f;
    public override void UseSkill(Transform caster)
    {
        // 캐릭터가 바라보는 방향 구하기
        // atan2 이용해서 방향각 구하기
        // 레이저 위치 계산하기
        // 회전값 생성하기
        // 그러면 ? 

       
        
        float directionX = Mathf.Sign(caster.localScale.x);
        
        float angle = Mathf.Atan2(0f, directionX) * Mathf.Rad2Deg;
        
        Vector3 spawnPos = caster.position + new Vector3(laserOffset * directionX, 0f, 0f);
        
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        
        GameObject laser = Instantiate(laserEffectPrefab, spawnPos, rotation);
        Destroy(laser, duration);
    }
}
