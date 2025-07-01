using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Flame")]
public class Flame : SkillDataSO
{
    // 스킬 이름 : 염화
    // 단일 타겟
    // 캐릭터의 정면 방향으로 불덩이 발사
    // 35초마다 40%확률로 발사
    // 1.5초 마다 10의 대미지 3회
    // 4m의 사거리
    // 관통 없음 (적중 시 즉시 파괴)
    public int _skillLevel = 1;
    public int _skillDamage = 0;
    public float _projectileSpeed = 5f;

    public override void UseSkill(Transform caster, Vector3 dir)
    {
        int _totalDamage;

        int _randomValue = Random.Range(0, 100);
        if (_randomValue > skillPossibility)
        {
            return;
        }
        // 염화 데미지 (스킬레벨 +1 == 데미지 +10)
        _totalDamage = _skillDamage + (_skillLevel * 10);

        // 염화 생성
        GameObject Flame = Instantiate(skillPrefab, caster.position, caster.rotation);
        FlameController FlameController = Flame.GetComponent<FlameController>();
        FlameController.Init(dir, _projectileSpeed, _totalDamage);

        // 캐릭터의 이동방향에 맞게 불덩이 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Flame.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
