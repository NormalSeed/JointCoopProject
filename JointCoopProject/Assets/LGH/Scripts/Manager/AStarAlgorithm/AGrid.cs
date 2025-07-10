using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    ANode[,] grid;
    public Vector3[] pathWaypoints;
    public List<ANode> path;
    [SerializeField] private bool isDrawGizmo;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        // 그리드를 노드 지름으로 나눠줌 => 노드 갯수 확정
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new ANode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        Vector3 worldPoint;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius * 0.95f, unwalkableMask));
                grid[x, y] = new ANode(walkable, worldPoint, x, y);
            }
        }
    }

    // 노드를 찾는 메서드
    public ANode GetNodeAt(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && x < gridSizeY)
        {
            return grid[x, y];
        }
        else
            return null;
    }

    // 노드의 주변 노드를 찾는 메서드
    public List<ANode> GetNeighbours(ANode node)
    {
        List<ANode> neighbors = new List<ANode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // 스스로는 스킵

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // x, y의 값이 Grid 범위 안에 있을 경우
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // 대각선 방향일 경우 진행방향의 한칸 위와 한칸 옆 타일도 통과 가능해야 함
                    if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        ANode nodeX = GetNodeAt(node.gridX + x, node.gridY); // 수평 셀
                        ANode nodeY = GetNodeAt(node.gridX, node.gridY + y); // 수직 셀

                        if (nodeX == null || nodeY == null || !nodeX.isWalkable || !nodeY.isWalkable)
                        {
                            continue; // 가로 또는 세로가 막혀 있으면 대각선 이동 불허
                        }
                    }
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // 유니티의 WorldPosition으로부터 그리드상의 노드를 찾는 메서드
    public ANode GetNodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    //Gizmo를 통해 확인
    private void OnDrawGizmos()
    {
        if (isDrawGizmo)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0f));
            if (grid != null)
            {
                foreach (ANode n in grid)
                {
                    Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPos, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0f));

                    // 탐색된 path의 노드 표시
                    if (pathWaypoints != null && pathWaypoints.Length > 1)
                    {
                        Gizmos.color = Color.green;
                        for (int i = 0; i < pathWaypoints.Length - 1; i++)
                        {
                            Gizmos.DrawLine(pathWaypoints[i], pathWaypoints[i + 1]);
                        }
                    }
                    
                }
            }
        }
    }
}
