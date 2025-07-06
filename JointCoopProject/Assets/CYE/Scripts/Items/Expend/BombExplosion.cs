using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExploion : MonoBehaviour
{
    private Bomb _parent;
    void Awake()
    {
        Init();
    }
    void OnEnable()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(3, 3), 0f);
        foreach (Collider2D collider in colliders)
        {
            int realDamage = (collider.gameObject.layer == LayerMask.NameToLayer("Player")) ? 2 : _parent._explosiveDamage;
            collider.GetComponent<IDamagable>()?.TakeDamage(realDamage, transform.position);

            if (collider.gameObject.name.Contains("SecretWall"))
            {
                MapGenerator mapGen = FindObjectOfType<MapGenerator>();
                if (mapGen != null)
                {
                    mapGen.DamagedSecretWall(collider.gameObject);
                }
            }
        }
    }

    private void Init()
    {
        _parent = GetComponentInParent<Bomb>();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(3, 3, 3));
    }
}
