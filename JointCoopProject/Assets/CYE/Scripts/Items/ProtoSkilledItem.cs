using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ProtoSkilledItem : ProtoItem
{
    [SerializeField] private float _cooldown;

    #region // Events
    [HideInInspector] public UnityEvent OnUsed; // 아이템 사용시 발생
    [HideInInspector] public UnityEvent OnCasted; // 스킬 시전시 발생
    #endregion

    protected abstract void Use();

    protected abstract void Cast();
}
