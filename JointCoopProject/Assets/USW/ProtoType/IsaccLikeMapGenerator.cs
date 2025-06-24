using System.Collections.Generic;
using UnityEngine;

public class IsaacLikeRoomGenerator : MonoBehaviour
{
    [Header("방 설정")]
    public List<GameObject> roomPrefabs; 
    public int numberOfRooms = 10;

    [Header("타일 사이즈")]
    public float tileWidth = 1f;
    public float tileHeight = 1f;

    [Header("방 타일 크기")]
    public Vector2Int roomTileSize = new Vector2Int(13, 8); 

    private Dictionary<Vector2Int, GameObject> placedRooms = new();
    private Vector2Int[] directions = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    void Start()
    {
        GenerateRooms();
    }

    void GenerateRooms()
    {
        Queue<Vector2Int> toVisit = new();
        HashSet<Vector2Int> visited = new();

        Vector2Int start = Vector2Int.zero;
        toVisit.Enqueue(start);
        visited.Add(start);
        SpawnRoom(start);

        while (visited.Count < numberOfRooms && toVisit.Count > 0)
        {
            Vector2Int current = toVisit.Dequeue();

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (!visited.Contains(neighbor) && Random.value < 0.5f)
                {
                    visited.Add(neighbor);
                    toVisit.Enqueue(neighbor);
                    SpawnRoom(neighbor);
                }
            }
        }
    }

    void SpawnRoom(Vector2Int gridPosition)
    {
        Vector2 worldPos = new Vector2(
            gridPosition.x * roomTileSize.x * tileWidth,
            gridPosition.y * roomTileSize.y * tileHeight
        );
        
        GameObject prefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        GameObject room = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        placedRooms.Add(gridPosition, room);
    }
}