using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/Flame")]
public class Flame : SkillDataSO
{
    // ��ų �̸� : ��ȭ
    // ���� Ÿ��
    // ĳ������ ���� �������� �ҵ��� �߻�
    // 35�ʸ��� 40%Ȯ���� �߻�
    // 1.5�� ���� 10�� ����� 3ȸ
    // 4m�� ��Ÿ�
    // ���� ���� (���� �� ��� �ı�)
    public int _skillLevel = 1;
    public int _skillDamage = 0;
    public float _projectileSpeed = 5f; // ����ü �ӵ�

    public override void UseSkill(Transform caster, Vector3 dir)
    {
        int _totalDamage;
        // Level +1 == Ȯ�� +5%
        float finalSkillPossibility = skillPossibility + (_skillLevel * 5f);

        int _randomValue = Random.Range(0, 100);
        if (_randomValue > finalSkillPossibility)
        {
            return;
        }
        // ��ȭ ����� (��ų���� +1 == ����� +10)
        _totalDamage = _skillDamage + (_skillLevel * 10);

        // ��ȭ ����
        GameObject flame = Instantiate(skillPrefab, caster.position, caster.rotation);
        FlameController flameController = flame.GetComponent<FlameController>();
        flameController.Init(dir, _projectileSpeed, _totalDamage);

        // ĳ������ �̵����⿡ �°� �ҵ��� ȸ��
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        flame.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
