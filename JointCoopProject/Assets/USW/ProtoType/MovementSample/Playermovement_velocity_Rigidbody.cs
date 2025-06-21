using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_velocity_Rigidbody: MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            enabled = false;
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            enabled = false;
        }
    }

    private void Update()
    {
         horizontalInput = Input.GetAxis("Horizontal"); 
         verticalInput = Input.GetAxis("Vertical");
        if (animator != null)
        {
            animator.SetFloat("Speed", rb.velocity.magnitude); 
        }
        if (horizontalInput < 0) 
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (horizontalInput > 0) 
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        

        
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

       
        rb.velocity = moveDirection * moveSpeed;
    }
}
