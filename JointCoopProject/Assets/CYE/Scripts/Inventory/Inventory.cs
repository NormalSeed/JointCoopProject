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
    public IPickable _activeSlot = null;
    // TraitItemSO[] _traitSlots = new TraitItemSO[5];
    // List<PassiveItemSO> _passiveList = new List<PassiveItemSO>();
    // List<EnhanceItemSO> _enhanceList = new List<EnhanceItemSO>();
    public List<IPickable> _itemList = new List<IPickable>(18);

    int _holdingCoins = 0;
    int _holdingBombs = 0;

    public void GetActiveItem(IPickable item)    
    {
        if (_activeSlot is not null)
        { 
            // _activeSlot.Drop(TempManager._)
        }
    }
}
