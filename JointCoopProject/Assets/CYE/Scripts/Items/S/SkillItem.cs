using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour, IPickable
{
    public SkillItemType _itemType;
    public int _itemGrade;
    public ItemDataSO _itemData;
    
    [Header("Skill Settings")]
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
