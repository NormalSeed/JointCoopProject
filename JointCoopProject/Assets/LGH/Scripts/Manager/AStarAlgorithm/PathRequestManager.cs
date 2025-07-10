using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PathRequestManager : MonoBehaviour
{
    // ������Ʈ�� ��θ� ��û�ϰ� ��λ��� ������ ��ġ�� �޾� ������Ʈ���� �̵� ������ �˸��� �ݹ��Լ��� �����ִ� �Ŵ���
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    private void Awake() => Init();

    private void Init()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    // ������Ʈ���� ��û�ϴ� �Լ�
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, UnityAction<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    // ť ������� ��ã�� ��û�� ������ Pathfinding �˰����� �����ϴ� �޼���
    public void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // ��ã�Ⱑ �Ϸ�� ��û�� ó���ϰ� ������Ʈ���� �̵����� ��� �ݹ��Լ��� �����ϴ� �޼���
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
}

struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public UnityAction<Vector3[], bool> callback;

    public PathRequest(Vector3 nStart, Vector3 nEnd, UnityAction<Vector3[], bool> nCallback)
    {
        pathStart = nStart;
        pathEnd = nEnd;
        callback = nCallback;
    }
}
