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
    private void UpgradePassiveSkill(Transform pickupPos)
    {
        int nextGrade = TempManager.inventory.GetItemSkillGrade(_itemData);
        PlayerSkillManager playerSkillManager = pickupPos.gameObject.GetComponentInChildren<PlayerSkillManager>();
        playerSkillManager.AddSkill(_itemSkill[nextGrade-1]);
    }
    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool insertResult = TempManager.inventory.TryGetItem(this, transform);
        if (insertResult)
        {
            if(_itemData._itemType==ItemType.PassiveAttack||_itemData._itemType==ItemType.PassiveAuto)
                UpgradePassiveSkill(pickupPos);
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
