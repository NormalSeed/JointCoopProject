using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    // 아이템 주움
    void PickedUp();
    
    // 아이템 떨굼
    void Drop(Transform itemPos);
}
public enum ItemType
{
    active, passive, trait, enhance, expendable
}
public enum StatChangeMethod
{ 
    sum, multiplication
}