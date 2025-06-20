using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Lerp_Velocity : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float accelerationRate = 5f; 
    private Rigidbody2D rb;
    private Vector2 desiredVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            enabled = false;
        }
    }

    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

       
        desiredVelocity = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        
        rb.velocity = Vector2.Lerp(rb.velocity, desiredVelocity, accelerationRate * Time.fixedDeltaTime);
    }
}