using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrayCatGrenade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(3, transform.position);
                Debug.Log("적에게 맞춤");
            }
        }
    }

    public void UnactivateSelf()
    {
        Destroy(gameObject);
    }
}
