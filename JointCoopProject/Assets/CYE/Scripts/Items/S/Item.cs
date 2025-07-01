using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item Info")]
    [Tooltip("아이템 타입을 결정합니다.\n - Active: 액티브 \n - PassiveAttack: 패시브-공격 강화 \n - PassiveAuto: 패시브-상시 활성화 ")]
    public ItemType _itemType;
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다.")]
    protected bool _canStackable;
    

    [Header("Item Data")]
    public ItemDataSO _itemData;
    
    [Header("Skill(Ability) Data")]
    public SkillDataSO _itemSkill;

    protected SpriteRenderer _spriteRenderer;

    protected virtual void Init()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _itemData._itemIcon;
    }
}
