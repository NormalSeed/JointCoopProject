using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacStatus : MonoBehaviour
{
    // Isaac Move Speed
    [SerializeField][Range(0, 10)] float moveSpeed;
    public float _moveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // Isaac Acceleration Speed;
    [SerializeField][Range(0, 30)] float accelerationSpeed;
    public float _accelerationSpeed { get { return accelerationSpeed; } set { accelerationSpeed = value; } }

    // Isaac Deceleration Speed;
    [SerializeField][Range(0, 30)] float DecelerationSpeed;
    public float _DecelerationSpeed { get {return DecelerationSpeed; } set { DecelerationSpeed = value; } }
}
