using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자가 임의로 사용이 가능하며, 사용시 효과(스킬)가 발동하는 아이템.
/// </summary>
public class ActiveItem : MonoBehaviour, IPickable
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

    #region // IPickable
    public void PickedUp()
    {

    }
    public void Drop(Transform itemPos)
    {

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
}
