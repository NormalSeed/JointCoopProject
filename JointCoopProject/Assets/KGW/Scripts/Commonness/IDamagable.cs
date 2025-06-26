using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    // 데미지를 받는 메서드
    public void TakeDamage(int damage, Vector2 targetPos);

}

