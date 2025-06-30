using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int ITEM_SLOT = 12;
    private SkillItem _activeItem = null;
    private ItemDataSO _activeItemData;
    private List<ItemDataSO> _itemList = new List<ItemDataSO>(ITEM_SLOT);
    private List<ItemDataSO> _enhanceList = new List<ItemDataSO>();

    public int _coin;
    public int _bomb;

    public void GetActiveItem(SkillItem item, Transform _currentPos)
    {
        if (_activeItem is not null)
        {
            Instantiate(_activeItem, _currentPos.position, _currentPos.rotation);
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
            // PlayerPos.gameObject.GetComponentInParent<PlayerSkillManager>().AddSkill(_itemSkill);
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
