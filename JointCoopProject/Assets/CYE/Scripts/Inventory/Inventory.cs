using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Active Item - Slot 1
    // Trait Item - Slot 5 -> 플레이어에 귀속될 수 있음
    // Passive Item - Slot 1/List
    // Enhance Item - List
    // Expendable(Coin, Bomb) -> 플레이어에 귀속될 수 있음
    ActiveItemSO _activeSlot;
    TraitItemSO[] _traitSlots = new TraitItemSO[5];
    PassiveItemSO _enablePassiveSlot;
    List<PassiveItemSO> _disablePassiveList = new List<PassiveItemSO>();
    List<EnhanceItemSO> _enhanceList = new List<EnhanceItemSO>();
    int _holdingCoins;
    int _holdingBombs;

    
}
