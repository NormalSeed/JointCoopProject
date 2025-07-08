using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotController : MonoBehaviour
{
    [SerializeField] ItemSlotUI[] _itemSlot;    // �κ��丮 UI Slot

    public void ItemSlotUIUpdate()
    {
        var items = ItemManager.inventory._visItemList;

        for(int i = 0; i < _itemSlot.Length; i++)
        {
            if (i < items.Count)
            {
                _itemSlot[i].SetItemSlot(items[i]); // ���Կ� ������ ���� ����
            }
            else
            {
                _itemSlot[i].SetItemSlot(new ItemSlot());   // ���Կ� ������ ���� ����
            }
        }
    }
}
