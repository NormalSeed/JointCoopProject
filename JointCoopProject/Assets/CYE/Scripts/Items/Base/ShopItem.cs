using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Item, IPickable
{
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다.")]
    private bool isVisibleInInventory;
    public bool _isVisibleInInventory { get { return isVisibleInInventory; } }
    
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    #endregion
    
    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool insertResult = TempManager.inventory.TryBuyItem(this);
        if (insertResult)
        {
            TempManager.inventory.UseCoin(_itemData._itemPrice);
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
