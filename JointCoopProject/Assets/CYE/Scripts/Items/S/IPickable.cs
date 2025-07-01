using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    public void PickUp(Transform pickupPos);
    public void Drop(Transform dropPos);
}
public enum SkillItemType
{
    Active, PassiveAttack, PassiveAuto, shop, Expend
}
public enum NonSkillItemType
{ 
    Enhance, Expend
}
public enum ChangeTarget
{
    CurHp, AttackPoint, AttackSpeed, MoveSpeed, Luck
}