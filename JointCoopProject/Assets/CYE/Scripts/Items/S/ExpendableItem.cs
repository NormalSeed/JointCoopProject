using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpendableItem : MonoBehaviour
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
    private void PickedUp()
    {
        
    }
    #endregion
}
