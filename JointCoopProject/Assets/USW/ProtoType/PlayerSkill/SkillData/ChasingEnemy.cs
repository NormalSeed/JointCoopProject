using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    private GameObject target;
    private float speed;
    // private float stunDuration;
    private bool isInit = false;

    // 초기화 함수
    public void Init(GameObject targetEnemy, float projectileSpeed )
    {
        target = targetEnemy;
        speed = projectileSpeed;
        isInit = true;
    }

    private void Update()
    {
        if (!isInit || target == null)
        {
            transform.Translate (Vector3.forward*speed*Time.deltaTime);
            return;
        }

        Vector3 targetDirection = (target.transform.position - transform.position).normalized;

        if(targetDirection != Vector3.zero )
        {
            transform.rotation = Quaternion.LookRotation (targetDirection);
        }


        transform.Translate(Vector3.forward * speed*Time.deltaTime);

      
    }


    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy (gameObject);
        }
    }


}
