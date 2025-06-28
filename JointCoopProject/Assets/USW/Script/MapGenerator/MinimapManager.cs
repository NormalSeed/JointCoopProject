using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    [Header("기본 설정")]
    public Canvas minimapCanvas;                    // 미니맵 캔버스
    public RectTransform container;                 // 미니맵 컨테이너
    public GameObject roomPrefab;                   // 방 아이콘 프리팹
    public Vector2 iconSize = new Vector2(20f, 20f);   // 아이콘 크기
    public float spacing = 25f;                     // 방 간격
    
    [Header("배경 설정")]
    public GameObject backgroundPrefab;             // 배경 프리팹
    public RectTransform bgTransform;              // 배경 Transform
    
    [Header("아이콘들")]
    public Sprite startIcon;                       // 시작방 아이콘
    public Sprite normalIcon;                      // 일반방 아이콘
    public Sprite bossIcon;                        // 보스방 아이콘
    public Sprite shopIcon;                        // 상점방 아이콘
    public Sprite itemIcon;                        // 아이템방 아이콘
    public Sprite secretIcon;                      // 비밀방 아이콘
    public Sprite currentIcon;                     // 현재방 아이콘
    public Sprite hiddenIcon;                      // 숨겨진방 아이콘
    
    [Header("색상 설정")]
    public Color visitedColor = Color.white;       // 방문한 방 색상
    public Color unvisitedColor = Color.gray;      // 방문하지 않은 방 색상
    public Color playerColor = Color.yellow;       // 플레이어 위치 색상
    
    // 내부 변수들
    private Dictionary<Vector2Int, MinimapRoom> rooms = new Dictionary<Vector2Int, MinimapRoom>();
    private MapGenerator mapGen;                   // 맵 생성기 참조
    private Vector2Int playerRoom;                 // 플레이어 현재 방
    
    // 디버그/테스트용
    public bool showAllRooms = false;              // 모든 방 표시 여부
    private bool initialized = false;              // 초기화 완료 여부
    
    void Start()
    {
        mapGen = FindObjectOfType<MapGenerator>();
        if (mapGen == null)
        {
            Debug.LogError("MapGenerator를 찾을 수 없음!");
            return;
        }
        
        SetupBackground();
        StartCoroutine(WaitAndGenerate());
    }
    
    void Update()
    {
        // 디버그 컨트롤
        if (Input.GetKeyDown(KeyCode.M))
        {
            showAllRooms = !showAllRooms;
            Debug.Log("모든 방 표시: " + showAllRooms);
            if (initialized) RefreshAllRooms();
        }
    }
    
    void SetupBackground()
    {
        // 기본 배경 제거
        Image bg = container.GetComponent<Image>();
        if (bg != null)
        {
            bg.color = Color.clear;
        }
        
        // 커스텀 배경이 있으면 추가
        if (backgroundPrefab != null)
        {
            GameObject bgObj = Instantiate(backgroundPrefab, container);
            bgTransform = bgObj.GetComponent<RectTransform>();
            
            if (bgTransform == null)
            {
                bgTransform = bgObj.AddComponent<RectTransform>();
            }
            
            // 컨테이너 전체에 맞게 늘리기
            bgTransform.anchorMin = Vector2.zero;
            bgTransform.anchorMax = Vector2.one;
            bgTransform.offsetMin = Vector2.zero;
            bgTransform.offsetMax = Vector2.zero;
            bgTransform.anchoredPosition = Vector2.zero;
            
            // 배경을 맨 뒤로 보내기
            bgTransform.SetAsFirstSibling();
            bgObj.name = "MinimapBackground";
        }
    }
    
    IEnumerator WaitAndGenerate()
    {
        yield return new WaitForSeconds(0.1f);
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        ClearMap();
        
        if (mapGen.generatedRooms == null || mapGen.generatedRooms.Count == 0)
        {
            Debug.LogWarning("미니맵에 표시할 방이 없음");
            return;
        }
        
        // 방 아이콘들 생성
        foreach (var roomPair in mapGen.generatedRooms)
        {
            CreateRoomIcon(roomPair.Key, roomPair.Value);
        }
        
        // 시작방을 현재 방으로 설정
        SetCurrentRoom(mapGen.startPosition);
        initialized = true;
        
        Debug.Log($"미니맵 생성 완료: {rooms.Count}개 방");
    }
    
    void CreateRoomIcon(Vector2Int pos, MapGenerator.RoomData roomData)
    {
        GameObject roomObj = Instantiate(roomPrefab, container);
        MinimapRoom room = roomObj.GetComponent<MinimapRoom>();
        
        if (room == null)
        {
            room = roomObj.AddComponent<MinimapRoom>();
        }
        
        // 아이콘 위치 설정
        Vector2 uiPos = GetUIPosition(pos);
        RectTransform rect = roomObj.GetComponent<RectTransform>();
        rect.anchoredPosition = uiPos;
        rect.sizeDelta = iconSize;
        
        // 방 초기화
        room.Setup(pos, roomData, GetIcon(roomData.roomType));
        rooms.Add(pos, room);
        
        // 방 아이콘을 배경 위에 표시
        roomObj.transform.SetAsLastSibling();
        
        // 시작방이 아니면 처음엔 숨기기
        if (roomData.roomType != MapGenerator.RoomType.Start && !showAllRooms)
        {
            room.SetVisible(false);
        }
    }
    
    Vector2 GetUIPosition(Vector2Int gridPos)
    {
        Vector2Int center = mapGen.startPosition;
        Vector2 offset = new Vector2(
            (gridPos.x - center.x) * spacing,
            (gridPos.y - center.y) * spacing
        );
        return offset;
    }
    
    Sprite GetIcon(MapGenerator.RoomType type)
    {
        switch (type)
        {
            case MapGenerator.RoomType.Start:
                return startIcon ? startIcon : normalIcon;
            case MapGenerator.RoomType.Boss:
                return bossIcon;
            case MapGenerator.RoomType.Shop:
                return shopIcon;
            case MapGenerator.RoomType.Item:
                return itemIcon;
            case MapGenerator.RoomType.Secret:
                return secretIcon;
            default:
                return normalIcon;
        }
    }
    
    public void SetCurrentRoom(Vector2Int pos)
    {
        // 이전 현재 방 표시 제거
        if (rooms.ContainsKey(playerRoom))
        {
            rooms[playerRoom].SetCurrent(false);
        }
    
        // 새로운 현재 방 설정
        playerRoom = pos;
        if (rooms.ContainsKey(pos))
        {
            rooms[pos].SetCurrent(true);
            rooms[pos].SetVisited(true);
        
            // 인접한 방들도 표시
            ShowAdjacentRooms(pos);
        
            // 미니맵을 현재 방 중심으로 이동
            RecenterMinimap(pos);
        }
    }
    
    void RecenterMinimap(Vector2Int centerRoom)
    {
        Vector2Int newCenter = centerRoom;
    
        // 모든 방 위치 재계산
        foreach (var roomPair in rooms)
        {
            Vector2Int gridPos = roomPair.Key;
            RectTransform rect = roomPair.Value.GetComponent<RectTransform>();
        
            Vector2 newPos = new Vector2(
                (gridPos.x - newCenter.x) * spacing,
                (gridPos.y - newCenter.y) * spacing
            );
        
            rect.anchoredPosition = newPos;
        }
    
        // 배경은 항상 중앙에 고정
        if (bgTransform != null)
        {
            bgTransform.anchoredPosition = Vector2.zero;
        }
    }
    
    void ShowAdjacentRooms(Vector2Int center)
    {
        Vector2Int[] dirs = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };
        
        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2Int adjacentPos = center + dirs[i];
            if (rooms.ContainsKey(adjacentPos))
            {
                rooms[adjacentPos].SetVisible(true);
            }
        }
    }
    
    public void VisitRoom(Vector2Int pos)
    {
        if (rooms.ContainsKey(pos))
        {
            rooms[pos].SetVisited(true);
        }
    }
    
    public void RevealSecretRoom(Vector2Int pos)
    {
        if (rooms.ContainsKey(pos))
        {
            rooms[pos].SetVisible(true);
            rooms[pos].SetVisited(true);
            Debug.Log("비밀방이 " + pos + "에서 발견됨");
        }
    }
    
    void RefreshAllRooms()
    {
        foreach (var room in rooms.Values)
        {
            if (showAllRooms)
            {
                room.SetVisible(true);
            }
            else
            {
                // 보이면 안 되는 방들 숨기기
                if (room.GetRoomData().roomType != MapGenerator.RoomType.Start && 
                    !room.IsVisited())
                {
                    room.SetVisible(false);
                }
            }
        }
    }
    
    void ClearMap()
    {
        foreach (var room in rooms.Values)
        {
            if (room != null && room.gameObject != null)
            {
                if (Application.isPlaying)
                    Destroy(room.gameObject);
                else
                    DestroyImmediate(room.gameObject);
            }
        }
        rooms.Clear();
    }
    
    // 테스트용 컨텍스트 메뉴
    [ContextMenu("배경 설정")]
    public void DoSetupBackground()
    {
        SetupBackground();
    }
    
    [ContextMenu("재생성")]
    public void Regenerate()
    {
        GenerateMap();
    }
    
    // 런타임에서 배경 변경
    public void ChangeBackground(GameObject newBg)
    {
        if (bgTransform != null)
        {
            if (Application.isPlaying)
                Destroy(bgTransform.gameObject);
            else
                DestroyImmediate(bgTransform.gameObject);
        }
        
        backgroundPrefab = newBg;
        SetupBackground();
    }
    
    // 호환성을 위한 기존 코드용 메서드
    public void GenerateMinimap()
    {
        GenerateMap();
    }
    
    // 게터 함수들
    public Vector2Int GetPlayerRoom() { return playerRoom; }
    public int GetRoomCount() { return rooms.Count; }
    public bool IsInitialized() { return initialized; }
}