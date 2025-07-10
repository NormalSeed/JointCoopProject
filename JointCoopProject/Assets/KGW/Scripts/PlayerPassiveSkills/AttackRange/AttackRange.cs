using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Player Passive/Attack Range")]
public class AttackRange : SkillDataSO
{
    // ��ų �̸� : ��Ÿ� ���� 
    // �⺻ ���� ��Ÿ� ����
    // Level +1 == 0.3m ����
    public int _skillLevel = 1;
    public float _upgradeRange = 0.3f;
    public float _scaleUpRange = 0.6f;

    public override void UseSkill(Transform caster, Vector3 dir)
    {
        float totalRange = PlayerStatManager.Instance._attackRange + _upgradeRange;
        PlayerStatManager.Instance._attackRange = totalRange;
        Debug.Log($"[�⺻ ����] {_upgradeRange} ��Ÿ� ����");

        // Range�� ������ ��ŭ �������� Scale.x�� �����Ͽ� �ð��� ���� ȿ��
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
