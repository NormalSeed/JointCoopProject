using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovingBackground : MonoBehaviour
{
   
   public bool autoReset = true;
   
   [SerializeField]
   public Vector2 moveSpeed = new Vector2(-2f,0f);
   public float resetPositionX = 10f;
   public float triggerPositionX = 10f;
   
   
   
   // fixedupdate 에서 velo주고 
   // 181 - > 150scale diff  25
   private void Update()
   {
      transform.Translate(moveSpeed * Time.deltaTime);

      if (autoReset && transform.position.x <= triggerPositionX)
      {
         Vector3 pos = transform.position;
         pos.x += resetPositionX;
         transform.position = pos;
      }
   }
}
