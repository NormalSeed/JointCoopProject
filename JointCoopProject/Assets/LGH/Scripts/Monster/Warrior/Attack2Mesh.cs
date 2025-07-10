using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Attack2Mesh : MonoBehaviour
{
    public float _radius;
    public float _angle;
    public int _segments = 600;
    public Color _fillColor = new Color(1f, 0f, 0f, 0.5f);

    private WarriorController _controller;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Awake() => Init();

    private void Init()
    {
        _controller = GetComponentInParent<WarriorController>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.sortingOrder = 2;

        gameObject.SetActive(false);
    }

    public void CreateSector(Vector2 direction)
    {
        _radius = _controller._model._attack2Range;
        _angle = _controller._attack2Angle;

        var mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = _fillColor;
        _meshRenderer.material = mat;
        _meshRenderer.sortingLayerName = "Player";
        _meshRenderer.sortingOrder = 2;

        // 방향 맞추기 : 플레이어를 바라보도록
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[_segments + 2];
        int[] triangles = new int[_segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= _segments; i++)
        {
            // 플레이어를 바라보는 방향 기준 반시계방향으로 순서대로 _angle/ _segments만큼의 각도로,
            // vertices[0](부채꼴의 꼭지점) 기준으로 _radius만큼 떨어진 점 verticise[i + 1]을 찍어줌
            float currentAngle = (-_angle / 2f + _angle * i / _segments) * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle), 0f) * _radius;
            vertices[i + 1] = point;
        }

        for (int i = 0; i < _segments; i++)
        {
            // triangles에 verticies를 (0, 1, 2), (0, 2, 3)... 과 같은 식으로 꼭지점을 배정시켜
            // 부채꼴을 이룰 삼각형들을 구성
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // mesh의 vertices와 triangles에 값을 넣어주고 법선벡터 재설정 후 WarriorAttack2라는 이름의 메쉬 생성
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.name = "WarriorAttack2";

        //_meshFilter에 메쉬 할당
        _meshFilter.mesh = mesh;
    }
}

