using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour, IPickable
{
    public SkillItemType _itemType;
    public SkillItemSO _itemData;
    
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         PickUp(collision.transform);
    //     }
    // }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickUp(collision.transform);
        }
    }
    #endregion

    private void Init()
    {

    }
    
    public void Act(Transform PlayerPos)
    { 
        
    }

    #region // IPickable
    public void PickUp(Transform PlayerPos)
    {
        switch (_itemType)
        {
            case SkillItemType.Active:
                TempManager._player._inventory.GetActiveItem(this, PlayerPos);
                break;
            case SkillItemType.PassiveAttack:
            case SkillItemType.PassiveAuto:
                TempManager._player._inventory.GetPassiveItem(this);
                break;
            default:
                Debug.Log("Skill Item type error.");
                break;
        }
        Destroy(gameObject);
    }
    public void Drop(Transform dropPos)
    {
        Instantiate(_itemData._itemPrefab, dropPos.position, dropPos.rotation);
    }
    #endregion 
}
