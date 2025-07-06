using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    [Header("미니맵 설정")]
    public Canvas minimapCanvas;
    public RectTransform minimapContainer;
    public GameObject minimapRoomPrefab;
    public Vector2 roomIconSize = new Vector2(20f, 20f);
    public float roomSpacing = 25f;
    
    [Header("방 아이콘")]
    public Sprite startRoomIcon;
    public Sprite defaultRoomIcon;
    public Sprite bossRoomIcon;
    public Sprite shopRoomIcon;
    public Sprite itemRoomIcon;
    public Sprite secretRoomIcon;
    public Sprite currentRoomIcon;
    public Sprite unexploredRoomIcon;
    
    [Header("색상")]
    public Color exploredColor = Color.white;
    public Color unexploredColor = Color.gray;
    public Color currentRoomColor = Color.yellow;
    
    private Dictionary<Vector2Int, MinimapRoom> minimapRooms = new Dictionary<Vector2Int, MinimapRoom>();
    private MapGenerator mapGenerator; 
    private Vector2Int currentPlayerRoom;
    
    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        if (mapGenerator == null)
        {
            Debug.LogError("MapGenerator를 찾을 수 없음");
            return;
        }
        
        StartCoroutine(DelayedSetup());
    }
    
    IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(0.1f);
        GenerateMinimap();
    }
    
    public void GenerateMinimap()
    {
        ClearMinimap();
        
        if (mapGenerator == null || mapGenerator.generatedRooms.Count == 0) 
        {
            Debug.Log("맵 생성기가 없거나 방이 없음");
            return;
        }
        
        if (minimapContainer == null)
        {
            Debug.Log("미니맵 컨테이너가 할당되지 않음");
            return;
        }
        
        if (minimapRoomPrefab == null)
        {
            Debug.Log("미니맵 룸 프리팹이 할당되지 않음");
            return;
        }
        
        foreach (var roomPair in mapGenerator.generatedRooms)
        {
            if (roomPair.Value.roomType == MapGenerator.RoomType.Secret)
            {
                continue;
            }
            CreateMinimapRoom(roomPair.Key, roomPair.Value);
        }
        
        SetCurrentRoom(mapGenerator.startPosition);
    }
    
    void CreateMinimapRoom(Vector2Int gridPos, MapGenerator.RoomData roomData)
    {
        GameObject roomObj = Instantiate(minimapRoomPrefab, minimapContainer);
        MinimapRoom minimapRoom = roomObj.GetComponent<MinimapRoom>();
        
        if (minimapRoom == null)
        {
            Debug.LogError("미니맵 룸 프리팹에 MinimapRoom 컴포넌트가 없음");
            return;
        }
        
        RectTransform rectTransform = roomObj.GetComponent<RectTransform>();
        // 초기에는 시작방 기준으로 배치
        Vector2Int tempCenter = mapGenerator.startPosition;
        Vector2 initialPos = new Vector2(
            (gridPos.x - tempCenter.x) * roomSpacing,
            (gridPos.y - tempCenter.y) * roomSpacing
        );
        rectTransform.anchoredPosition = initialPos;
        rectTransform.sizeDelta = roomIconSize;
        
        minimapRoom.Initialize(gridPos, roomData, GetRoomIcon(roomData.roomType));
        minimapRooms.Add(gridPos, minimapRoom);
        
        if (roomData.roomType != MapGenerator.RoomType.Start)
        {
            minimapRoom.SetExplored(false);
        }
    }
    
    Vector2 GridToUIPosition(Vector2Int gridPos)
    {
        Vector2Int centerGrid = currentPlayerRoom; // 현재 플레이어 위치를 중심으로
        return new Vector2(
            (gridPos.x - centerGrid.x) * roomSpacing,
            (gridPos.y - centerGrid.y) * roomSpacing
        );
    }
    
    Sprite GetRoomIcon(MapGenerator.RoomType roomType)
    {
        switch (roomType)
        {
            case MapGenerator.RoomType.Start: return startRoomIcon ? startRoomIcon : defaultRoomIcon;
            case MapGenerator.RoomType.Boss: return bossRoomIcon;
            case MapGenerator.RoomType.Shop: return shopRoomIcon;
            case MapGenerator.RoomType.Item: return itemRoomIcon;
            case MapGenerator.RoomType.Secret: return secretRoomIcon;
            default: return defaultRoomIcon;
        }
    }
    
    public void SetCurrentRoom(Vector2Int roomPosition)
    {
        if (minimapRooms.ContainsKey(currentPlayerRoom))
        {
            minimapRooms[currentPlayerRoom].SetAsCurrent(false);
        }
        
        currentPlayerRoom = roomPosition;
        if (minimapRooms.ContainsKey(roomPosition))
        {
            minimapRooms[roomPosition].SetAsCurrent(true);
            minimapRooms[roomPosition].SetExplored(true);
            RevealAdjacentRooms(roomPosition);
        }
        
        // 플레이어 이동 시 미니맵 위치 업데이트
        UpdateMinimapPositions();
    }
    
    void UpdateMinimapPositions()
    {
        foreach (var roomPair in minimapRooms)
        {
            Vector2Int gridPos = roomPair.Key;
            MinimapRoom room = roomPair.Value;
            
            Vector2 newPosition = GridToUIPosition(gridPos);
            room.GetComponent<RectTransform>().anchoredPosition = newPosition;
        }
    }
    
    void RevealAdjacentRooms(Vector2Int centerPos)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int adjacentPos = centerPos + directions[i];
            if (minimapRooms.ContainsKey(adjacentPos))
            {
                minimapRooms[adjacentPos].SetVisible(true);
            }
        }
    }
    
    public void ExploreRoom(Vector2Int roomPosition)
    {
        if (minimapRooms.ContainsKey(roomPosition))
        {
            minimapRooms[roomPosition].SetExplored(true);
        }
    }

    public void HideSecretRoom(Vector2Int secretRoomPos)
    {
        if (minimapRooms.ContainsKey(secretRoomPos))
        {
            minimapRooms[secretRoomPos].SetExplored(false);
        }
    }
    
    public void RevealSecretRoom(Vector2Int secretRoomPos)
    {
        if (!minimapRooms.ContainsKey(secretRoomPos))
        {
            var secretRoomData = mapGenerator.generatedRooms[secretRoomPos];
            CreateMinimapRoom(secretRoomPos, secretRoomData);
        }
        
        if (minimapRooms.ContainsKey(secretRoomPos))
        {
            minimapRooms[secretRoomPos].SetVisible(true);
            minimapRooms[secretRoomPos].SetExplored(true);
        }
    }
    
    void ClearMinimap()
    {
        foreach (var room in minimapRooms.Values)
        {
            if (room != null && room.gameObject != null)
                Destroy(room.gameObject);
        }
        minimapRooms.Clear();
    }
    
    [ContextMenu("미니맵 재생성")]
    public void RegenerateMinimap()
    {
        GenerateMinimap();
    }
    
    public Vector2Int GetCurrentRoom() 
    { 
        return currentPlayerRoom; 
    }
}