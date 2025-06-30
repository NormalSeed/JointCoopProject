using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    // 아이템 주움
    public void PickUp(Transform PlayerPos);

    // 아이템 떨굼
    public void Drop(Transform dropPos);
}
public enum SkillItemType
{
    Active, PassiveAttack, PassiveAuto
}
public enum NonSkillItemType
{ 
    Enhance, Expend
}
public enum StatChangeMethod
{ 
    plus, mlt, minus, div
}