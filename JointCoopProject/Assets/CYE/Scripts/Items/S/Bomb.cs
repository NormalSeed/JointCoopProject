using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : Item, IPickable
{
    [Header("Item Data")]
    [SerializeField]
    private float _explosiveDelay;
    [SerializeField]
    private float _explosiveDamage;

    private bool _isSetUp;


    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isSetUp && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickUp(collision.transform);
        }
    }
    #endregion

    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool insertResult = TempManager.inventory.TryGetItem(this);
        if (insertResult) // true - 아이템 획득 성공, false - 아이템 획득 실패
        {
            Destroy(gameObject);
        }
    }
    public void Drop(Transform dropPos)
    {
        GameObject itemObject = Instantiate(gameObject, dropPos.position, Quaternion.identity);
        itemObject.SetActive(true);
        itemObject.GetComponent<Rigidbody2D>().AddForce(0.5f * transform.forward);
    }
    #endregion

    #region // Member Function
    private void SetUp(Transform datumPoint)
    {
        RaycastHit2D checkForward = Physics2D.Raycast(datumPoint.position, datumPoint.forward, 1f);
        if (checkForward)
        {

        }
    }
    #endregion
}
