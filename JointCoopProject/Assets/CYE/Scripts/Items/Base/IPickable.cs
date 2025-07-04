using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    public void PickUp(Transform pickupPos);
    public void Drop(Transform dropPos);
}
public enum ItemType
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
public interface IInstallable
{
    public void Install(Transform setUpPos);
}