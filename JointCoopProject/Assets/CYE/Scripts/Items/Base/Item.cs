using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item Data")]
    public ItemDataSO _itemData;
    
    [Header("Skill(Ability) Data")]
    public SkillDataSO[] _itemSkill = new SkillDataSO[5];

    protected SpriteRenderer _spriteRenderer;

    protected virtual void Init()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _itemData._itemIcon;
        _spriteRenderer.color = Color.white;
    }
}
