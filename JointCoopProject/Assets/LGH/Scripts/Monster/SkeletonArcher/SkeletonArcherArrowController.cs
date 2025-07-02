using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherArrowController : MonoBehaviour
{
    private ObjectPool _arrowPool;
    [SerializeField] private PooledObject _arrow;
    private float _arrowSpd = 10f;

    private void Awake() => Init();

    private void Init()
    {
        _arrowPool = new ObjectPool(transform, _arrow, 2);
    }

    public void ShootArrow(Vector2 shootDir, int damage)
    {
        Arrow arrow = GetArrow();
        arrow._damage = damage;
        arrow.transform.position = transform.position;
        // 발사 방향에 맞춰 화살의 rotation 조정
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        arrow.Shoot(shootDir, _arrowSpd, damage);
    }

    public Arrow GetArrow()
    {
        PooledObject po = _arrowPool.PopPool();
        return po as Arrow; 
    }
}
