using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 획득시 사용자의 수치를 상승 또는 하락시키는 아이템.
/// </summary>
public class EnhanceItem : MonoBehaviour, IPickable
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
    #endregion
}
