using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자가 임의로 사용이 불가능한, 효과(스킬)가 자동으로 발생하는 아이템.
/// </summary>
public class PassiveItem : MonoBehaviour, IPickable
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
