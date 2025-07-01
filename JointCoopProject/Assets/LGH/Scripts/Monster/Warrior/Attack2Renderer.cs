using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Renderer : MonoBehaviour
{
    // TODO: LineRenderer가 아니라 MeshRenderer로 채워진 부채꼴 구현 시도
    private WarriorController _controller;
    private LineRenderer _lineRenderer;

    public float _radius;
    public float _angle;
    public int _segments = 60;
    public Vector2 _direction;

    private void Awake() => Init();

    private void Init()
    {
        _controller = GetComponent<WarriorController>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _segments + 2;
        _lineRenderer.enabled = false;
    }

    private void Start()
    {
        _angle = _controller._attack2Angle;
    }

    public void ShowAttack2()
    {
        _radius = _controller._model._attack2Range;
        _lineRenderer.enabled = true;
        Vector2 origin = transform.position;
        _direction = _controller._attack2Dir;
        Vector2 startDir = Quaternion.Euler(0, 0, -_angle / 2f) * _direction.normalized;

        _lineRenderer.SetPosition(0, origin);

        for (int i = 0; i <= _segments; i++)
        {
            float currentAngle = (-_angle / 2f) + (_angle * i / _segments);
            Vector2 dir = Quaternion.Euler(0, 0, currentAngle) * _direction.normalized;
            Vector2 point = origin + dir * _radius;
            _lineRenderer.SetPosition(i + 1, point);
        }
    }

    public void HideAttack2()
    {
        _lineRenderer.enabled = false;
    }
}
