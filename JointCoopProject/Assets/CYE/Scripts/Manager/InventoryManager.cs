using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : TempSingleton<InventoryManager>
{
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

    private const int SLOT_COUNT = 12;

    // UI visible
    private GameItem _activeItem = null;
    private ItemDataSO _activeItemData;
    private List<ItemSlot> _visItemList = new List<ItemSlot>(SLOT_COUNT);

    // UI invisible
    private List<ItemDataSO> _invItemList = new List<ItemDataSO>();

    public int _coin;
    public int _bomb;

    public bool TryGetItem(GameItem insertItem)
    {
        bool insertResult = false;
        switch (insertItem._itemType)
        {
            case SkillItemType.Active:
                if (_activeItem != null)
                {
                    // _activeItem.Drop(PlayerTransform);
                }
                _activeItem = insertItem;
                _activeItemData = insertItem._itemData;
                insertResult = true;
                break;
            case SkillItemType.PassiveAttack:
            case SkillItemType.PassiveAuto:
                foreach (ItemSlot item in _visItemList)
                {
                    if (insertItem._itemData._itemID == item.itemDataSO._itemID)
                    {
                        item.UpgradeStackCount();
                        insertResult = true;
                        break;
                    }
                }
                if (_visItemList.Count < _visItemList.Capacity)
                {
                    ItemSlot newItem = new ItemSlot(insertItem._itemData, 1);
                    _visItemList.Add(newItem);
                    insertResult = true;
                    break;
                }
                break;
            case SkillItemType.shop:
                _invItemList.Add(insertItem._itemData);
                break;
            case SkillItemType.Expend:
                break;
            default:
                break;
        }
        return insertResult;
    }
}
