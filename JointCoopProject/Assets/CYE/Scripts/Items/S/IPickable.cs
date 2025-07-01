using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    // 아이템 주움
    public void PickUp(Transform PlayerPos);
}
public enum SkillItemType
{
    Active, PassiveAttack, PassiveAuto
}
public enum NonSkillItemType
{ 
    Enhance, Expend
}
public enum ChangeTarget
{
    CurHp, AttackPoint, AttackSpeed, MoveSpeed, Luck
}