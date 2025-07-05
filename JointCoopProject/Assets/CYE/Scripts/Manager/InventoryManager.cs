using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : TempSingleton<InventoryManager>
{
    private const int SLOT_COUNT = 12;

    // UI visible
    private GameItem activeItem;
    public GameItem _activeItem { get { return activeItem; } set { activeItem = value; } }
    private ItemDataSO _activeItemData;

    private List<ItemSlot> _visItemList = new List<ItemSlot>(SLOT_COUNT);

    // UI invisible
    private List<ItemSlot> _invItemList = new List<ItemSlot>();

    // Expend Items Info
    private int coinCount;
    public int _coinCount { get { return coinCount; } private set { coinCount = value; } }
    [SerializeField] private GameObject _coinPrefab;
    private int bombCount;
    public int _bombCount { get { return bombCount; } private set { bombCount = value; } }
    [SerializeField] private GameObject _bombPrefab;

    [System.Serializable]
    private struct ItemSlot
    {
        public ItemDataSO itemDataSO;
        public int itemStackCount;
        public ItemSlot(ItemDataSO itemDataSO, int itemStackCount)
        {
            this.itemDataSO = itemDataSO;
            this.itemStackCount = itemStackCount;
        }
        public void UpgradeStackCount()
        {
            itemStackCount++;
            if (itemStackCount > 5)
            {
                itemStackCount = 5;
            }
        }
    }

    public bool TryGetItem(GameItem insertItem, Transform pickupPos)
    {
        bool insertResult = false;
        switch (insertItem._itemData._itemType)
        {
            case ItemType.Active:
                if (_activeItem != null)
                {
                    _activeItem.Drop(pickupPos);
                }
                _activeItem = insertItem;
                _activeItemData = insertItem._itemData;
                insertResult = true;
                break;
            case ItemType.PassiveAttack:
            case ItemType.PassiveAuto:
                foreach (ItemSlot item in _visItemList)
                {
                    if (insertItem._itemData._itemID == item.itemDataSO._itemID)
                    {
                        item.UpgradeStackCount();
                        insertResult = true;
                        break; // >> foreach break;
                    }
                }
                if (!insertResult && _visItemList.Count < _visItemList.Capacity)
                {
                    ItemSlot newItem = new ItemSlot(insertItem._itemData, 1);
                    _visItemList.Add(newItem);
                    insertResult = true;
                }
                break;
            default:
                break;
        }
        return insertResult;
    }
    public bool TryBuyItem(ShopItem insertItem)
    {
        bool insertResult = false;
        if (_coinCount > insertItem._itemData._itemPrice)
        {
            if (insertItem._isVisibleInInventory)
            {
                insertResult = InsertBoughtItemToList(insertItem, ref _visItemList, insertItem._itemData._canStack, insertItem._isVisibleInInventory);
            }
            else
            {
                insertResult = InsertBoughtItemToList(insertItem, ref _invItemList, insertItem._itemData._canStack);
            }
        }
        return insertResult;
    }
    private bool InsertBoughtItemToList(Item insertItem, ref List<ItemSlot> insertedList, bool stackable = false, bool checkCapacity = false)
    {
        bool insertResult = false;
        if (stackable)
        {
            foreach (ItemSlot item in insertedList)
            {
                if (insertItem._itemData._itemID == item.itemDataSO._itemID)
                {
                    item.UpgradeStackCount();
                    insertItem._itemSkill[0].UseSkill(insertItem.transform);
                    insertResult = true;
                    break;
                }
            }
        }
        else
        {
            if (!checkCapacity || insertedList.Count < insertedList.Capacity)
            {
                ItemSlot newItem = new ItemSlot(insertItem._itemData, 1);
                insertedList.Add(newItem);
                insertItem._itemSkill[0].UseSkill(insertItem.transform);
                insertResult = true;
            }
        }
        return insertResult;
    }
    public void GetCoin(int getAmount)
    {
        _coinCount += getAmount;
    }
    public void UseCoin(int useAmount)
    {
        _coinCount -= useAmount;
    }
    public void GetBomb(int getAmount)
    {
        _bombCount += getAmount;
    }
    public void UseBomb(Transform playerPos)
    {
        if (_bombCount >= 1)
        {
            _bombCount -= 1;
            _bombPrefab.GetComponent<Bomb>().Install(playerPos);
        }
    }
    public int GetItemSkillGrade(ItemDataSO _itemData)
    {
        int grade = 0;
        switch (_itemData._itemType)
        {
            case ItemType.Active:
            case ItemType.shop:
                grade = 1;
                break;
            case ItemType.PassiveAttack:
            case ItemType.PassiveAuto:
                foreach (ItemSlot item in _visItemList)
                {
                    if (_itemData._itemID == item.itemDataSO._itemID)
                    {
                        grade = item.itemStackCount;
                        break; // >> foreach break;
                    }
                }
                break;
            case ItemType.Expend:
                break;
            default:
                break;
        }
        return grade;
    }

}
