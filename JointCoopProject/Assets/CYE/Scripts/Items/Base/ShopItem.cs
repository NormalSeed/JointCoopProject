using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Item, IPickable
{
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다. 체크하면 획득시 인벤토리에 표시됩니다.")]
    private bool isVisibleInInventory;
    public bool _isVisibleInInventory { get { return isVisibleInInventory; } }

    [SerializeField]
    [Tooltip("아이템 하단에 가격 표시 여부를 결정합니다. 체크하면 가격이 표시되지 않습니다.")]
    private bool showPrice;
    public bool _showPrice { get { return showPrice; } }
    
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         PickUp(collision.transform);
    //     }
    // }
    #endregion
    
    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool insertResult = TempManager.inventory.TryBuyItem(this);
        if (insertResult)
        {
            TempManager.inventory.UseCoin(_itemData[0]._itemPrice);
        }
    }
    public void Drop(Transform dropPos) // 사실 필요없음
    {
        GameObject itemObject = Instantiate(gameObject, dropPos.position, Quaternion.identity);
        itemObject.SetActive(true);
        itemObject.GetComponent<Rigidbody2D>().AddForce(0.5f * transform.forward);
    }
    #endregion 
    
}
