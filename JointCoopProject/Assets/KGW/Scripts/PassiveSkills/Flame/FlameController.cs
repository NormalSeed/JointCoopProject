using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    [SerializeField] float _throwTimer = 0.9f;
    float _FlameSpeed;
    int _FlameDamage;
    Vector3 _FlameDirection;
    int _FlameDamageCount = 3;
    Coroutine _flameCoroutine;

    public void Init(Vector3 dir, float speed, int damage)
    {
        _FlameSpeed = speed;
        _FlameDamage = damage;
        _FlameDirection = dir;
        //Destroy(gameObject, _throwTimer);
    }

    private void Update()
    {
        transform.position += _FlameDirection * _FlameSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _FlameDirection = Vector3.zero;
            Debug.Log($"적을 {_FlameDamage}로 공격했습니다.");
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                if(_flameCoroutine != null)
                {
                    _flameCoroutine = null;
                }
                else
                {
                    _flameCoroutine = StartCoroutine(FlameDamageCoroutine(damagable));
                }
                StopCoroutine(_flameCoroutine);
            }
            //Destroy(gameObject);
        }
    }
    private IEnumerator FlameDamageCoroutine(IDamagable damagable)
    {
        damagable.TakeDamage(_FlameDamage, transform.position);
        yield return new WaitForSeconds(1.5f);
        damagable.TakeDamage(_FlameDamage, transform.position);
        yield return new WaitForSeconds(1.5f);
        damagable.TakeDamage(_FlameDamage, transform.position);
        _flameCoroutine = null;
    }
}
