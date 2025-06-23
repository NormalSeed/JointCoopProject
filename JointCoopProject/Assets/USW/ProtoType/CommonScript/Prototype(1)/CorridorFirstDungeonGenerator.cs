using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

// 복도부터 만들고 나중에 방을 연결하거나 채우는 형태로 구현 .
public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] 
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    // 나중에 구현될 바에서 생성된 복도 위나 주변에 전체 복도중 몇% 정도 위치에 방을 붙일지 비율로 결정함 ( 0.5 면 전체 복도중 절반에 방이 붙는 구조 )
    private float roomPercentage = 0.5f;
   
    
    
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    // 그냥 좌표에 타일 칠함.
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        CreateCorridors(floorPositions,potentialRoomPositions);
        
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        
        floorPositions.UnionWith(roomPositions);
        
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions,tilemapVisualizer);
    }
    // 방 생성 (potentialroom 의 노드중 percentage 를 따져 그 기점을 통해 방을 만듬)
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercentage);
        
        List<Vector2Int> roomToCreate = potentialRoomPositions.OrderBy(x=>Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkSo, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    /// <summary>
    /// CorridorCount 만큼 복도 생성 , 하고
    /// 복도는 RandomWalkCorridor 를 호출해서 한방향으로 corridorLength 만큼 생성함.
    /// 생성된 모든 복도를 floorPositions 에 합침 
    /// </summary>
    /// <param name="floorPositions"></param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}
