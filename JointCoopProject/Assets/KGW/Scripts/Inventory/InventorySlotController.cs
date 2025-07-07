using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotController : MonoBehaviour
{
    [SerializeField] ItemSlotUI[] _itemSlot;    // 인벤토리 UI Slot

    public void ItemSlotUIUpdate()
    {
        var items = TempManager.inventory._visItemList;

        for(int i = 0; i < _itemSlot.Length; i++)
        {
            if (i < items.Count)
            {
                _itemSlot[i].SetItemSlot(items[i]); // 슬롯에 아이템 정보 저장
            }
            else
            {
                _itemSlot[i].SetItemSlot(new ItemSlot());   // 슬롯에 아이템 정보 저장
            }
        }
    }
}
