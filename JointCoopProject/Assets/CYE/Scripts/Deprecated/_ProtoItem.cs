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

    #region // Item Events
    [HideInInspector] public UnityEvent OnAcquired; // 아이템 획득시 발생
    #endregion

    #region // Item Properties
    protected Sprite ItemSprite { get { return _itemSprite; } private set { _itemSprite = value; } }
    #endregion

    // 아이템 기본 정보 초기화
    protected void InitItemBase()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        ItemSprite = spriteRenderer.sprite;
    }

    // 아이템 획득
    protected abstract void Acquire();

    // 아이템 버림
    public void Drop(Vector2 itemPos)
    {
        Vector3 newItemPosition = new Vector3(itemPos.x, itemPos.y, 0);
        GameObject item = Instantiate(gameObject, newItemPosition, Quaternion.identity);
        // 아이템 드랍시 효과 혹은 애니메이션 재생
    }
}
