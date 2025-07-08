using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGuageController : MonoBehaviour
{
    float _coolTime;

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
            GetComponent<Animator>().speed = 1f / _coolTime;
            GetComponent<Animator>().Play(COOLDOWN_HASH, 0, 0f);

            CancelInvoke("OffCoolDown");
            Invoke("OffCoolDown", _coolTime);
        }
        
    }

    public void SetCoolTime(float coolTime)
    {
        _coolTime = coolTime;
    }

    private void OffCoolDown()
    {
        _canUseItem = true;
        GetComponent<Animator>().speed = 1f;
    }

    public void ItemUse()
    {
        if (_canUseItem)
        {
            return;
        }
        GetComponent<Animator>().Play(IDLE_HASH, 0, 0f);
        _canUseItem = false;
        OnCoolDown();
    }

    public void GetItme()
    {
        GetComponent<Animator>().speed = 1f;
        OnCoolDown();
    }
}
