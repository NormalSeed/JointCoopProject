using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBallController : MonoBehaviour
{
    bool _isContact = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체가 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 한번 접촉했으면 재접촉 불가
            if (_isContact)
            {
                // TODO : 사용했다는 문구 출력?
                Debug.Log("이미 버프를 받았습니다.");
                return;
            }

            // 랜덤 버퍼 룰렛 생성
            IBuff randomBuff = BuffCreateFactory.BuffRoulette();
            // 버퍼 적용
            BuffManager.Instance.ApplyBuff(randomBuff, PlayerStatManager.Instance);
            // 버프를 받고 수정구슬은 사라지지 않고 남아있지만 다시 접촉을해도 상호작용 없음
            //_isContact = true;

            // 접촉시 사운드와 이펙트
        }
    }
}
