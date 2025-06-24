using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillItem : SkillItem, IPickable
{
    private bool _IsActivated;

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
        _itemType = SkillItemType.passive;

    }
    #endregion

    // 아이템 주움
    public void PickedUp()
    {
        Destroy(gameObject, 0.1f);
    }
    
    // 아이템 떨굼
    public void Drop(Transform itemPos) { }
}
