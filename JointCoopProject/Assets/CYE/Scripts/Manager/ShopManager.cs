using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : TempSingleton<ShopManager>
{
    [Serializable]
    private struct SellItemSlot
    {
        public ItemDataSO itemData;
        public int maxStock;
        private int curStock;
        public SellItemSlot(ItemDataSO itemData, int sellLimit)
        {
            this.itemData = itemData;
            this.maxStock = sellLimit;
            this.curStock = this.maxStock;
        }
        public bool TryDecreaseCurStock()
        {     
            if (curStock >= 1)
            {
                curStock--;
                return true;
            }
            return false;
        }
    }

    [SerializeField] private List<SellItemSlot> _sellingItems;

    public bool TrySellItem(ItemDataSO itemData)
    {
        foreach (SellItemSlot itemSlot in _sellingItems)
        {
            if (itemSlot.itemData == itemData)
            {
                // 재고 차감 성공 여부를 리턴
                return itemSlot.TryDecreaseCurStock();
            }
        }
        // foreach를 다 돌았는데 return을 못했다면 구매 실패        
        return false;
    }
}
