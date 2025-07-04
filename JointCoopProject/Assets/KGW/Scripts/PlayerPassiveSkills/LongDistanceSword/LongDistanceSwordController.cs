using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceSwordController : MonoBehaviour
{
    [SerializeField] float _throwTimer = 0.9f;
    float _swordEnergySpeed;
    int _swordEnergyDamage;
    Vector3 _swordEnergyDirection;

    public void Init(Vector3 dir, float speed, int damage)
    {
        _swordEnergySpeed = speed;
        _swordEnergyDamage = damage;
        _swordEnergyDirection = dir;
        Destroy(gameObject, _throwTimer);
    }

    private void Update()
    {
        transform.position += _swordEnergyDirection * _swordEnergySpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log($"[검기 대미지] {_swordEnergyDamage} 피해 적용됨.");
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_swordEnergyDamage, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
