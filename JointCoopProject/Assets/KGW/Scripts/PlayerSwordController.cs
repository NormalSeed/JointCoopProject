using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    Transform _playerPos;

    float _wieldDuration = 0.3f;
    float _timer = 0;
    float _rotationAngle = 300f;
    int _swordDamage = 1;

    //Vector3 _standardOffset = new Vector3(0.7f, 0, 0);

    private void Update()
    { 
        _timer -= Time.deltaTime;
        transform.RotateAround(_playerPos.position, Vector3.forward, _rotationAngle * Time.deltaTime);

        if (_timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.GetComponent<IDamagable>() != null)
        {
            // 대상이 공격을 받을 수 있으므로 데미지를 준다.
        }
    }

    public void Init(Transform player, Vector2 direction)
    {
        _playerPos = player;
        _timer = _wieldDuration;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 60f);

        transform.position = _playerPos.position + (Vector3)direction.normalized * _wieldDuration;
    }
}
