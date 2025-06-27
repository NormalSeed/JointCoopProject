using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    public GameObject startRoomPrefab;
    public GameObject[] defaultRoomPrefabs = new GameObject[20];
    public GameObject shopRoomPrefab;
    public GameObject itemRoomPrefab;
    public GameObject secretRoomPrefab;
    public GameObject[] bossRoomPrefabs = new GameObject[3];
    
    [Header("Map Settings")]
    public int mapSize = 13;
    public Vector2Int startPosition = new Vector2Int(7, 7);
    public int totalRooms = 9;
    public float roomGenerationChance = 0.7f;
    
    [Header("Prefab Settings")]
    public Vector2 prefabSize = new Vector2(15, 9);
    
    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Vector2 playerSpawnOffset = new Vector2(7.5f, 4.5f);
    
    [Header("System References")]
    public CameraController cameraSystem;
    public MinimapManager minimapManager;
    
    // 생성된 방 데이터
    public Dictionary<Vector2Int, RoomData> generatedRooms = new Dictionary<Vector2Int, RoomData>();
    
    // 확장 가능한 좌표 
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    
    private GameObject spawnedPlayer;   
    private int currentAttempts = 0;        // 시도 횟수
    private int maxGenerationAttempts = 30; // 최대 시도 횟수

    [System.Serializable]
    public class RoomData
    {
        public Vector2Int position;
        public RoomType roomType;
        public GameObject roomPrefab;
        public GameObject instantiatedRoom;
        public bool isGenerated;
        public bool isDeadEnd;
        
        // 방 데이터 초기화 
        public RoomData(Vector2Int pos, RoomType type, GameObject prefab)
        {
            position = pos;
            roomType = type;
            roomPrefab = prefab;
            isGenerated = false;
            isDeadEnd = false;  
        }
    }
    
    public enum RoomType
    {
        Start, Default, Shop, Item, Secret, Boss
    }

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        ClearMap();         // 기존 맵 정리
        currentAttempts++;  // 시도 횟수 증가
        
        // 현재 시도 횟수가 최대 횟수를 넘었을때 return (안정장치)
        if (currentAttempts > maxGenerationAttempts)
        {
            return;
        }

        // 1단계: 기본 구조 생성 (막다른길 3개 이상 보장)
        if (!GenerateBasicMapStructure())
        {
            GenerateMap();
            return;
        }
        
        // 2단계: 특수방 배치 (막다른길 우선)
        if (!GenerateSpecialRoomsFlexible())
        {
            GenerateMap();
            return;
        }
        
        // 3단계: 비밀방 생성
        GenerateSecretRoom();   
        
        // 4단계: 남은 공간에 추가 일반방 배치
        FillRemainingSpaces();  
        
        // 5단계: 최종 검증 (특수방들이 진짜 막다른길인지 확인)
        if (!ValidateSpecialRoomsPlacement())
        {
            GenerateMap();
            return;
        }
        
        // 6단계: 실제 프리팹 생성 및 시스템 설정
        InstantiateRooms();     
        SpawnPlayer();          
        SetupSystems();         
        
        currentAttempts = 0;    // 성공적으로 생성되면 시도 횟수 리셋 
    }

    bool GenerateBasicMapStructure()
    {
        CreateStartRoom(); // 13*13 맵의 (7,7)에 시작방 생성 
        
        // 기본 방 구조 생성 (최소 3-4개 정도)
        if (!GenerateInitialRooms())
        {
            return false;
        }
        
        return true;
    }

    // 초기 기본방들 생성 (막다른길 5개 이상 보장으로 여유분 확보)
    bool GenerateInitialRooms()
    {
        int attempts = 0;   
        int generated = 0;
        int maxAttempts = 200;
        int minDeadEnds = 5; // 3개 + 여유분 2개 = 5개 이상 확보
        
        while (attempts < maxAttempts && availablePositions.Count > 0)
        {
            attempts++;
            
            // 현재 막다른길 개수 확인
            List<Vector2Int> currentDeadEnds = FindDeadEndPositions();
            
            // 막다른길이 5개 이상이고, 최소 기본방도 확보했으면 성공
            if (currentDeadEnds.Count >= minDeadEnds && generated >= 2)
            {
                return true;
            }
            
            // 랜덤 위치 선택하여 기본방 생성
            Vector2Int randomPos = availablePositions[Random.Range(0, availablePositions.Count)];
            
            if (IsValidPosition(randomPos) && Random.value < roomGenerationChance)
            {
                GameObject roomPrefab = defaultRoomPrefabs[Random.Range(0, defaultRoomPrefabs.Length)];
                RoomData newRoom = new RoomData(randomPos, RoomType.Default, roomPrefab);
                
                generatedRooms.Add(randomPos, newRoom);
                availablePositions.Remove(randomPos);
                AddAdjacentPositions(randomPos);
                generated++;
            }
            else
            {
                availablePositions.Remove(randomPos);
            }
        }
        
        // 최종 확인
        List<Vector2Int> finalDeadEnds = FindDeadEndPositions();
        bool success = finalDeadEnds.Count >= minDeadEnds && generated >= 2;
        Debug.Log($"최종 결과 - 막다른길: {finalDeadEnds.Count}개, 기본방: {generated}개, 성공: {success}");
        return success;
    }

    // 특수방 생성 순서 정의
    bool GenerateSpecialRoomsFlexible()
    {
        List<(RoomType type, GameObject prefab)> specialRooms = new List<(RoomType, GameObject)>
        {
            (RoomType.Boss, GetRandomBossRoomPrefab()),
            (RoomType.Item, itemRoomPrefab),
            (RoomType.Shop, shopRoomPrefab)
        };
        
        // 순서대로 생성 시도, 하나라도 실패하면 전체 실패
        foreach (var roomInfo in specialRooms)
        {
            if (!GenerateSpecialRoom(roomInfo.type, roomInfo.prefab))
            {
                return false;
            }
        }
        return true;
    }

    // 특수방 생성 규칙 (배치 전 완전한 막다른길 검증)
    bool GenerateSpecialRoom(RoomType roomType, GameObject prefab)
    {
        if (prefab == null) 
            return false;

        // 막다른길 후보 찾기
        List<Vector2Int> deadEndPositions = FindDeadEndPositions();
    
        // 보스방은 시작점에서 최소 2칸 이상 떨어진 막다른길에 배치
        if (roomType == RoomType.Boss && deadEndPositions.Count > 0)
        {
            deadEndPositions = FilterBossRoomPositions(deadEndPositions);
        }

        // 진짜 막다른길인지 한번 더 검증 (주변에 방이 정확히 1개만 있는지 확인)
        List<Vector2Int> validDeadEnds = new List<Vector2Int>();
        foreach (Vector2Int pos in deadEndPositions)
        {
            if (IsRealDeadEnd(pos))
            {
                validDeadEnds.Add(pos);
            }
        }

        // 진짜 막다른길이 없으면 실패
        if (validDeadEnds.Count == 0)
        {
            return false;
        }

        Vector2Int targetPos = validDeadEnds[Random.Range(0, validDeadEnds.Count)];
    
        RoomData specialRoom = new RoomData(targetPos, roomType, prefab);
        specialRoom.isDeadEnd = true;
        generatedRooms.Add(targetPos, specialRoom);
        availablePositions.Remove(targetPos);
        
        return true;
    }

    // 진짜 막다른길인지 확인 (인접한 생성된 방이 진짜 진짜 1개인지)
    bool IsRealDeadEnd(Vector2Int pos)
    {
        return CountAdjacentRooms(pos) == 1;
    }

    // 특수방 배치 최종 검증 (인스턴스화 전 막다른길 상태 재확인)
    bool ValidateSpecialRoomsPlacement()
    {
        
        List<RoomType> specialRoomTypes = new List<RoomType> { RoomType.Boss, RoomType.Item, RoomType.Shop };
        int validSpecialRooms = 0;
        
        foreach (var roomPair in generatedRooms)
        {
            RoomData room = roomPair.Value;
            
            // 특수방인지 확인
            if (specialRoomTypes.Contains(room.roomType))
            {
                Vector2Int pos = room.position;
                int adjacentCount = CountAdjacentRooms(pos);
                bool isActualDeadEnd = adjacentCount == 1;
                
                if (isActualDeadEnd)
                {
                    validSpecialRooms++;
                }
                else
                {
                    return false; // 하나라도 막다른길이 아니면 실패
                }
            }
        }
        
        bool success = validSpecialRooms == 3; // Boss, Item, Shop 총 3개가 모두 막다른길이어야 함
        
        return success;
    }

    // 인접한 생성된 방의 개수 세기
    int CountAdjacentRooms(Vector2Int pos)
    {
        int count = 0;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = pos + dir;
            if (generatedRooms.ContainsKey(checkPos))
            {
                count++;
            }
        }
        
        return count;
    }

    // 남은 공간에 추가 일반방 배치 (특수방 배치 후 호출)
    void FillRemainingSpaces()
    {
        // 총 방 개수에서 현재 생성된 방 개수를 빼서 남은 일반방 개수 계산
        int currentRoomCount = generatedRooms.Count;
        int remainingRooms = totalRooms - currentRoomCount;
        
        if (remainingRooms <= 0) return;
        
        int attempts = 0;
        int generated = 0;
        
        while (generated < remainingRooms && attempts < 100 && availablePositions.Count > 0)
        {
            attempts++;
            
            Vector2Int randomPos = availablePositions[Random.Range(0, availablePositions.Count)];
            
            if (IsValidPosition(randomPos) && Random.value < roomGenerationChance)
            {
                GameObject roomPrefab = defaultRoomPrefabs[Random.Range(0, defaultRoomPrefabs.Length)];
                RoomData newRoom = new RoomData(randomPos, RoomType.Default, roomPrefab);
                
                generatedRooms.Add(randomPos, newRoom);
                availablePositions.Remove(randomPos);
                AddAdjacentPositions(randomPos);
                generated++;
            }
            else
            {
                availablePositions.Remove(randomPos);
            }
        }
    }

    // 보스방 배치를 위한 위치 필터링
    List<Vector2Int> FilterBossRoomPositions(List<Vector2Int> positions)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();
        
        foreach (Vector2Int pos in positions)
        {
            // 시작점에서 최소 2칸 이상 떨어진 곳만 선택
            if (Vector2Int.Distance(pos, startPosition) >= 2)
            {
                validPositions.Add(pos);
            }
        }
        
        return validPositions;
    }
    
    // 기존 데이터와 후보 위치 초기화 후 시작방 생성
    void CreateStartRoom()
    {
        generatedRooms.Clear();
        availablePositions.Clear();
        
        GameObject startPrefab = startRoomPrefab != null ? startRoomPrefab : defaultRoomPrefabs[0];
        RoomData startRoom = new RoomData(startPosition, RoomType.Start, startPrefab);
        generatedRooms.Add(startPosition, startRoom);
        AddAdjacentPositions(startPosition);
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null || !generatedRooms.ContainsKey(startPosition)) 
            return;

        // 기존 플레이어가 있으면 제거
        if (spawnedPlayer != null)
        {
            if (Application.isPlaying) 
                Destroy(spawnedPlayer);
            else 
                DestroyImmediate(spawnedPlayer);
        }

        RoomData startRoom = generatedRooms[startPosition];
        Vector3 startRoomPos = startRoom.instantiatedRoom.transform.position;
        Vector3 prefabCenter = new Vector3(prefabSize.x * 0.5f, prefabSize.y * 0.5f, 0);
        Vector3 playerSpawnPos = startRoomPos + prefabCenter + 
            new Vector3(playerSpawnOffset.x - prefabSize.x * 0.5f, playerSpawnOffset.y - prefabSize.y * 0.5f, 0);

        spawnedPlayer = Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        spawnedPlayer.name = "Player";
        spawnedPlayer.tag = "Player";
    }

    void SetupSystems()
    {
        // 시스템 참조가 없으면 자동으로 찾기
        if (cameraSystem == null) 
            cameraSystem = FindObjectOfType<CameraController>();
        if (minimapManager == null) 
            minimapManager = FindObjectOfType<MinimapManager>();
        
        // 카메라 시스템 설정
        if (cameraSystem != null)
        {
            cameraSystem.SetupForRoom(startPosition);
        }
        
        // 미니맵 설정 (약간의 지연 후 실행)
        if (minimapManager != null)
        {
            StartCoroutine(DelayedMinimapSetup());
        }
    }

    IEnumerator DelayedMinimapSetup()
    {
        yield return new WaitForEndOfFrame();
        minimapManager.GenerateMinimap();
    }

    // 막다른길 위치 찾기 
    List<Vector2Int> FindDeadEndPositions()
    {
        List<Vector2Int> deadEndPositions = new List<Vector2Int>();
        
        foreach (Vector2Int pos in availablePositions)
        {
            if (IsValidPosition(pos) && IsDeadEndPosition(pos))
            {
                deadEndPositions.Add(pos);
            }
        }
        
        return deadEndPositions;
    }

    // 막다른길인지 확인 
    bool IsDeadEndPosition(Vector2Int pos)
    {
        int adjacentCount = 0;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = pos + dir;
            // 인접한 방 개수 세기
            if (generatedRooms.ContainsKey(checkPos)) 
            {
                adjacentCount++;
            }
        }
        
        bool isDeadEnd = adjacentCount == 1;
        return isDeadEnd;
    }

    // 인접한 위치들을 후보에 추가
    void AddAdjacentPositions(Vector2Int centerPos)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = centerPos + dir;
            
            // 맵 경계 내에 있고, 이미 방이 없고, 중복되지 않은 곳만 후보로 등록
            if (IsInMapBounds(newPos) && 
                !generatedRooms.ContainsKey(newPos) && 
                !availablePositions.Contains(newPos))
            {
                availablePositions.Add(newPos);
            }
        }
    }

    // 유효한 위치인지 확인
    bool IsValidPosition(Vector2Int pos) 
    {
        return IsInMapBounds(pos) && !generatedRooms.ContainsKey(pos);
    }
    
    // 맵 경계 내에 있는지 확인
    bool IsInMapBounds(Vector2Int pos) 
    {
        return pos.x >= 0 && pos.x < mapSize && pos.y >= 0 && pos.y < mapSize;
    }

    // 랜덤 보스방 프리팹 선택
    GameObject GetRandomBossRoomPrefab()
    {
        if (bossRoomPrefabs == null || bossRoomPrefabs.Length == 0)
            return null;
            
        return bossRoomPrefabs[Random.Range(0, bossRoomPrefabs.Length)];
    }

    // 비밀방 생성 (4면 막힘 우선, 3면, 2면 순으로 탐색)
    void GenerateSecretRoom()
    {
        if (secretRoomPrefab == null) 
            return;
    
        List<Vector2Int> perfectCandidates = new List<Vector2Int>();
        
        // 1차: 4면이 막힌 위치 찾기
        perfectCandidates = FindPositionsWithSurroundingCount(4);
        
        // 2차: 4면 막힌 곳이 없으면 3면 막힌 곳 찾기
        if (perfectCandidates.Count == 0)
        {
            perfectCandidates = FindPositionsWithSurroundingCount(3);
        }
        
        // 3차: 그래도 정 없으면 2면 이상 막힌 곳 찾기
        if (perfectCandidates.Count == 0)
        {
            perfectCandidates = FindPositionsWithSurroundingCount(2, true);
        }
    
        // 적절한 위치가 있으면 비밀방 생성
        if (perfectCandidates.Count > 0)
        {
            Vector2Int secretPos = perfectCandidates[Random.Range(0, perfectCandidates.Count)];
            RoomData secretRoom = new RoomData(secretPos, RoomType.Secret, secretRoomPrefab);
            generatedRooms.Add(secretPos, secretRoom);
            availablePositions.Remove(secretPos);
        }
    }

    // 특정 개수만큼 둘러싸인 위치들 찾기
    List<Vector2Int> FindPositionsWithSurroundingCount(int targetCount, bool orMore = false)
    {
        List<Vector2Int> candidates = new List<Vector2Int>();
        
        foreach (Vector2Int pos in availablePositions)
        {
            int surroundingCount = CountSurroundingRooms(pos);
            
            if (orMore)
            {
                // targetCount 이상인 경우
                if (surroundingCount >= targetCount)
                {
                    candidates.Add(pos);
                }
            }
            else
            {
                // 정확히 targetCount인 경우
                if (surroundingCount == targetCount)
                {
                    candidates.Add(pos);
                }
            }
        }
        
        return candidates;
    }

    // 주변 방의 개수 세기
    int CountSurroundingRooms(Vector2Int pos)
    {
        int count = 0;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        foreach (Vector2Int dir in directions)
        {
            if (generatedRooms.ContainsKey(pos + dir))
            {
                count++;
            }
        }
        
        return count;
    }

    // 모든 방 프리팹을 실제로 인스턴스화
    void InstantiateRooms()
    {
        foreach (var roomPair in generatedRooms)
        {
            RoomData room = roomPair.Value;
            
            if (room.roomPrefab != null && !room.isGenerated)
            {
                // 월드 좌표 계산
                Vector3 worldPos = new Vector3(room.position.x * prefabSize.x, room.position.y * prefabSize.y, 0);
                
                // 방 인스턴스 생성
                room.instantiatedRoom = Instantiate(room.roomPrefab, worldPos, Quaternion.identity, transform);
                room.instantiatedRoom.name = $"{room.roomType}Room_{room.position.x}_{room.position.y}";
                room.isGenerated = true;
                
                // RoomController 컴포넌트 설정
                var roomController = room.instantiatedRoom.GetComponent<RoomController>();
                if (roomController == null)
                {
                    roomController = room.instantiatedRoom.AddComponent<RoomController>();
                }
                roomController.SetRoomData(room);
            }
        }
    }

    // 기존 맵 정리
    void ClearMap()
    {
        // 모든 방 제거
        foreach (var room in generatedRooms.Values)
        {
            if (room.instantiatedRoom != null)
            {
                if (Application.isPlaying) 
                    Destroy(room.instantiatedRoom);
                else 
                    DestroyImmediate(room.instantiatedRoom);
            }
        }
        
        // 플레이어 제거
        if (spawnedPlayer != null)
        {
            if (Application.isPlaying) 
                Destroy(spawnedPlayer);
            else 
                DestroyImmediate(spawnedPlayer);
        }
        
        // 데이터 초기화
        generatedRooms.Clear();
        availablePositions.Clear();
    }

    [ContextMenu("Regenerate Map")]
    public void RegenerateMap()
    {
        currentAttempts = 0;
        GenerateMap();
    }
}