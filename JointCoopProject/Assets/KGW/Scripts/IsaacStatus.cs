using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacStatus : MonoBehaviour
{
    // Isaac Move Speed
    [SerializeField][Range(0, 10)] float moveSpeed;
    public float _moveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // Isaac Acceleration Speed
    [SerializeField][Range(0, 30)] float accelerationSpeed;
    public float _accelerationSpeed { get { return accelerationSpeed; } set { accelerationSpeed = value; } }

    // Isaac Deceleration Speed
    [SerializeField][Range(0, 30)] float decelerationSpeed;
    public float _decelerationSpeed { get {return decelerationSpeed; } set { decelerationSpeed = value; } }

    // Isaac Tear Shot Speed
    [SerializeField] float shotSpeed;
    public float _shotSpeed { get { return shotSpeed; } set { shotSpeed = value; } }

}
