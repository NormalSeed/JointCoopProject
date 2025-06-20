using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_MoveToward : MonoBehaviour
{
   public float movespeed = 5f;
   private Rigidbody2D rb2d;
   private Vector2 moveDirection;
   private Animator animator;
   private float horizontalInput;
   private float verticalInput;


   void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      if (rb2d == null)
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
      
      moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
      
      if (animator != null)
      {
         animator.SetFloat("Speed", moveDirection.magnitude * movespeed); 
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
      Vector2 targetPosition = rb2d.position + moveDirection * movespeed * Time.fixedDeltaTime;
      
      rb2d.MovePosition(Vector2.MoveTowards(rb2d.position, targetPosition, movespeed * Time.fixedDeltaTime));
   }
}

// isDodge = false 상시 : 
// shift 를 눌렀을때 = isDodge = true 
// isDodgeDuration =  2.0f 무적시간
// class Dodge 기능 ( 애니메이션을 추가한다면 반투명 상태  + 구르기 애니메이션 )(2.5f)  0.5초간은 마지막칸을 구르고있는 시점이니깐 적용이 가능해요 
// 