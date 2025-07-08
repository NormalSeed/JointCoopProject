using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public class Coin : Item, IPickable
{    
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    #endregion
    
    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        TempManager.inventory.GetCoin(_itemData._itemPrice);
        Destroy(gameObject);
    }
    public void Drop(Transform dropPos)
    {
        GameObject itemObject = Instantiate(gameObject, dropPos.position, Quaternion.identity);
        itemObject.SetActive(true);
        itemObject.GetComponent<Rigidbody2D>().AddForce(0.5f * transform.forward);
    }
    #endregion 
}
