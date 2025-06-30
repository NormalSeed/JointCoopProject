using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSkillItem : MonoBehaviour, IPickable
{
    public NonSkillItemType _itemType;
    public NonSkillItemSO _itemData;
    
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
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
        gameObject.GetComponent<SpriteRenderer>().sprite = _itemData._itemIcon;
    }
    
    public void Act()
    {
        _itemData.Act();
    }

    #region // IPickable
    public void PickUp(Transform PlayerPos)
    {
        TempManager._player._inventory.GetNonSkillItem(this);
        Destroy(gameObject);
    }
    public void Drop(Transform dropPos)
    {
        Instantiate(gameObject, dropPos.position, dropPos.rotation);
    }
    #endregion 
}
