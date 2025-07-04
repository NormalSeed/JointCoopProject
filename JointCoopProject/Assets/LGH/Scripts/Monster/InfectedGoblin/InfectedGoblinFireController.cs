using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedGoblinFireController : MonoBehaviour
{
    private ObjectPool _firePool;
    [SerializeField] private PooledObject _fire;
    private float _fireSpd = 10f;

    private void Awake() => Init();

    private void Init()
    {
        _firePool = new ObjectPool(transform, _fire, 6);
    }

    public void ShootFire(Vector2 shootDir, int damage)
    {
        InfectedGoblinFire fire = GetFire();
        fire._damage = damage;
        fire.transform.position = transform.position;
        // 발사 방향에 맞춰 화염의 roatation 조정
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        fire.transform.rotation = Quaternion.Euler(0, 0, angle);

        fire.Shoot(shootDir, _fireSpd, damage);
    }

    public InfectedGoblinFire GetFire()
    {
        PooledObject po = _firePool.PopPool();
        return po as InfectedGoblinFire;
    }
}
