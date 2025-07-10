using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AGrid grid;
    Coroutine findPath;
    
    private void Awake() => Init();

    private void Init()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<AGrid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        findPath = StartCoroutine(FindPath(startPos, endPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        ANode startNode = grid.GetNodeFromWorldPoint(startPos);
        ANode targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if (startNode.isWalkable && targetNode.isWalkable)
        {
            List<ANode> openList = new List<ANode>();
            HashSet<ANode> closedList = new HashSet<ANode>();
            openList.Add(startNode);

            while(openList.Count > 0)
            {
                ANode currentNode = openList[0];
                // openList�� f�ڽ�Ʈ�� ���� ���� ��带 ����. f�ڽ�Ʈ�� ���ٸ� h�ڽ�Ʈ�� ���� ��� ����.
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost  < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }
                // Ž���� ���� openList���� �����ϰ� closedList�� ����
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // Ž���� ��尡 ��ǥ ����� Ž�� ����
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                // �ƴ϶�� ��� Ž��
                foreach (ANode n in grid.GetNeighbours(currentNode))
                {
                    // �̵��Ұ� ���ų� closedList�� �ִ� ���� ��ŵ
                    if (!n.isWalkable || closedList.Contains(n)) continue;

                    // �̿� ������ g�ڽ�Ʈ�� h�ڽ�Ʈ�� ����� openList�� �߰�
                    int newCurrentToNeighbourCost = currentNode.gCost + GetDistanceCost(currentNode, n);
                    if (newCurrentToNeighbourCost < n.gCost || !openList.Contains(n))
                    {
                        n.gCost = newCurrentToNeighbourCost;
                        n.hCost = GetDistanceCost(n, targetNode);
                        n.parentNode = currentNode;

                        if (!openList.Contains(n))
                            openList.Add(n);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        // ������ worldPosition�� ���� waypoints�� �������θ� request�Ŵ����� ����
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // Ž������ �� ��������� parentNode�� �����Ͽ� ����Ʈ�� ��� �޼���
    Vector3[] RetracePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode; //parentNode�� ���� ��� ����
        }
        Vector3[] waypoints = path.Select(n => n.worldPos).ToArray();
        Array.Reverse(waypoints);
        grid.pathWaypoints = waypoints;
        return waypoints;
    }

    // Path����Ʈ�� �ִ� ������ worldPosition�� Vector3[]�� ��� �����ϴ� �޼���
    Vector3[] SimplifyPath(List<ANode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    // �� ��� ���� �Ÿ��� �ڽ�Ʈ�� ����ϴ� �޼���
    int GetDistanceCost(ANode nodeA, ANode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
