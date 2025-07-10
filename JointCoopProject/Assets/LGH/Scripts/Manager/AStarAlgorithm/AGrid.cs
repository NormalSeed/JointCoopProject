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
        // �׸��带 ��� �������� ������ => ��� ���� Ȯ��
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

    // ��带 ã�� �޼���
    public ANode GetNodeAt(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && x < gridSizeY)
        {
            return grid[x, y];
        }
        else
            return null;
    }

    // ����� �ֺ� ��带 ã�� �޼���
    public List<ANode> GetNeighbours(ANode node)
    {
        List<ANode> neighbors = new List<ANode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // �����δ� ��ŵ

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // x, y�� ���� Grid ���� �ȿ� ���� ���
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // �밢�� ������ ��� ��������� ��ĭ ���� ��ĭ �� Ÿ�ϵ� ��� �����ؾ� ��
                    if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        ANode nodeX = GetNodeAt(node.gridX + x, node.gridY); // ���� ��
                        ANode nodeY = GetNodeAt(node.gridX, node.gridY + y); // ���� ��

                        if (nodeX == null || nodeY == null || !nodeX.isWalkable || !nodeY.isWalkable)
                        {
                            continue; // ���� �Ǵ� ���ΰ� ���� ������ �밢�� �̵� ����
                        }
                    }
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // ����Ƽ�� WorldPosition���κ��� �׸������ ��带 ã�� �޼���
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

    //Gizmo�� ���� Ȯ��
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

                    // Ž���� path�� ��� ǥ��
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
