using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : Item, IPickable
{
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다. 체크하면 획득시 인벤토리에 표시됩니다.")]
    private bool isVisibleInInventory;
    public bool _isVisibleInInventory { get { return isVisibleInInventory; } }

    [SerializeField]
    [Tooltip("아이템 하단에 가격 표시 여부를 결정합니다. 체크하면 가격이 표시되지 않습니다.")]
    private bool showPrice;
    public bool _showPrice { get { return showPrice; } }

    private TMP_Text _priceTag;

    #region // Unity Message Function
    void Awake()
    {
        Init();
        _priceTag = GetComponentInChildren<TMP_Text>();
    }
    void OnEnable()
    { 
        _priceTag.text = (_showPrice)?$"{_itemData._itemPrice}C":"";   
    }
    #endregion

    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool buyResult = ItemManager.shop.TrySellItem(this._itemData);
        if (buyResult)
        {
            bool insertResult = ItemManager.inventory.TryBuyItem(this);
            if (insertResult)
            {
                ItemManager.inventory.UseCoin(_itemData._itemPrice);

                UIManager.Instance.SetActiveItemImage(_itemData._itemIcon);   // 획득한 액티브 아이템의 이미지 저장
                UIManager.Instance._itemGuageController.SetCoolTime(_itemSkill[0].skillCooldown);    // 액비트 아이템 쿨타임 저장
                UIManager.Instance._itemGuageController._canUseItem = true;

                // 획득한 액티브 아이템 정보 UI 출력
                ItemManager.inventory._timer = ItemManager.inventory._skillTitleTextTime;   // UI 오픈마다 타이머 초기화
                ItemManager.inventory._isSkillTitleOpen = true;

                GameObject getActiveItem = UIManager.Instance.GetUI(UIKeyList.itemInfo);
                TMP_Text[] activeItemText = getActiveItem.GetComponentsInChildren<TMP_Text>(true);
                UIManager.Instance.OpenUi(UIKeyList.itemInfo);
                activeItemText[0].text = _itemData._itemName;
                activeItemText[1].text = _itemData._itemDesc;
                
                if (_itemData._itemPrice == 0)
                {
                    Destroy(gameObject);
                }
            }    
        }
    }
    public void Drop(Transform dropPos)
    {
        GameObject itemObject = Instantiate(gameObject, dropPos.position, Quaternion.identity);
        itemObject.SetActive(true);
        itemObject.GetComponent<Rigidbody2D>().AddForce(0.5f * transform.forward);
    }
    #endregion 
    
}
