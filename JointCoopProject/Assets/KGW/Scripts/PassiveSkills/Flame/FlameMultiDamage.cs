using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameMultiDamage : MonoBehaviour
{
    IDamagable _target; // ���� ���� Ÿ��
    int _flameDamage;   // ��ȭ �����
    float _flameTime = 1.5f;    // �ٴ� ��Ʈ �ð�
    int _flameAttackCount = 3;  // �ٴ� ��Ʈ Ƚ��
    Coroutine _flameRoutine;

    public void Apply(IDamagable target, int damage)
    {
        _target = target;
        _flameDamage = damage;
        // �ڷ�ƾ�� Null�� �ƴϸ� Null�� ����
        if(_flameRoutine != null)
        {
            StopCoroutine(_flameRoutine);
            _flameRoutine = null;
        }
        _flameRoutine = StartCoroutine(ApplyFlame());
    }

    // �÷��� �ٴ� ��Ʈ ����
    private IEnumerator ApplyFlame()
    {
        int count = 0;
        while(count < _flameAttackCount)
        {
            _target.TakeDamage(_flameDamage, transform.position);
            Debug.Log($"[��ȭ �����] {_flameDamage} ���� �����.");

            count++;
            yield return new WaitForSeconds(_flameTime);
        }
        _flameRoutine = null;

        // �ٽ� �����ϱ� ���ؼ� Component ����
        Destroy(gameObject.GetComponent<FlameMultiDamage>());
    }

}
