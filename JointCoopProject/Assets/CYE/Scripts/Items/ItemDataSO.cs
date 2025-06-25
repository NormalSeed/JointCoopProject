using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    active, passive, trait, enhance, expendable
}
public enum StatChangeMethod
{ 
    sum, multiplication
}

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
    public virtual void Act(Transform usePos)
    {
        // 동작함
        _skillData.UseSkill(usePos);
    }
    public virtual void Interact()
    { 
        // 상호작용함
    }
}