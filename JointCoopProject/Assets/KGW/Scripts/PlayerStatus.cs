using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Player Status")]
    // Player Move Speed
    [SerializeField][Range(0, 10)] float moveSpeed;
    public float _moveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // Player Acceleration Speed
    [SerializeField][Range(0, 30)] float accelerationSpeed;
    public float _accelerationSpeed { get { return accelerationSpeed; } set { accelerationSpeed = value; } }

    // Player Deceleration Speed
    [SerializeField][Range(0, 30)] float decelerationSpeed;
    public float _decelerationSpeed { get {return decelerationSpeed; } set { decelerationSpeed = value; } }

    // Player Dash Speed
    [SerializeField][Range(5, 20)] int dashSpeed;
    public int _dashSpeed { get { return dashSpeed; } set { dashSpeed = value; } }

    // Player Dash Duration Time
    [SerializeField][Range(0, 1)] float dashDurationTime;
    public float _dashDurationTime { get { return dashDurationTime; } set { dashDurationTime = value; } }

    // Player Dash CoolTime
    [SerializeField][Range(0, 2)] float dashCoolTime;
    public float _dashCoolTime { get { return dashCoolTime; } set { dashCoolTime = value; } }

    // Player Shot Speed
    [SerializeField] float shotSpeed;
    public float _shotSpeed { get { return shotSpeed; } set { shotSpeed = value; } }

    [SerializeField] float attackSpeed;
    public float _attackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
}
