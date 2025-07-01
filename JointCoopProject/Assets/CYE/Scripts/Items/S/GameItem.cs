using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public class GameItem : MonoBehaviour, IPickable
{
    [Header("Item Info")]
    [Tooltip("아이템 타입을 결정합니다.\n - Active: 액티브 \n - PassiveAttack: 패시브-공격 강화 \n - PassiveAuto: 패시브-상시 활성화 ")]
    public SkillItemType _itemType;
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다.")]
    private bool _canStackable;
    

    [Header("Item Data")]
    public ItemDataSO _itemData;
    
    [Header("Skill(Ability) Data")]
    public SkillDataSO _itemSkill;

    private SpriteRenderer _spriteRenderer;
    
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickUp(collision.transform);
        }
    }
    #endregion

    private void Init()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _itemData._itemIcon;
    }
    
    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool insertResult = TempManager._inventory.TryGetItem(this);
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
}
