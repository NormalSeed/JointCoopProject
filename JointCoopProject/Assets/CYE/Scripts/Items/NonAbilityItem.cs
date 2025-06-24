using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonAbilityItem : MonoBehaviour, IPickable
{
    private string _itemName;
    private string _itemDesc;

    private string _varianceTarget; // string은 임시
    private float _varianceValue;

    // 아이템 주움
    public void PickedUp() { }

    // 아이템 떨굼
    public void Drop(Transform itemPos) { }
    
}
