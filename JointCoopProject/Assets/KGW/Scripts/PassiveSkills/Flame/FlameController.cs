using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    [SerializeField] float _throwTimer = 0.9f;
    float _FlameSpeed;
    int _FlameDamage;
    Vector3 _FlameDirection;

    public void Init(Vector3 dir, float speed, int damage)
    {
        _FlameSpeed = speed;
        _FlameDamage = damage;
        _FlameDirection = dir;
        Destroy(gameObject, _throwTimer);
    }

    private void Update()
    {
        transform.position += _FlameDirection * _FlameSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                // 공격 받은 대상이 FlameMultiDamage Component가 없으면 생성
                if(!collision.gameObject.GetComponent<FlameMultiDamage>())
                {
                    FlameMultiDamage flameMultiDamage = collision.AddComponent<FlameMultiDamage>();
                    // 화염 다단히트 적용
                    flameMultiDamage.Apply(damagable, _FlameDamage);
                }
                Destroy(gameObject);
            }
        }
    }
}
