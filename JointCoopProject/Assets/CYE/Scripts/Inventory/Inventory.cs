using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    // Active Item - Slot 1
    // Trait Item - Slot 5 -> 플레이어에 귀속될 수 있음
    // Passive Item - List
    // Enhance Item - List
    // Expendable(Coin, Bomb) -> 플레이어에 귀속될 수 있음
    private const int MIN_ITEM_SLOT = 12;
    private const int MAX_ITEM_SLOT = 18;
    public IPickable _activeSlot = null;
    // TraitItemSO[] _traitSlots = new TraitItemSO[5];
    // List<PassiveItemSO> _passiveList = new List<PassiveItemSO>();
    // List<EnhanceItemSO> _enhanceList = new List<EnhanceItemSO>();
    public List<IPickable> _itemList = new List<IPickable>(MIN_ITEM_SLOT);

    int _holdingCoins = 0;
    int _holdingBombs = 0;

    public void GetActiveItem(IPickable item)
    {
        if (_activeSlot is not null)
        {
            // _activeSlot.Drop(TempManager._)
        }
    }

    public void UseActiveItem()
    {

    }

    public void GetPassiveItem(IPickable item)
    {

    }

    public void IncreasePassiveSlot()
    {

    }

    public void UpgradePassiveItem(IPickable item)
    { 
        
    }

}
