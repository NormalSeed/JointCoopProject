using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item, IPickable
{
    private enum HeartGauge
    { 
        Half = 1, Full = 2
    }
    [SerializeField] private HeartGauge _hpUpValue;

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
        PlayerStatManager.Instance._playerHp += (int)_hpUpValue;
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
