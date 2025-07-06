using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGuageController : MonoBehaviour
{
    [Header("Item Guage Setting")]
    [SerializeField] float _coolTime = 15f;

    Animator _guageAnimator;
    public bool _canUseItem;

    readonly int IDLE_HASH = Animator.StringToHash("ActiveItemGuageIdle");
    readonly int COOLDOWN_HASH = Animator.StringToHash("ActiveItemCoolTime");

    private void Awake()
    {
        _guageAnimator = GetComponent<Animator>();
    }

    public void OnCoolDown()
    {
        if(!_canUseItem)
        {
            _guageAnimator.speed = 1f / _coolTime;  // 쿨타임에 맞게 애니메이션 속도 조절
            _guageAnimator.Play(COOLDOWN_HASH, 0, 0f);

            CancelInvoke("OffCoolDown");    // 중복 금지
            Invoke("OffCoolDown", _coolTime);
        }
        
    }

    private void OffCoolDown()
    {
        _canUseItem = true;
        _guageAnimator.speed = 1f;
    }

    public void ItemUse()
    {
        _guageAnimator.Play(IDLE_HASH, 0, 0f);
        Debug.Log("아이템 사용");
        _canUseItem = false;
        OnCoolDown();
    }

    public void GetItme()
    {
        _guageAnimator.speed = 1f;
        OnCoolDown();
    }
}
