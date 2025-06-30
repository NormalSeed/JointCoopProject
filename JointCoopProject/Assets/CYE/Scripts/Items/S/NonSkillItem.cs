using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSkillItem : MonoBehaviour, IPickable
{
    public NonSkillItemType _itemType;
    public ItemDataSO _itemData;

    // 수치를 변경시키고자하는 타겟 
    // public UnityEvent<float, StatChangeMethod> _changeTargetFunction;
    public ChangeTarget _changetarget;
    // 변경할 수치
    public float _changeValue;
    
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
    
    public void Act()
    {
        switch (_changetarget)
        {
            case ChangeTarget.CurHp:
                TempManager._player._status._playerHp += (int)_changeValue;
                break;
            case ChangeTarget.AttackPoint:
                TempManager._player._status._attackDamage += (int)_changeValue;
                break;
            case ChangeTarget.AttackSpeed:
                TempManager._player._status._attackSpeed += (int)_changeValue;
                break;
            case ChangeTarget.MoveSpeed:
                TempManager._player._status._moveSpeed += (int)_changeValue;
                break;
            default:
                break;
        }
    }

    #region // IPickable
    public void PickUp(Transform PlayerPos)
    {
        TempManager._player._inventory.GetNonSkillItem(this);
        Destroy(gameObject);
    }
    #endregion 
}
