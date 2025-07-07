using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterMovement : MonoBehaviour
{
    // �����¿�� �ڵ����� �����̴� ������ AI ���� �ʿ�
    // �������� �ʴ� ���͵� ���� �� �����Ƿ� canMove bool������ �¿��� ����
    public bool _isPatrol;
    public bool _isTrace;
    private SpriteRenderer _sprRend;
    public Rigidbody2D _rb;
    private Vector2 _patrolDir;
    private float _moveTimer;
    private float _changeInterval = 1.5f;

    public float checkDist = 1f;
    public int rayCount = 16;           // ���� �� 8���� �˻� �� �� 16����
    public float maxAvoidAngle = 90f;   // ������ �� ��90�� ������ ȸ��
    private LayerMask obstacleMask;

    private Vector2 _prevMoveDir = Vector2.zero;
    private Vector2 _velSmooth = Vector2.zero;

    [Header("Steering Smoothing")]
    [SerializeField] private float smoothTime = 0.12f;     // Ŭ���� �� �ε巴��
    [SerializeField] private float avoidWeight = 1f;       // rawDir�� toTarget�� �󸶳� ����ġ�� �ݿ��Ǵ���


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

        // 1) �ﰢ ȸ�� �������� rawDir ���
        Vector2 rawDir = FindFreeDirection(currentPos, toTarget);

        // 2) �ε巯�� ����: ���� ���� �� rawDir
        //    _velSmooth�� ���������� ���Ǵ� ref �Ķ����
        Vector2 moveDir = Vector2.SmoothDamp(_prevMoveDir, rawDir, ref _velSmooth, smoothTime);

        _prevMoveDir = moveDir;  // ���� ������ ������ ���

        // 3) ���� �̵�
        Vector2 newPos = currentPos + moveDir * moveSpd * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);

        // 4) ��������Ʈ ���� ���� (x������)
        if (moveDir.x < 0) _sprRend.flipX = true;
        else if (moveDir.x > 0) _sprRend.flipX = false;
    }

    // ��ǥ ������ �������� �˻��ϰ�, �ո� ���� ����
    private Vector2 FindFreeDirection(Vector2 origin, Vector2 toTarget)
    {
        // ������ �շ����� toTarget �״��
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

        // ���� ��쿣 ������ �ε巴��
        return -toTarget;
    }

    // 2D ���� ���� ȸ�� ����
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
