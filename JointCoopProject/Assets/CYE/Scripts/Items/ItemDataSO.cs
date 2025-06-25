using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Scriptable Objects/Item Data Object")]
public class ItemDataSO : ScriptableObject
{
    #region // Item Info
    public int _itemID;
    public string _itemName;
    // private SkillItemType itemType;
    // public SkillItemType _itemType { get { return itemType; } protected set { itemType = value; } }
    public int _itemPrice;
    public Sprite _itemIcon;
    [TextArea]
    public string _itemDesc;
    #endregion

    public virtual void PickedUp()
    {
        // 주워짐
    }
    public virtual void Dropped(Transform currentPosition)
    {
        // 버려짐
    }
    public virtual void Act(Transform currentPosition)
    {
        // 동작함
    }
    public virtual void Interact()
    { 
        // 상호작용함
    }
}