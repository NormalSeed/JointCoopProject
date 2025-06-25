using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Scriptable Objects/Item Data Object")]
public class ItemDataSO : ScriptableObject
{
    #region // Item Info
    public int _itemID;
    public string _itemName;
    public SkillItemType _itemType;
    public int _itemPrice;
    public Sprite _itemIcon;
    [TextArea]
    public string _itemDesc;


    [Header("Skill Settings")]
    public SkillDataSO _skillData;
    #endregion

    public virtual void PickedUp()
    {
        // 주워짐
    }
    public virtual void Dropped()
    {
        // 버려짐
    }
    public virtual void Act()
    {
        // 동작함
    }
    public virtual void Interact()
    { 
        // 상호작용함
    }
}