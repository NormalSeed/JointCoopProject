using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeEnemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    
    [SerializeField]
    public float fireRate = 0.5f;

    private float burstDelay = 1.0f;

    public float initDelyBetweenBurst = 2.0f;
    
    public int bulletBurstCount = 3;

    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        
        if (playerObj != null)
        {
            player = playerObj.transform;

            StartCoroutine(ShootPlayerRoutine());
        }
    }

    IEnumerator ShootPlayerRoutine()
    {
        yield return new WaitForSeconds(initDelyBetweenBurst);

        while (true)
        {
            if (player != null)
            {
                for (int i = 0; i < bulletBurstCount; i++)
                {
                    ShootBullet();
                    
                    yield return new WaitForSeconds(fireRate);
                }
            }

            yield return new WaitForSeconds(initDelyBetweenBurst);
        }
    }

    void ShootBullet()
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Settarget(player.position);
            }
        }
    }
}
