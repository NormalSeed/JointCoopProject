using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SkillItem : MonoBehaviour, IPickable
{
    [Header("Item Info")]
    [Tooltip("아이템 타입을 결정합니다.\n - Active: 액티브 \n - PassiveAttack: 패시브-공격 강화 \n - PassiveAuto: 패시브-상시 활성화 ")]
    public SkillItemType _itemType;    
    [SerializeField]
    [Tooltip("아이템 중복 획득 가능 여부를 결정합니다.")]
    private bool _canStackable;

    [Header("Item Data")]
    public ItemDataSO _itemData;
    
    [Header("Skill Data")]
    public SkillDataSO _itemSkill;
    
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
        gameObject.GetComponent<SpriteRenderer>().sprite = _itemData._itemIcon;
    }
    
    public void Act(Transform PlayerPos)
    {
        _itemSkill.UseSkill(PlayerPos);
    }

    #region // IPickable
    public void PickUp(Transform PlayerPos)
    {
        switch (_itemType)
        {
            case SkillItemType.Active:
                TempManager._player._inventory.GetActiveItem(this, PlayerPos);
                break;
            case SkillItemType.PassiveAttack:
            case SkillItemType.PassiveAuto:
                TempManager._player._inventory.GetPassiveItem(this);
                break;
            default:
                Debug.Log("Skill Item type error.");
                break;
        }
        Destroy(gameObject);
    }
    #endregion 
}
