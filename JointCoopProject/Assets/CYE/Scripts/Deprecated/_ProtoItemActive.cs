using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtoItemActive : ProtoSkilledItem
{
    private bool _canUse;
    #region // Events
    [HideInInspector] public UnityEvent OnUsed; // 아이템 사용시 발생
    #endregion

    private void Awake()
    {
        InitItemBase();
        InitSkilledItem();
        InitItemActive();
    }

    private void Update()
    {
        if (!_canUse)
        {
            CountCooldown();
        }
    }

    private void InitItemActive()
    {
        _canUse = false;
    }
    
    protected void Use()
    {
        OnUsed?.Invoke();
        Cast();
    }

    protected override void Acquire()
    {
        OnAcquired?.Invoke();
    }
}
