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

    public float checkDist = 1f;
    public int rayCount = 16;           // 한쪽 당 8방향 검사 → 총 16방향
    public float maxAvoidAngle = 90f;   // 막혔을 때 ±90° 까지만 회피
    private LayerMask obstacleMask;

    private Vector2 _prevMoveDir = Vector2.zero;
    private Vector2 _velSmooth = Vector2.zero;

    [Header("Steering Smoothing")]
    [SerializeField] private float smoothTime = 0.12f;     // 클수록 더 부드럽게
    [SerializeField] private float avoidWeight = 1f;       // rawDir이 toTarget에 얼마나 가중치로 반영되는지


    private void Awake() => Init();

    private void Init()
    {
        _sprRend = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _isPatrol = false;
        _moveTimer = 0f;
        obstacleMask = LayerMask.GetMask("Obstacle");
    }

    private void Update()
    {

    }

    public void Trace(float moveSpd)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Vector2 currentPos = _rb.position;
        Vector2 toTarget = (player.transform.position - transform.position).normalized;

        // 1) 즉각 회피 로직으로 rawDir 계산
        Vector2 rawDir = FindFreeDirection(currentPos, toTarget);

        // 2) 부드러운 보간: 이전 방향 ↔ rawDir
        //    _velSmooth는 내부적으로 사용되는 ref 파라미터
        Vector2 moveDir = Vector2.SmoothDamp(_prevMoveDir, rawDir, ref _velSmooth, smoothTime);

        _prevMoveDir = moveDir;  // 다음 프레임 보간에 사용

        // 3) 실제 이동
        Vector2 newPos = currentPos + moveDir * moveSpd * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);

        // 4) 스프라이트 방향 갱신 (x값으로)
        if (moveDir.x < 0) _sprRend.flipX = true;
        else if (moveDir.x > 0) _sprRend.flipX = false;
    }

    // 목표 방향이 막혔는지 검사하고, 뚫린 방향 리턴
    private Vector2 FindFreeDirection(Vector2 origin, Vector2 toTarget)
    {
        // 정면이 뚫렸으면 toTarget 그대로
        if (!Physics2D.CircleCast(origin, 0.2f, toTarget, checkDist, obstacleMask))
            return toTarget;

        float step = maxAvoidAngle / (rayCount / 2);
        for (int i = 1; i <= rayCount / 2; i++)
        {
            Vector2 dirL = Rotate(toTarget, step * i);
            if (!Physics2D.CircleCast(origin, 0.2f, dirL, checkDist, obstacleMask))
                return Vector2.Lerp(toTarget, dirL, avoidWeight).normalized;

            Vector2 dirR = Rotate(toTarget, -step * i);
            if (!Physics2D.CircleCast(origin, 0.2f, dirR, checkDist, obstacleMask))
                return Vector2.Lerp(toTarget, dirR, avoidWeight).normalized;
        }

        // 막힌 경우엔 후진도 부드럽게
        return -toTarget;
    }

    // 2D 벡터 각도 회전 헬퍼
    private Vector2 Rotate(Vector2 v, float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        ).normalized;
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
