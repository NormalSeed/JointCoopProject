using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct ItemSlot
{
    public ItemDataSO itemDataSO;
    public int itemStackCount;
    public ItemSlot(ItemDataSO itemDataSO, int itemStackCount)
    {
        this.itemDataSO = itemDataSO;
        this.itemStackCount = itemStackCount;
    }
    public void UpgradeStackCount()
    {
        itemStackCount++;
        Debug.Log("1");
        if (itemStackCount > 5)
        {
            itemStackCount = 5;
        }
    }
}
public class InventoryManager : TempSingleton<InventoryManager>
{
    private const int SLOT_COUNT = 12;

    // UI visible
    [SerializeField]
    private GameItem activeItem;
    public GameItem _activeItem { get { return activeItem; } private set { activeItem = value; } }
    public ItemDataSO _activeItemData;
    public SkillDataSO _activeSkillData;
    [SerializeField]
    private List<GameItem> _activeItemPool;

    public List<ItemSlot> _visItemList = new List<ItemSlot>(SLOT_COUNT);

    // UI invisible
    public List<ItemSlot> _invItemList = new List<ItemSlot>();

    // Expend Items Info
    private int coinCount = 1000;
    public int _coinCount { get { return coinCount; } private set { coinCount = value; } }
    [SerializeField] private GameObject _coinPrefab;
    private int bombCount;
    public int _bombCount { get { return bombCount; } private set { bombCount = value; } }
    [SerializeField] private GameObject _bombPrefab;

    private float _activeCooldownTimer = 0f;
    private float _activeDurationTimer = 0f;

    [SerializeField] private Image _cooldownImage;

    private Coroutine _skillCooldownRoutine;
    private Coroutine _skillDurationRoutine;

