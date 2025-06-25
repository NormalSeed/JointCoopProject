using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, IDamagable
{
    private int _curHP = 30;

    public void TakeDamage(int damage, Vector2 targetPos)
    {
        _curHP -= damage;
        Debug.Log($"{gameObject.name}이 공격을 받아 HP가 줄어듦");
        Debug.Log($"현재 HP : {_curHP}");
    }
}
