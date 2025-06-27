using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Player Status")]
    // Player HP
    [SerializeField] int playerHp = 3;
    public int _playerHp { get { return playerHp; } set { playerHp = value; } }

    // Player Attack Damage
    [SerializeField] int attackDamage = 1;
    public int _attackDamage { get { return attackDamage; } set { attackDamage = value; } }

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

    // Player Attack Speed
    [SerializeField] float attackSpeed;
    public float _attackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

    // Player Dash Ability Check
    [SerializeField] bool canDash = false;
    public bool _canDash { get { return canDash; } set { canDash = value; } }

    // Player Luck
    [SerializeField] float playerLuck;
    public float _playerLuck { get { return playerLuck; } set { playerLuck = value; } }

    // ------------------------------------------------------------------------------------------------------
    [Header("Reference List")]
    [SerializeField] PlayerMovement _playerMove;
    [SerializeField] Animator _animator;
    [SerializeField] public float _hitCoolTime = 0.4f;
    [SerializeField] float _knockBackForce = 1.5f;
    [SerializeField] float _knockBackTime = 0.2f;

    public bool _isAlive;
    public bool _isKnockBack = false;

    // Player Hp Down
    public void HealthDown(int damage)
    {
        if ( _playerHp > 1)
        {
            _playerHp -= damage;
            Debug.Log($"플레이어의 체력이 {_playerHp} 입니다.");
            // TODO : 플레이어 피격 사운드
        }
        else
        {
            Debug.Log("플레이어가 사망했습니다.");
            // Player Death
            _isAlive = false;
        }
    }

    // Player Death
    public void PlayerDeath()
    {
        _animator.SetBool("IsDeath", true);
        GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(gameObject, 9f);
        // TODO : UI 추가
    }
    
    // Player KnockBack
    public void PlayerKnockBack(Vector2 targetPos)
    {
        _isKnockBack = true;
        Vector2 hitDirection = ((Vector2)transform.position - targetPos).normalized;
        _playerMove._playerRigid.velocity = Vector2.zero;
        _playerMove._playerRigid.AddForce(hitDirection * _knockBackForce , ForceMode2D.Impulse);

        Invoke("StopKnockBack", _knockBackTime);
    }

    private void StopKnockBack()
    {
        _playerMove._playerRigid.velocity = Vector2.zero;
        _playerMove._curVelocity = Vector2.zero;
        _isKnockBack = false;
    }
    
}