    public bool TryGetItem(GameItem insertItem, Transform pickupPos)
    {
        bool insertResult = false;
        switch (insertItem._itemData._itemType)
        {
            case ItemType.Active:
                DropPrevActiveItem(_activeItemData, pickupPos);

                _activeItemData = insertItem._itemData;
                _activeSkillData = insertItem._itemSkill[0];

                _activeCooldownTimer = 0f;
                _activeDurationTimer = 0f;

                StopCountActiveCooldown();
                StopCountActiveDuration();

                insertResult = true;
                break;
            case ItemType.PassiveAttack:
            case ItemType.PassiveAuto:
                foreach (ItemSlot item in _visItemList)
                {
                    if (insertItem._itemData._itemID == item.itemDataSO._itemID)
                    {
                        _visItemList.Remove(item);
                        ItemSlot upgradeItem = new ItemSlot(insertItem._itemData, item.itemStackCount + 1);
                        _visItemList.Add(upgradeItem);
                        insertResult = true;
                        break; // >> foreach break;
                    }
                }
                if (!insertResult && _visItemList.Count < _visItemList.Capacity)
                {
                    ItemSlot newItem = new ItemSlot(insertItem._itemData, 1);
                    _visItemList.Add(newItem);
                    insertResult = true;
                }
                break;
            default:
                break;
        }
        return insertResult;
    }
    public bool TryBuyItem(ShopItem insertItem)
    {
        bool insertResult = false;
        if (_coinCount >= insertItem._itemData._itemPrice)
        {
            if (insertItem._isVisibleInInventory)
            {
                insertResult = InsertBoughtItemToList(insertItem, insertItem._itemData._canStack, insertItem._isVisibleInInventory);
            }
            else
            {
                insertResult = InsertBoughtItemToList(insertItem, insertItem._itemData._canStack);
            }

            if (insertResult)
            {
                insertItem._itemSkill[0].UseSkill(insertItem.transform, out bool useSkillResult);
                insertResult = useSkillResult;
            }
        }
        return insertResult;
    }
    private bool InsertBoughtItemToList(Item insertItem, bool stackable = false, bool checkCapacity = false)
    {
        bool insertResult = false;
        if (stackable && checkCapacity)
        {
            foreach (ItemSlot item in _visItemList)
            {
                if (insertItem._itemData._itemID == item.itemDataSO._itemID)
                {
                    item.UpgradeStackCount();
                    insertResult = true;
                    break;
                }
            }
        }
        else if (stackable && !checkCapacity)
        {
            foreach (ItemSlot item in _invItemList)
            {
                if (insertItem._itemData._itemID == item.itemDataSO._itemID)
                {
                    item.UpgradeStackCount();
                    insertResult = true;
                    break;
                }
            }
        }
        else if (!stackable && checkCapacity) // 사실상 stackable 체크는 필요없음
        {
            if (_visItemList.Count < _visItemList.Capacity)
            {
                ItemSlot newItem = new ItemSlot(insertItem._itemData, 1);
                _visItemList.Add(newItem);
                insertResult = true;
            }
        }
        else if (!stackable && !checkCapacity) // 사실상 stackable 체크는 필요없음
        {
            ItemSlot newItem = new ItemSlot(insertItem._itemData, 1);
            _invItemList.Add(newItem);
            insertResult = true;
        }
        return insertResult;
    }
    public void GetCoin(int getAmount)
    {
        _coinCount += getAmount;
    }
    public void UseCoin(int useAmount)
    {
        Debug.Log($"{_coinCount}");
        _coinCount -= useAmount;
    }
    public void GetBomb(int getAmount)
    {
        _bombCount += getAmount;
    }
    public void UseBomb(Transform playerPos)
    {
        if (_bombCount >= 1)
        {
            _bombCount -= 1;
            _bombPrefab.GetComponent<Bomb>().Install(playerPos);
        }
    }
    public int GetItemSkillGrade(ItemDataSO _itemData)
    {
        int grade = 0;
        switch (_itemData._itemType)
        {
            case ItemType.Active:
            case ItemType.shop:
                grade = 1;
                break;
            case ItemType.PassiveAttack:
            case ItemType.PassiveAuto:
                foreach (ItemSlot item in _visItemList)
                {
                    if (_itemData._itemID == item.itemDataSO._itemID)
                    {
                        grade = item.itemStackCount;
                        Debug.Log($"{grade}");
                        break; // >> foreach break;
                    }
                }
                break;
            case ItemType.Expend:
                break;
            default:
                break;
        }
        return grade;
    }
    public void UseActiveSkill(Transform usePos)
    {
        if (_activeSkillData != null && _activeCooldownTimer <= 0f && _activeDurationTimer <= 0f)
        {
            _activeSkillData.UseSkill(usePos);
            _activeCooldownTimer = _activeSkillData.skillCooldown;

            if (_skillCooldownRoutine == null)
            {
                _skillCooldownRoutine = StartCoroutine(CountSkillCooltime());
            }
            if (_skillDurationRoutine == null)
            {
                _skillDurationRoutine = StartCoroutine(CountSkillDuration());
            }
        }
    }
    private void DropPrevActiveItem(ItemDataSO _itemData, Transform dropPos)
    {
        if (_itemData != null)
        {
            foreach (GameItem item in _activeItemPool)
            {
                if (item._itemData == _itemData)
                {
                    item._itemSkill[0].ReleaseSkill();
                    item.Drop(dropPos);
                }
            }
        }
    }

    private IEnumerator CountSkillCooltime()
    {
        while (_activeCooldownTimer > 0f)
        {
            _activeCooldownTimer -= Time.deltaTime;
            // _cooldownImage.fillAmount = _activeSkillTimer / _activeSkillData.skillCooldown;
            
            yield return new WaitForFixedUpdate();
        }

        StopCountActiveCooldown();
    }
    private void StopCountActiveCooldown()
    {
        if (_skillCooldownRoutine != null)
        {
            StopCoroutine(_skillCooldownRoutine);
            _skillCooldownRoutine = null;
        }
    }
        private IEnumerator CountSkillDuration()
    {
        while (_activeDurationTimer > 0f)
        {
            _activeDurationTimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if(TempManager.inventory._activeSkillData.name!="Dash Attack")
            StopCountActiveDuration();
    }
    private void StopCountActiveDuration()
    {
        if (_skillDurationRoutine != null)
        {
            StopCoroutine(_skillDurationRoutine);
            _skillDurationRoutine = null;
        }
    }
}
