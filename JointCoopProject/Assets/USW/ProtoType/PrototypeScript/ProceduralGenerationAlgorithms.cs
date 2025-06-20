using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Random Walk Algorithm 을 토대로한 알고리즘
/// 시작 위치에서 4방향중 무작위 방향으로 이동하면서 지나간 위치들을 HashSet에 저장함
/// 이 과정을 지정한 횟수(WalkLength) 만큼 반복한다면 랜덤한 경로가 생성이 됨.
/// HashSet 덕분에 중복 위치는 한번만 기록됨.
/// </summary>


public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        
        // 초기 위치 추가
        path.Add(startPosition);
        var previousposition = startPosition;

        // 새로운 위치에 Add(newPosition) 한다음 preivousposition 을 갱신
        // 최종적으로 지나간 tile 좌표들은 HashSet<Vector2Int> 로 변환함.
        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousposition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousposition = newPosition;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), // 위
        new Vector2Int(1, 0), //오른쪽
        new Vector2Int(0, -1), // 아래
        new Vector2Int(-1, 0), // 왼쪽
    };

    // 무작위 방향을 얻음
    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
