using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자가 능동/피동적으로 사용할 수 있고, 사용시 소모되는 아이템
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Item/Expendable Item")]
public class ExpendableItemSO : ItemDataSO
{
    // 값어치
    public int _worthAmonut;
    
}
