using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttackMultiDamage : MonoBehaviour
{
    IDamagable _target; // 공격 받을 타겟
    int _poisonDamage;  // 독 대미지
    float _poisonTime = 1.5f;   // 다단 히트 시간
    int _poisonAttackCount = 3; // 다단 히트 횟수
    Coroutine _poisonRoutine;
    
    public void Apply(IDamagable target, int damage)
    {
        _target = target;
        _poisonDamage = damage;
        // 코루틴이 Null이 아니면 Null로 세팅
        if (_poisonRoutine != null )
        {
            StopCoroutine(_poisonRoutine);
            _poisonRoutine = null;
        }
        _poisonRoutine = StartCoroutine(ApplyPoison());
    }

    // 독 공격 다단 히트 실행
    private IEnumerator ApplyPoison()
    {
        int count = 0;
        while (count < _poisonAttackCount)
        {
            _target.TakeDamage(_poisonDamage, transform.position);
            Debug.Log($"[독 대미지] {_poisonDamage} 피해 적용됨.");

            count++;
            yield return new WaitForSeconds(_poisonTime);
        }
        _poisonRoutine = null;

        // 다시 적용하기 위해서 Component 삭제
        Destroy(gameObject.GetComponent<PoisonAttackMultiDamage>());
    }
}
