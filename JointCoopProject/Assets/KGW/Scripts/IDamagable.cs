using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    // Take Damage
    public void TakeDamage(int damage, Vector2 targetPos);

}

