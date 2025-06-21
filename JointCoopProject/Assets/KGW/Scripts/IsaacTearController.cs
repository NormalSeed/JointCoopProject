using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacTearController : MonoBehaviour
{
    [SerializeField] float _tearLifeTime = 3f;
    [SerializeField][Range(1, 3)] int _tearDamage = 1;

    private void Start()
    {
        Destroy(gameObject, _tearLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
