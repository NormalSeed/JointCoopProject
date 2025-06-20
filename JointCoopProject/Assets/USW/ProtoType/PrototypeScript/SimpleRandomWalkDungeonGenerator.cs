using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
  

  
    // 몇번의 랜덤워크를 반복할지를 결정
    //private int iterations 
    //
    // 한번의 랜덤 워크에서 이동할 칸 수
    // public int walkLength ;
    //
    // 반복 간 시작 지점을 랜덤하게 할지의 여부.
    //public bool startRandomlyEachIteration 

    [SerializeField] 
    protected SimpleRandomWalkSO randomWalkSo;

   
    
    // 던전 생성 시작 함수
    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkSo,startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        
        foreach (var position in floorPositions)
            {
                Debug.Log(position);
            }
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkSo.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkSo.walkLength);
            floorPositions.UnionWith(path);
            if (randomWalkSo.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
}