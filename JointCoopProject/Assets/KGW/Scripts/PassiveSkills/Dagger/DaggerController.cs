using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerController : MonoBehaviour
{
    [SerializeField] float _throwTimer = 0.9f;
    float _daggerSpeed;
    int _daggerDamage;
    Vector3 _daggerDirection;

    public void Init(Vector3 dir, float speed, int damage)
    {
        _daggerSpeed = speed;
        _daggerDamage = damage;
        _daggerDirection = dir;
        Destroy(gameObject, _throwTimer);
    }

    private void Update()
    {
        transform.position += _daggerDirection * _daggerSpeed * Time.deltaTime; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log($"[단검 대미지] {_daggerDamage} 피해 적용됨.");
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_daggerDamage, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
