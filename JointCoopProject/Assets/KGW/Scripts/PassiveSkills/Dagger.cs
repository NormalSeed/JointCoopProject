using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Dagger")]
public class Dagger : SkillDataSO
{
    // 스킬 이름 : 단검 투척
    // 단일 타겟
    // 캐릭터의 정면 방향으로 단검 투척
    // 15초 마다 2개의 단검 투척
    // 25의 데미지
    // 4m의 투척거리
    // 관통 없음 (적중 시 즉시 파괴)
    public int _skillLevel = 1;
    public int _skillDamage = 25;
    public float _projectileSpeed = 5f;
    public float _skillTimer = 3f;
    public Transform _PlayerThrowPoint;

    public override void UseSkill(Transform caster)
    {
        int _randomValue = Random.Range(0, 100);
        if (_randomValue > skillPossibility)
        {
            return;
        }

        GameObject Dagger = Instantiate(skillPrefab, _PlayerThrowPoint.forward, Quaternion.identity);
        _skillTimer -= Time.deltaTime;

        if(_skillTimer <= 0)
        {
            Destroy(Dagger);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

}
