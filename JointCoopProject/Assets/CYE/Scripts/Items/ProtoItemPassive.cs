using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoItemPassive : ProtoItem
{
    [SerializeField] private float _cooldown;
    

    private void Awake()
    {
        InitItemBase();
    }

    protected override void Use() { }

    protected override void Acquire()
    {
        OnAcquired?.Invoke();
    }
}
