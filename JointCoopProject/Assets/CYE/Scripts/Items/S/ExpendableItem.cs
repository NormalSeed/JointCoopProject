using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 소모성 재화 아이템.
/// </summary>
public class ExpendableItem : MonoBehaviour, IPickable
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
