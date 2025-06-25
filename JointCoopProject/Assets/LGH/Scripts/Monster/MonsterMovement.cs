using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterMovement : MonoBehaviour
{
    // 상하좌우로 자동으로 움직이는 간단한 AI 구현 필요
    // 움직이지 않는 몬스터도 있을 수 있으므로 canMove bool값으로 온오프 가능
    public bool _isPatrol;
    public bool _isTrace;
    private SpriteRenderer _sprRend;
    private Rigidbody2D _rb;
    private Vector2 _patrolDir;
    private float _moveTimer;
    private float _changeInterval = 1.5f;

    private void Awake() => Init();

    private void Init()
    {
        _sprRend = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _isPatrol = false;
        _moveTimer = 0f;
    }

    public void Trace(float moveSpd)
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) return;

        Vector2 currentPos = _rb.position;
        Vector2 targetPos = player.transform.position;
        _patrolDir = targetPos - currentPos;
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, moveSpd * Time.fixedDeltaTime);

        UpdateSpriteDir();

        _rb.MovePosition(newPos);
    }

    public void Patrol(float moveSpd)
    {
        // timer 시간 동안 이동
        _rb.MovePosition(_rb.position + _patrolDir * moveSpd * Time.fixedDeltaTime);
        UpdateSpriteDir();
        _moveTimer += Time.fixedDeltaTime;
        if (_moveTimer >= _changeInterval)
        {
            ChangeDir();
            _moveTimer = 0f;
        }
    }

    // 벽 또는 장애물과 부딪히면 ChangeDir를 하도록 함(태그는 협의 후 결정)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ChangeDir();
        }
    }

    protected void ChangeDir()
    {
        int dir = Random.Range(0, 5);

        switch (dir)
        {
            case 0: _patrolDir = Vector2.up; break;
            case 1: _patrolDir = Vector2.down; break;
            case 2: _patrolDir = Vector2.left; break;
            case 3: _patrolDir = Vector2.right; break;
            case 4: _patrolDir = Vector2.zero; break;
        }
    }

    protected void UpdateSpriteDir()
    {
        if (_patrolDir.x < 0)
            _sprRend.flipX = true;
        else if( _patrolDir.x > 0)
            _sprRend.flipX = false;
    }
}
