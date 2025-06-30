using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int ITEM_SLOT = 12;
    private SkillItem _activeItem = null;
    private SkillItemSO _activeItemData;
    private List<SkillItemSO> _itemList = new List<SkillItemSO>(ITEM_SLOT);
    private List<NonSkillItemSO> _enhanceList = new List<NonSkillItemSO>();

    public void GetActiveItem(SkillItem item, Transform _currentPos)
    {
        if (_activeItem is not null)
        {
            _activeItem.Drop(_currentPos);
            _activeItem = null;
        }
        _activeItem = item;
        _activeItemData = item._itemData;
    }

    public void UseActiveItem(Transform _currentPos)
    {
        _activeItem.Act(_currentPos);
    }

    public void GetPassiveItem(SkillItem item)
    {
        if (_itemList.Count < _itemList.Capacity)
        {
            _itemList.Add(item._itemData);
            // PlayerSkillManager.AddSkill(item._itemData._itemSkill);
        }
    }

    public void GetNonSkillItem(NonSkillItem item)
    {
        item.Act();
        if (item._itemType == NonSkillItemType.Enhance)
        { 
            _enhanceList.Add(item._itemData);
        }
    }

}
