using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardSlayerFireController : MonoBehaviour
{
    private ObjectPool _firePool;
    [SerializeField] private PooledObject _fire;
    private float _fireSpd = 10f;

    private void Awake() => Init();

    private void Init()
    {
        _firePool = new ObjectPool(transform, _fire, 4);
    }

    public void ShootFire(Vector2 shootDir, int damage)
    {
        ShardSlayerFire fire = GetFire();
        fire._damage = damage;
        fire.transform.position = transform.position;
        // �߻� ���⿡ ���� ȭ���� roatation ����
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        fire.transform.rotation = Quaternion.Euler(0, 0, angle);

        fire.Shoot(shootDir, _fireSpd, damage);
    }

    public ShardSlayerFire GetFire()
    {
        PooledObject po = _firePool.PopPool();
        return po as ShardSlayerFire;
    }
}
