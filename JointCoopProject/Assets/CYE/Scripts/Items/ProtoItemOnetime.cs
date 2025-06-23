using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoItemOnetime : ProtoItem
{

    private void Awake()
    {
        InitItemBase();
    }
    
    protected override void Acquire()
    {
        OnAcquired?.Invoke();
    }
}
