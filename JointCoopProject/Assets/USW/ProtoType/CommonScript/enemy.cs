using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
   public Transform target;

   private void Update()
   {
      transform.LookAt2D(target.position);
   }
}
