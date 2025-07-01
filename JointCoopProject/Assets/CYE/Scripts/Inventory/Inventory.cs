using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private struct ItemSlot
    {
        ItemDataSO itemDataSO;
        int itemCount;
    }

    private const int SLOT_COUNT = 12;

    // UI visible
    private SkillItem _activeItem = null;
    private ItemDataSO _activeItemData;
    private List<ItemSlot> _itemList = new List<ItemSlot>(SLOT_COUNT);

    // UI invisible
    private List<ItemSlot> _invItemList = new List<ItemSlot>();

    public int _coin;
    public int _bomb;

    public bool TryGetItem(IPickable item)
    {
        return true;
    }

    private void AddStackableItem(ItemDataSO itemData)
    {
        foreach (ItemSlot item in _itemList)
        {

        }
    }
    private void GetActiveItem(SkillItem item, Transform _currentPos)
    {
        if (_activeItem is not null)
        {
            Instantiate(_activeItem, _currentPos.position, _currentPos.rotation);
            _activeItem = null;
        }
        _activeItem = item;
        _activeItemData = item._itemData;
    }

    private void UseActiveItem(Transform _currentPos)
    {
        _activeItem.Act(_currentPos);
    }

    private void GetPassiveItem(SkillItem item)
    {
        if (_itemList.Count < _itemList.Capacity)
        {
            _itemList.Add(item._itemData);
            // PlayerPos.gameObject.GetComponentInParent<PlayerSkillManager>().AddSkill(_itemSkill);
        }
    }

    private void GetNonSkillItem(NonSkillItem item)
    {
        _invItemList.Add(item._itemData);
    }
}
