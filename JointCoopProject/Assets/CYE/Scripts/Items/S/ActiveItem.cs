using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillItem : MonoBehaviour, IPickable // SkillItem
{
    public ItemDataSO _itemData;

    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickedUp();
        }
    }
    #endregion

    #region // funciton
    private void Init()
    {

    }
    public void Use(Transform playerPos)
    {
        _itemData.Act(playerPos);
    }
    #endregion

    #region // IPickable
    public void PickedUp()
    {
        // TempManager._InventoryManager._inventory.GetActiveItem()
        Destroy(gameObject, 0.1f);
    }
    public void Drop(Transform itemPos)
    {
        GameObject droppedItem = Instantiate(gameObject, itemPos.position, itemPos.rotation);
        droppedItem.SetActive(true);
    }
    #endregion
}
