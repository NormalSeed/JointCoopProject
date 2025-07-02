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
    public int _skillDamage = 20;
    public float _projectileSpeed = 5f; // 투사체 속도
    
    public override void UseSkill(Transform caster, Vector3 dir)
    {
        int _totalDamage;
        // Level +1 == 확률 +5%
        skillPossibility += (_skillLevel * 5f);

        int _randomValue = Random.Range(0, 100);
        if (_randomValue > skillPossibility)
        {
            return;
        }
        // 단검 데미지 (스킬레벨 +1 == 데미지 +5)
        _totalDamage = _skillDamage + (_skillLevel * 5);

        // 단검 생성
        GameObject dagger = Instantiate(skillPrefab, caster.position, caster.rotation);
        DaggerController daggerController = dagger.GetComponent<DaggerController>();
        daggerController.Init(dir, _projectileSpeed, _totalDamage);

        // 캐릭터의 이동방향에 맞게 단검 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dagger.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
