using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoItemActive : ProtoSkilledItem
{
    private void Awake()
    {
        InitItemBase();
    }

    protected override void Use()
    {
        OnUsed?.Invoke();
        Cast();
    }

    protected override void Acquire()
    {
        OnAcquired?.Invoke();
    }

    protected override void Cast()
    {
        OnCasted?.Invoke();
    }
}
