using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Laser Skill")]
public class LaserSkillSO : SkillDataSO
{
    public float duration = 0.5f;

    public float laserSpeed = 5f;
    
    [SerializeField]
    public float laserOffset = 5.0f;

    // 적의 태그를 설정하여 추적용으로 사용 , 
    // 태그를 이용한 추적함수는 디폴트로 정합니다.
    public string enemyTag = "Enemy";
   
    public override void UseSkill(Transform caster)
    {
        // 캐릭터가 바라보는 방향 구하기
        // atan2 이용해서 방향각 구하기
        // 레이저 위치 계산하기
        // 회전값 생성하기
        // 그러면 ? 
        // 적 추적 ,  - > tag 형식 ? , layer 형식 ? 
        // enemy 종류 10가지 산정 , tag 형식 
        // 각도 아크탄젠트로 추적후 
        // distance 를 산정할필요가있나 .            
        Transform target = FindClosestTarget(caster.position);
        
        if (target == null)
        {
            Debug.Log("타겟 없음");
        }
        
        GameObject laser = Instantiate(skillPrefab,caster.position, Quaternion.identity);
        
        var bullet = laser.GetComponent<ChasingEnemy>();
        if (bullet != null)
        {
            bullet.SetTarget(target.position);
            bullet.SetSpeed(laserSpeed);
        }
        
        // 아 너무 머리아픈데;
        // 1. 스킬이 많아질 예정이고  , 스킬로직이 유사할경우 ( skillmanager 에 통합설계 )  
        // 2 . 확장성을 이용해 (VFX , 타격 ) 을 분리하고싶으면 ( 전용 이동 컨트롤러 스크립트 따로 작성해야함. )
        // 3 . 스킬이 10개 내외고 프리팹마다 특성 다르게 줄꺼면 ( 지금 유지 할꺼임 ) 
        
        // 예를들어 skillmanager를 통합설계할꺼면 skillmanager에다가 그냥 visualeffect transform = toward postion 빼면되는 클래스 설계)
        // 그리고 playerskillmanager에다가 update문으로 돌림 
        // var로 순회돌면서 시각효과만 리턴 치고 vertor3 target = findcloestenemy 를 skillmanager에서 처리하는거져 
        //  activeskill.add(new Movingskill {
        //  visualEffect = effect
        //  start = transform.position
        //  target = target
        //  speed 
        // 지정해두고 아래에 skill.timer = skill.cooldown 으로 실시간 시간 피드백하면 되긴해요 근데 그렇게하면 확장할때마다 일일이 선언해줘야함.
        // 
        // ㅖ ? ㅢㅖ? 
        
       
            
        //float directionX = Mathf.Sign(caster.localScale.x);
        //
        //float angle = Mathf.Atan2(0f, directionX) * Mathf.Rad2Deg;
        //
        //Vector3 spawnPos = caster.position + new Vector3(laserOffset * directionX, 0f, 0f);
        //
        //Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        //
        //GameObject laser = Instantiate(laserEffectPrefab, spawnPos, rotation);
        //Destroy(laser, duration);
    }
    
    
    
    
    
    
    
    
    // 가까이 있는 target 거리 추적
    private Transform FindClosestTarget(Vector3 casterPos)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closest = null;
        float minDist = float.MaxValue;
        
        // 적 리스트 순회
        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(casterPos,enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }
        return closest;
    }
}
