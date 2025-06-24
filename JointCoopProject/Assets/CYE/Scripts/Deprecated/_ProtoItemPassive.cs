using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoItemPassive : ProtoSkilledItem
{

    private void Awake()
    {
        InitItemBase();
        InitSkilledItem();
    }

    protected override void Acquire()
    {
        OnAcquired?.Invoke();
    }
}
