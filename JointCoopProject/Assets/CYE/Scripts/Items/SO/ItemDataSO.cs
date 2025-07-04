using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/ItemDataSO", order = 1)]
public class ItemDataSO : ScriptableObject
{
    #region // Item Info
    [SerializeField] private int itemID;
    public int _itemID { get { return itemID; } } // readonly

    [Space(10f)]
    public string _itemName;
    public int _itemPrice;
    public Sprite _itemIcon;
    [Multiline(3)]
    public string _itemDesc;
    
    [Space(10f)]
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다.")]
    private bool canStack;
    public bool _canStack { get { return canStack; } }
    #endregion
}