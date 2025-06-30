using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataSO : ScriptableObject
{
    #region // Item Info
    [SerializeField] private int _itemID;
    public GameObject _itemPrefab; // 재귀적 참조가.. 발생할수도?

    [Space(10f)]
    public string _itemName;
    public int _itemPrice;
    public Sprite _itemIcon;
    [Multiline(3)]
    public string _itemDesc;
    #endregion

    public virtual void Act()
    {
        // 동작함
    }
    public virtual void Act(Transform currentPosition)
    {
        // 동작함
    }
}