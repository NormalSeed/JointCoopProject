using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    [Header("Minimap Settings")]
    public Canvas minimapCanvas;
    public RectTransform minimapContainer;
    public GameObject minimapRoomPrefab;
    public Vector2 roomIconSize = new Vector2(20f, 20f);
    public float roomSpacing = 25f;
    
    [Header("Custom Shadow Background")]
    public GameObject customShadowPrefab; 
    public RectTransform shadowBackground; 
    
    [Header("Room Icons")]
    public Sprite startRoomIcon;
    public Sprite defaultRoomIcon;
    public Sprite bossRoomIcon;
    public Sprite shopRoomIcon;
    public Sprite itemRoomIcon;
    public Sprite secretRoomIcon;
    public Sprite currentRoomIcon;
    public Sprite unexploredRoomIcon;
    
    [Header("Colors")]
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
            return;
        }
        
        // 커스텀 Shadow 배경 설정
        SetupCustomShadowBackground();
        
        // 맵 생성 완료 후 미니맵 생성
        StartCoroutine(WaitForMapGeneration());
    }
    
    void SetupCustomShadowBackground()
    {
        // 기존 배경 이미지 제거
        Image existingBg = minimapContainer.GetComponent<Image>();
        if (existingBg != null)
        {
            existingBg.color = Color.clear; // 투명하게 만들기
        }
        
        // 커스텀 Shadow 배경 생성
        if (customShadowPrefab != null)
        {
            GameObject shadowBgObj = Instantiate(customShadowPrefab, minimapContainer);
            shadowBackground = shadowBgObj.GetComponent<RectTransform>();
            
            if (shadowBackground == null)
            {
                shadowBackground = shadowBgObj.AddComponent<RectTransform>();
            }
            
            // 강제로 앵커 피벗 설정.
            shadowBackground.anchorMin = Vector2.zero;        // (0, 0)
            shadowBackground.anchorMax = Vector2.one;         // (1, 1)
            shadowBackground.offsetMin = Vector2.zero;        // Left=0, Bottom=0
            shadowBackground.offsetMax = Vector2.zero;        // Right=0, Top=0
            shadowBackground.anchoredPosition = Vector2.zero; // 중앙 정렬
            
            // Shadow를 맨 뒤에 배치 (방 아이콘들 뒤로)
            shadowBackground.SetAsFirstSibling();
            
            // Shadow 이름 설정
            shadowBgObj.name = "CustomShadowBackground";
            
        }
    }
    
    IEnumerator WaitForMapGeneration()
    {
        // 맵 생성이 완료될 때까지 대기
        yield return new WaitForSeconds(0.1f);
        GenerateMinimap();
    }
    
    public void GenerateMinimap()
    {
        ClearMinimap();
        
        if (mapGenerator.generatedRooms == null || mapGenerator.generatedRooms.Count == 0)
        {
            return;
        }
        
        // 각 방에 대해 미니맵 아이콘 생성
        foreach (var roomPair in mapGenerator.generatedRooms)
        {
            CreateMinimapRoom(roomPair.Key, roomPair.Value);
        }
        
        // 시작 방을 현재 방으로 설정
        SetCurrentRoom(mapGenerator.startPosition);
    }
    
    void CreateMinimapRoom(Vector2Int gridPos, MapGenerator.RoomData roomData)
    {
        // 미니맵 룸 오브젝트 생성
        GameObject roomObj = Instantiate(minimapRoomPrefab, minimapContainer);
        MinimapRoom minimapRoom = roomObj.GetComponent<MinimapRoom>();
        
        if (minimapRoom == null)
        {
            minimapRoom = roomObj.AddComponent<MinimapRoom>();
        }
        
        // 위치 설정 (그리드 좌표를 UI 좌표로 변환)
        Vector2 uiPosition = GridToUIPosition(gridPos);
        roomObj.GetComponent<RectTransform>().anchoredPosition = uiPosition;
        roomObj.GetComponent<RectTransform>().sizeDelta = roomIconSize;
        
        // 미니맵 룸 초기화
        minimapRoom.Initialize(gridPos, roomData, GetRoomIcon(roomData.roomType));
        minimapRooms.Add(gridPos, minimapRoom);
        
        // Shadow 배경 위에 방 아이콘이 오도록 순서 조정
        roomObj.transform.SetAsLastSibling();
        
        // 시작 방이 아닌 경우 탐험되지 않은 상태로 설정
        if (roomData.roomType != MapGenerator.RoomType.Start)
        {
            minimapRoom.SetExplored(false);
        }
    }
    
    Vector2 GridToUIPosition(Vector2Int gridPos)
    {
        // 중앙을 기준으로 UI 위치 계산
        Vector2Int centerGrid = mapGenerator.startPosition;
        Vector2 offset = new Vector2(
            (gridPos.x - centerGrid.x) * roomSpacing,
            (gridPos.y - centerGrid.y) * roomSpacing
        );
        return offset;
    }
    
    Sprite GetRoomIcon(MapGenerator.RoomType roomType)
    {
        switch (roomType)
        {
            case MapGenerator.RoomType.Start:
                return startRoomIcon ? startRoomIcon : defaultRoomIcon;
            case MapGenerator.RoomType.Boss:
                return bossRoomIcon;
            case MapGenerator.RoomType.Shop:
                return shopRoomIcon;
            case MapGenerator.RoomType.Item:
                return itemRoomIcon;
            case MapGenerator.RoomType.Secret:
                return secretRoomIcon;
            default:
                return defaultRoomIcon;
        }
    }
    
    
    public void SetCurrentRoom(Vector2Int roomPosition)
    {
        // 이전 현재 방 표시 제거
        if (minimapRooms.ContainsKey(currentPlayerRoom))
        {
            minimapRooms[currentPlayerRoom].SetAsCurrent(false);
        }
    
        // 새로운 현재 방 설정
        currentPlayerRoom = roomPosition;
        if (minimapRooms.ContainsKey(roomPosition))
        {
            minimapRooms[roomPosition].SetAsCurrent(true);
            minimapRooms[roomPosition].SetExplored(true);
        
            // 인접한 방들도 표시 
            RevealAdjacentRooms(roomPosition);
        
            // 미니맵을 현재 방 중심으로 이동
            CenterMinimapOnCurrentRoom(roomPosition);
        }
    }
    void CenterMinimapOnCurrentRoom(Vector2Int roomPosition)
    {
        // 전체 미니맵 컨테이너를 이동시키는 방식 대신
        // 개별 방들의 상대 위치를 다시 계산
    
        Vector2Int centerGrid = roomPosition; // 현재 방을 중심으로 설정
    
        // 모든 방 아이콘들의 위치를 현재 방 기준으로 재계산
        foreach (var roomPair in minimapRooms)
        {
            Vector2Int gridPos = roomPair.Key;
            RectTransform roomRect = roomPair.Value.GetComponent<RectTransform>();
        
            // 현재 방을 중심으로 상대 위치 계산
            Vector2 relativePos = new Vector2(
                (gridPos.x - centerGrid.x) * roomSpacing,
                (gridPos.y - centerGrid.y) * roomSpacing
            );
        
            roomRect.anchoredPosition = relativePos;
        }
    
        // Shadow 배경은 항상 중앙에 고정
        if (shadowBackground != null)
        {
            shadowBackground.anchoredPosition = Vector2.zero;
        }
        
    }
    
    void RevealAdjacentRooms(Vector2Int centerPos)
    {
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };
        
        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPos = centerPos + dir;
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
    
    public void RevealSecretRoom(Vector2Int roomPosition)
    {
        if (minimapRooms.ContainsKey(roomPosition))
        {
            minimapRooms[roomPosition].SetVisible(true);
            minimapRooms[roomPosition].SetExplored(true);
        }
    }
    
    void ClearMinimap()
    {
        foreach (var room in minimapRooms.Values)
        {
            if (room != null && room.gameObject != null)
            {
                if (Application.isPlaying)
                    Destroy(room.gameObject);
                else
                    DestroyImmediate(room.gameObject);
            }
        }
        minimapRooms.Clear();
    }
    
    [ContextMenu("Setup Custom Shadow Background")]
    public void SetupShadowBackground()
    {
        SetupCustomShadowBackground();
    }
    
    [ContextMenu("Regenerate Minimap")]
    public void RegenerateMinimap()
    {
        GenerateMinimap();
    }
    
    // 커스텀 Shadow 배경 교체
    public void ChangeCustomShadowBackground(GameObject newShadowPrefab)
    {
        // 기존 Shadow 배경 제거
        if (shadowBackground != null)
        {
            if (Application.isPlaying)
                Destroy(shadowBackground.gameObject);
            else
                DestroyImmediate(shadowBackground.gameObject);
        }
        
        // 새로운 Shadow 배경 설정
        customShadowPrefab = newShadowPrefab;
        SetupCustomShadowBackground();
    }
    
    // 외부에서 현재 방 정보 가져오기
    public Vector2Int GetCurrentRoom() => currentPlayerRoom;
}