using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterMovement : MonoBehaviour
{
    // 상하좌우로 자동으로 움직이는 간단한 AI 구현 필요
    // 움직이지 않는 몬스터도 있을 수 있으므로 canMove bool값으로 온오프 가능
    public bool _canMove;
    private Rigidbody2D _rb;
    private Vector2 _patrolDir;
    private float _moveTimer;
    protected float _moveSpd = 2f;
    private float _changeInterval = 1.5f;
    private Coroutine _coPatrolDelay;
    private WaitForSeconds _patrolDelay = new WaitForSeconds(1f);

    private void Awake() => Init();

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canMove = false;
        _moveTimer = 0f;
    }

    public void Patrol()
    {
        // timer 시간 동안 이동
        _rb.MovePosition(_rb.position + _patrolDir * _moveSpd * Time.fixedDeltaTime);
        _moveTimer += Time.fixedDeltaTime;
        if (_moveTimer >= _changeInterval)
        {
            ChangeDir();
            _moveTimer = 0f;
            _coPatrolDelay = StartCoroutine(CoPatrolDelay());
        }
    }

    private IEnumerator CoPatrolDelay()
    {
        yield return _patrolDelay;
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
}
