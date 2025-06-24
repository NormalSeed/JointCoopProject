using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitSkill : SkillItem, IPickable
{
    private int grade;
    public int _grade { get { return grade; } private set { value = _grade; } }

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

    #region // funciton
    private void Init()
    {
        _itemType = SkillItemType.trait;
        _grade = 0;
    }

    private void UpgradeSkillGrade()
    {
        _grade++;
        if (_grade > 5) { _grade = 5; }
        
    }
    #endregion

    // 아이템 주움
    public void PickedUp()
    {
        // player
        Destroy(gameObject, 0.1f);
    }
    
    // 아이템 떨굼
    public void Drop(Transform itemPos) { }
}
