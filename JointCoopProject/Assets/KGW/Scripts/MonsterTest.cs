using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        Debug.Log($"몬스터가 {damage}를 받았습니다.");
    }
}
