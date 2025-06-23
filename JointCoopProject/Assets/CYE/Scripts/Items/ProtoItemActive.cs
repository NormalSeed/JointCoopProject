using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoItemActive : ProtoItem
{
    private void Awake()
    {
        InitItemBase();
    }

    protected override void Use() 
    {
        OnUsed?.Invoke();
    }

    protected override void Acquire() 
    {
        OnAcquired?.Invoke();
    }
}
