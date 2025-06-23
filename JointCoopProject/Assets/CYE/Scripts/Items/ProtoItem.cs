using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ProtoItem : MonoBehaviour
{
    #region // Item Info
    [SerializeField] private string _itemName; // 아이템 이름
    [SerializeField] private string _itemDescription; // 아이템 설명
    private Sprite _itemSprite; // 아이템 Icon
    #endregion

    #region // Events
    // [HideInInspector] public UnityEvent OnUsed; // 아이템 사용시 발생
    [HideInInspector] public UnityEvent OnAcquired; // 아이템 획득시 발생
    #endregion

    #region // Properties
    protected Sprite ItemSprite { get { return _itemSprite; } private set { _itemSprite = value; } }
    #endregion

    protected void InitItemBase() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        ItemSprite = spriteRenderer.sprite;
    }
    
    protected abstract void Acquire();

}
