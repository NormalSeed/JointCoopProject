using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterMovement : MonoBehaviour
{
    // 몬스터 이동을 담당하는 추상 클래스
    // 이동 가능 여부를 제어하는 bool값들
    public bool _isPatrol;
    public bool _isTrace;
    private SpriteRenderer _sprRend;
    public Rigidbody2D _rb;
    private Vector2 _patrolDir;
    private float _moveTimer;
    private float _changeInterval = 1.5f;

    public float checkDist = 1f;
    public int rayCount = 16;           // 장애물 탐지를 위한 CircleCast 회전 횟수(16방향)
    public float maxAvoidAngle = 90f;   // 최대 회피 각도(왼쪽 90도 ~ 오른쪽 90도)
    private Vector2 _steeringDir = Vector2.zero;
    private float _steeringCooldown = 0f;
    [SerializeField] private float _steeringUpdateDelay = 1f; // 방향 재탐색 간격
    private LayerMask obstacleMask;

    Vector3[] path;
    int pathIndex;

    private Vector2 _prevMoveDir = Vector2.zero;
    private Vector2 _velSmooth = Vector2.zero;

    [Header("Steering Smoothing")]
    [SerializeField] private float smoothTime = 0.12f;
    [SerializeField] private float avoidWeight = 1f;

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
        //GameObject player = GameObject.FindWithTag("Player");
        //if (player == null) return;

        //Vector2 currentPos = _rb.position;
        //Vector2 toTarget = (player.transform.position - transform.position).normalized;

        //// 일정 시간마다만 회피 방향 재탐색
        //if (_steeringCooldown <= 0f)
        //{
        //    _steeringDir = FindFreeDirection(currentPos, toTarget);
        //    _steeringCooldown = _steeringUpdateDelay;
        //}
        //else
        //{
        //    _steeringCooldown -= Time.deltaTime;
        //}

        //// 부드러운 이동 처리
        //Vector2 moveDir = Vector2.SmoothDamp(_prevMoveDir, _steeringDir, ref _velSmooth, smoothTime);
        //_prevMoveDir = moveDir;

        //// 실제 위치 이동
        //Vector2 newPos = currentPos + moveDir * moveSpd * Time.fixedDeltaTime;
        //_rb.MovePosition(newPos);

        //// 좌우 반전
        //if (moveDir.x < 0) _sprRend.flipX = true;
        //else if (moveDir.x > 0) _sprRend.flipX = false;
    }

    public void RequestTracePath()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            pathIndex = 0;
        }
    }

    public void FollowTracePath(float moveSpd)
    {
        if (path == null || pathIndex >= path.Length) return;

        Vector2 targetPos = path[pathIndex];
        Vector2 currentPos = _rb.position;

        if (Vector2.Distance(currentPos, targetPos) < 0.1f)
        {
            pathIndex++;
            if (pathIndex >= path.Length) return;
            targetPos = path[pathIndex];
        }

        Vector2 moveDir = (targetPos - currentPos).normalized;
        Vector2 newPos = currentPos + moveDir * moveSpd * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);

        _sprRend.flipX = moveDir.x < 0;
    }


    private Vector2 FindFreeDirection(Vector2 origin, Vector2 toTarget)
    {
        if (!Physics2D.CircleCast(origin, 0.2f, toTarget, checkDist, obstacleMask))
            return toTarget;

        float step = maxAvoidAngle / (rayCount / 2);
        for (int i = 1; i <= rayCount / 2; i++)
        {
            Vector2 dirL = Rotate(toTarget, step * i);
            if (!Physics2D.CircleCast(origin, 0.5f, dirL, checkDist, obstacleMask))
                return Vector2.Lerp(toTarget, dirL, avoidWeight).normalized;

            Vector2 dirR = Rotate(toTarget, -step * i);
            if (!Physics2D.CircleCast(origin, 0.5f, dirR, checkDist, obstacleMask))
                return Vector2.Lerp(toTarget, dirR, avoidWeight).normalized;
        }

        // 이전 방향 유지
        return _prevMoveDir == Vector2.zero ? toTarget : _prevMoveDir;
    }


    // 2D 회전 방향 반환 함수
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
        // timer �ð� ���� �̵�
        _rb.MovePosition(_rb.position + _patrolDir * moveSpd * Time.fixedDeltaTime);
        UpdateSpriteDir();
        _moveTimer += Time.fixedDeltaTime;
        if (_moveTimer >= _changeInterval)
        {
            ChangeDir();
            _moveTimer = 0f;
        }
    }

    // �� �Ǵ� ��ֹ��� �ε����� ChangeDir�� �ϵ��� ��(�±״� ���� �� ����)
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
