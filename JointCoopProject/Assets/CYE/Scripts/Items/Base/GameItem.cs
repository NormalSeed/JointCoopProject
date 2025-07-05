using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public class GameItem : Item, IPickable
{
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
        bool insertResult = TempManager.inventory.TryGetItem(this, transform);
        if (insertResult) // true - 아이템 획득 성공, false - 아이템 획득 실패
        {
            PlayerSkillManager playerSkillManager = pickupPos.gameObject.GetComponentInChildren<PlayerSkillManager>();
            playerSkillManager.AddSkill(_itemSkill);
            Destroy(gameObject);
        }
    }
    public void Drop(Transform dropPos)
    {
        GameObject itemObject = Instantiate(gameObject, dropPos.position, Quaternion.identity);
        itemObject.SetActive(true);
        itemObject.GetComponent<Rigidbody2D>().AddForce(0.5f * transform.forward);
    }
    #endregion 
}
