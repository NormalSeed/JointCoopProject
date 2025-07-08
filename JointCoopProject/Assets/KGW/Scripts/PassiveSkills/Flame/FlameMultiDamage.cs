using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameMultiDamage : MonoBehaviour
{
    IDamagable _target; // 공격 받을 타겟
    int _flameDamage;   // 염화 대미지
    float _flameTime = 1.5f;    // 다단 히트 시간
    int _flameAttackCount = 3;  // 다단 히트 횟수
    Coroutine _flameRoutine;

    public void Apply(IDamagable target, int damage)
    {
        _target = target;
        _flameDamage = damage;
        // 코루틴이 Null이 아니면 Null로 세팅
        if(_flameRoutine != null)
        {
            StopCoroutine(_flameRoutine);
            _flameRoutine = null;
        }
        _flameRoutine = StartCoroutine(ApplyFlame());
    }

    // 플레임 다단 히트 실행
    private IEnumerator ApplyFlame()
    {
        int count = 0;
        while(count < _flameAttackCount)
        {
            _target.TakeDamage(_flameDamage, transform.position);
            Debug.Log($"[염화 대미지] {_flameDamage} 피해 적용됨.");

            count++;
            yield return new WaitForSeconds(_flameTime);
        }
        _flameRoutine = null;

        // 다시 적용하기 위해서 Component 삭제
        Destroy(gameObject.GetComponent<FlameMultiDamage>());
    }

}
