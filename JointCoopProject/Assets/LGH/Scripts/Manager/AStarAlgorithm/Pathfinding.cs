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
                // openList에 f코스트가 가장 작은 노드를 저장. f코스트가 같다면 h코스트가 작은 노드 저장.
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost  < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }
                // 탐색된 노드는 openList에서 제거하고 closedList에 저장
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // 탐색된 노드가 목표 노드라면 탐색 종료
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                // 아니라면 계속 탐색
                foreach (ANode n in grid.GetNeighbours(currentNode))
                {
                    // 이동불가 노드거나 closedList에 있는 노드는 스킵
                    if (!n.isWalkable || closedList.Contains(n)) continue;

                    // 이웃 노드들의 g코스트와 h코스트를 계산해 openList에 추가
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
        // 노드들의 worldPosition을 담은 waypoints와 성공여부를 request매니저에 전달
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // 탐색종료 후 최종노드의 parentNode를 추적하여 리스트에 담는 메서드
    Vector3[] RetracePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode; //parentNode에 원래 노드 저장
        }
        Vector3[] waypoints = path.Select(n => n.worldPos).ToArray();
        Array.Reverse(waypoints);
        grid.pathWaypoints = waypoints;
        return waypoints;
    }

    // Path리스트에 있는 노드들의 worldPosition을 Vector3[]에 담아 전달하는 메서드
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

    // 두 노드 간의 거리로 코스트를 계산하는 메서드
    int GetDistanceCost(ANode nodeA, ANode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
