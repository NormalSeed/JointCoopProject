using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    // 아이템 주움
    public void PickedUp();
    
    // 아이템 떨굼
    public void Drop(Transform itemPos);
}
public enum SkillItemType
{
    Active, Passive
}
public enum StatChangeMethod
{ 
    sum, mlt
}