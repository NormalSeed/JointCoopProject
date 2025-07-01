using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcRiderBulletController : MonoBehaviour
{
    private ObjectPool _bulletPool;
    [SerializeField] private PooledObject _bullet;
    private float _bulletSpd = 10f;

    private void Awake() => Init();

    private void Init()
    {
        _bulletPool = new ObjectPool(transform, _bullet, 6);
    }

    public void ShootBullet(Vector2 shootDir, int damage)
    {
        OrcRiderBullet bullet = GetBullet();
        bullet._damage = damage;
        bullet.transform.position = transform.position;
        // 발사 방향에 맞춰 검기의 roatation 조정
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        bullet.Shoot(shootDir, _bulletSpd, damage);
    }

    public OrcRiderBullet GetBullet()
    {
        PooledObject po = _bulletPool.PopPool();
        return po as OrcRiderBullet;
    }
}
