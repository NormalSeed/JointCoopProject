using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlameController : MonoBehaviour
{
    private ObjectPool _flamePool;
    [SerializeField] private PooledObject _flame;
    private float _flameSpd = 5f;

    private void Awake() => Init();

    private void Init()
    {
        _flamePool = new ObjectPool(transform, _flame, 4);
    }

    public void ShootFlame(Vector2 shootDir, int damage)
    {
        DragonFlame flame = GetFlame();
        flame._damage = damage;
        flame.transform.position = transform.position;
        // 발사 방향에 맞춰 화염의 roatation 조정
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        flame.transform.rotation = Quaternion.Euler(0, 0, angle);

        flame.Shoot(shootDir, _flameSpd, damage);
    }

    public DragonFlame GetFlame()
    {
        PooledObject po = _flamePool.PopPool();
        return po as DragonFlame;
    }
}
