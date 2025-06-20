using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_velocity_Rigidbody: MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

       
        rb.velocity = moveDirection * moveSpeed;
    }
}
