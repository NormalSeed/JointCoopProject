using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(menuName = "Items/ItemDataSO", order = 1)]
public class ItemDataSO : ScriptableObject
{
    #region // Item Info
    [SerializeField] private int _itemID;

    [Space(10f)]
    public string _itemName;
    public int _itemPrice;
    public Sprite _itemIcon;
    [Multiline(3)]
    public string _itemDesc;
    #endregion
}