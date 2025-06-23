using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed = 5f;
    private float lifeTime = 2f;
    private float timer = 0f;
    private bool init = false;
    public ParticleSystem particleSystem;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        init = true;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetLifeTime(float t)
    {
        lifeTime = t;
    }
    void Update()
    {
        if (!init) return;
        
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }
        
        
        // 목표위치까지 이동 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }


  
}
