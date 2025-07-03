using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("방 프리팹들")] public GameObject startRoomPrefab;
    public GameObject[] defaultRoomPrefabs = new GameObject[20];
    public GameObject shopRoomPrefab;
    public GameObject itemRoomPrefab;
    public GameObject secretRoomPrefab;
    public GameObject[] bossRoomPrefabs = new GameObject[3];

    [Header("맵 설정")] public int mapSize = 13;
    public Vector2Int startPosition = new Vector2Int(7, 7);
    public int totalRooms;
    public float roomGenerationChance = 0.7f;

    [Header("프리팹 설정")] public Vector2 prefabSize = new Vector2(15, 9); // 방 프리팹 크기

    [Header("플레이어 설정")] public GameObject playerPrefab; // 플레이어 프리팹
    public Vector2 playerSpawnOffset = new Vector2(7.5f, 4.5f); // 스폰 오프셋


    [Header("문 설정")] public Sprite wallUpPrefab;
    public Sprite wallDownPrefab;
    public Sprite wallLeftPrefab;
    public Sprite wallRightPrefab;

    public Sprite doorClosedUpPrefab;
    public Sprite doorClosedDownPrefab;
    public Sprite doorClosedLeftPrefab;
    public Sprite doorClosedRightPrefab;

    public Sprite doorOpenUpPrefab;
    public Sprite doorOpenDownPrefab;
    public Sprite doorOpenLeftPrefab;
    public Sprite doorOpenRightPrefab;

    // 문 위치 설정
    private Vector2 doorUpPos = new Vector2(7.5f, 8.5f);
    private Vector2 doorDownPos = new Vector2(7.5f, 0.5f);
    private Vector2 doorLeftPos = new Vector2(0.5f, 4.5f);
    private Vector2 doorRightPos = new Vector2(14.5f, 4.5f);

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public CameraController cameraSystem;
    public MinimapManager minimapManager;

    // 생성된 방들 정보
    public Dictionary<Vector2Int, Dictionary<Direction, GameObject>> roomDoors =
        new Dictionary<Vector2Int, Dictionary<Direction, GameObject>>();

    public Dictionary<Vector2Int, RoomData> generatedRooms = new Dictionary<Vector2Int, RoomData>();
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private GameObject spawnedPlayer;
    private int currentAttempts = 0;
    private int maxGenerationAttempts = 50;

    // 디버그 관련
    public bool enableDebugLogs = false;
    private int failsafe = 0; // 무한루프 방지

    [System.Serializable]
    public class RoomData
    {
        public Vector2Int position;
        public RoomType roomType;
        public GameObject roomPrefab;
        public GameObject instantiatedRoom;
        public bool isGenerated;
        public bool isDeadEnd;

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
        Start,
        Default,
        Shop,
        Item,
        Secret,
        Boss
    }

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        // TODO: 나중에 이 디버그 코드 지우기
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("맵 재생성 중");
            RegenerateMap();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            enableDebugLogs = !enableDebugLogs;
            Debug.Log("디버그 로그: " + enableDebugLogs);
        }
    }

    public void GenerateMap()
    {
        ClearMap(); // 기존 맵 정리
        currentAttempts++; // 시도 횟수 증가

        if (currentAttempts > maxGenerationAttempts)
        {
            Debug.LogError("currentAttempts 가 30번넘음");
            return;
        }

        // 1단계: 기본 구조 생성
        if (!GenerateBasicMapStructure())
        {
            GenerateMap();
            return;
        }

        // 2단계: 특수방 배치
        if (!GenerateSpecialRoomsFlexible())
        {
            GenerateMap();
            return;
        }

        // 3단계: 비밀방 생성
        GenerateSecretRoom();


        // 4단계: 남은 공간에 일반방 추가
        FillRemainingSpaces();


        // 5단계: 최종 검증
        if (!ValidateSpecialRoomsPlacement())
        {
            if (enableDebugLogs) Debug.Log("최종찐찐막 검증 실패해버림");
            GenerateMap();
            return;
        }

        // 6단계: 실제 방들 생성 및 시스템 설정
        InstantiateRooms();

        GenerateDoorsAndWalls();

        SpawnPlayer();
        SetupSystems();

        currentAttempts = 0; // 성공하면 시도 횟수 리셋
        Debug.Log("맵 생성 성공 총 방 개수: " + generatedRooms.Count);
    }

    bool GenerateBasicMapStructure()
    {
        CreateStartRoom(); // 시작방 생성

        if (!GenerateInitialRooms())
        {
            return false;
        }

        return true;
    }

    // 초기 기본방들 생성
    bool GenerateInitialRooms()
    {
        int attempts = 0;
        int generated = 0;
        int maxAttempts = 200;
        int minDeadEnds = 5;

        while (attempts < maxAttempts && availablePositions.Count > 0)
        {
            attempts++;
            failsafe++; // 혹시 모를 무한루프 방지

            if (failsafe > 1000)
            {
                Debug.LogWarning("GenerateInitialRooms에서 failsafe 1000번넘어감");
                break;
            }

            // 현재 막다른길 개수 확인
            List<Vector2Int> currentDeadEnds = FindDeadEndPositions();

            if (currentDeadEnds.Count >= minDeadEnds && generated >= 2)
            {
                return true;
            }

            // 랜덤한 위치에 방 생성 시도
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

        // 최종 결과 확인
        List<Vector2Int> finalDeadEnds = FindDeadEndPositions();
        bool success = finalDeadEnds.Count >= minDeadEnds && generated >= 2;
        return success;
    }

    // 특수방들 생성 (보스방, 아이템방, 상점방 순서대로)
    bool GenerateSpecialRoomsFlexible()
    {
        // 보스방이 제일 조건 까다로우니깐
        if (!GenerateSpecialRoom(RoomType.Boss, GetRandomBossRoomPrefab()))
        {
            return false;
        }

        // 그 다음 아이템방
        if (!GenerateSpecialRoom(RoomType.Item, itemRoomPrefab))
        {
            return false;
        }

        // 마지막으로 상점
        if (!GenerateSpecialRoom(RoomType.Shop, shopRoomPrefab))
        {
            return false;
        }

        return true;
    }

    bool GenerateSpecialRoom(RoomType roomType, GameObject prefab)
    {
        if (prefab == null)
        {
            return false;
        }

        // 막다른길 후보 찾기
        List<Vector2Int> deadEndPositions = FindDeadEndPositions();

        // 보스방은 시작점에서 멀리 배치
        if (roomType == RoomType.Boss && deadEndPositions.Count > 0)
        {
            deadEndPositions = FilterBossRoomPositions(deadEndPositions);
        }

        // 진짜 막다른길인지 검증
        List<Vector2Int> validDeadEnds = new List<Vector2Int>();
        for (int i = 0; i < deadEndPositions.Count; i++)
        {
            Vector2Int pos = deadEndPositions[i];
            if (IsRealDeadEnd(pos))
            {
                validDeadEnds.Add(pos);
            }
        }

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

    bool IsRealDeadEnd(Vector2Int pos)
    {
        return CountAdjacentRooms(pos) == 1;
    }

    // 특수방 배치 최종 검증
    bool ValidateSpecialRoomsPlacement()
    {
        RoomType[] specialTypes = { RoomType.Boss, RoomType.Item, RoomType.Shop };
        int validCount = 0;

        foreach (var roomPair in generatedRooms)
        {
            RoomData room = roomPair.Value;

            bool isSpecial = false;
            for (int i = 0; i < specialTypes.Length; i++)
            {
                if (room.roomType == specialTypes[i])
                {
                    isSpecial = true;
                    break;
                }
            }

            if (isSpecial)
            {
                Vector2Int pos = room.position;
                int adjacentCount = CountAdjacentRooms(pos);
                bool isActualDeadEnd = adjacentCount == 1;

                if (isActualDeadEnd)
                {
                    validCount++;
                }
                else
                {
                    return false;
                }
            }
        }

        int currentRoomCount = generatedRooms.Count;
        bool hasCorrectTotalRooms = currentRoomCount == totalRooms;


        bool success = validCount == 3 == hasCorrectTotalRooms;
        return success;
    }

    int CountAdjacentRooms(Vector2Int pos)
    {
        int count = 0;
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2Int checkPos = pos + dirs[i];
            if (generatedRooms.ContainsKey(checkPos))
            {
                count++;
            }
        }

        return count;
    }


    // 남은 공간에 일반방 추가로 채우기
    void FillRemainingSpaces()
    {
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

    // 보스방 2칸 떨어져서 방만드는 필터링
    List<Vector2Int> FilterBossRoomPositions(List<Vector2Int> positions)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int i = 0; i < positions.Count; i++)
        {
            Vector2Int pos = positions[i];
            if (Vector2Int.Distance(pos, startPosition) >= 2)
            {
                validPositions.Add(pos);
            }
        }

        return validPositions;
    }

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
        {
            return;
        }

        GameObject existingPlayer = GameObject.FindWithTag("Player");

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
                                 new Vector3(playerSpawnOffset.x - prefabSize.x * 0.5f,
                                     playerSpawnOffset.y - prefabSize.y * 0.5f, 0);

        if (existingPlayer != null)
        {
            existingPlayer.transform.position = playerSpawnPos;
            spawnedPlayer = existingPlayer; // 참조 업데이트
        }
        else
        {
            spawnedPlayer = Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
            spawnedPlayer.name = "Player";
            spawnedPlayer.tag = "Player";
        }
    }

    void SetupSystems()
    {
        if (cameraSystem != null)
        {
            cameraSystem.SetupForRoom(startPosition);
        }

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

    // 막다른길 위치들 찾기
    List<Vector2Int> FindDeadEndPositions()
    {
        List<Vector2Int> deadEndPositions = new List<Vector2Int>();

        for (int i = 0; i < availablePositions.Count; i++)
        {
            Vector2Int pos = availablePositions[i];
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

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int dir = directions[i];
            Vector2Int checkPos = pos + dir;
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

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int dir = directions[i];
            Vector2Int newPos = centerPos + dir;

            if (IsInMapBounds(newPos) &&
                !generatedRooms.ContainsKey(newPos) &&
                !availablePositions.Contains(newPos))
            {
                availablePositions.Add(newPos);
            }
        }
    }

    bool IsValidPosition(Vector2Int pos)
    {
        return IsInMapBounds(pos) && !generatedRooms.ContainsKey(pos);
    }

    bool IsInMapBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < mapSize && pos.y >= 0 && pos.y < mapSize;
    }

    GameObject GetRandomBossRoomPrefab()
    {
        if (bossRoomPrefabs == null || bossRoomPrefabs.Length == 0)
        {
            return null;
        }

        return bossRoomPrefabs[Random.Range(0, bossRoomPrefabs.Length)];
    }

    // 비밀방 생성 (4면 막힌 곳 우선, 그 다음 3면, 2면 순으로)
    void GenerateSecretRoom()
    {
        if (secretRoomPrefab == null)
        {
            return;
        }

        List<Vector2Int> perfectCandidates = new List<Vector2Int>();

        // 1차: 4면이 막힌 곳 찾기
        perfectCandidates = FindPositionsWithSurroundingCount(4);

        if (perfectCandidates.Count == 0)
        {
            perfectCandidates = FindPositionsWithSurroundingCount(3);
        }

        if (perfectCandidates.Count == 0)
        {
            perfectCandidates = FindPositionsWithSurroundingCount(2, true);
        }

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

        for (int i = 0; i < availablePositions.Count; i++)
        {
            Vector2Int pos = availablePositions[i];
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

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int dir = directions[i];
            if (generatedRooms.ContainsKey(pos + dir))
            {
                count++;
            }
        }

        return count;
    }

    // 모든 방 프리팹을 실제로 생성
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
            }
        }
    }

    // 기존 맵 정리
    void ClearMap()
    {
        // 기존 방들 제거
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
        }

        // 데이터 초기화
        generatedRooms.Clear();
        availablePositions.Clear();
        roomDoors.Clear();
        failsafe = 0;
    }

    [ContextMenu("맵 재생성")]
    public void RegenerateMap()
    {
        currentAttempts = 0;
        GenerateMap();
    }


    void GenerateDoorsAndWalls()
    {
        foreach (var roomPair in generatedRooms)
        {
            Vector2Int roomPos = roomPair.Key;
            RoomData roomData = roomPair.Value;

            if (roomData.instantiatedRoom == null) continue;

            // 해당 방의 문들을 저장할 딕셔너리 초기화
            roomDoors[roomPos] = new Dictionary<Direction, GameObject>();

            // 4방향 확인해서 문/벽 생성
            CheckAndCreateDoor(roomPos, Direction.Up, Vector2Int.up);
            CheckAndCreateDoor(roomPos, Direction.Down, Vector2Int.down);
            CheckAndCreateDoor(roomPos, Direction.Left, Vector2Int.left);
            CheckAndCreateDoor(roomPos, Direction.Right, Vector2Int.right);
        }
    }

    void CheckAndCreateDoor(Vector2Int roomPos, Direction direction, Vector2Int directionVector)
    {
        Vector2Int adjacentRoomPos = roomPos + directionVector;
        Vector3 roomWorldPos = generatedRooms[roomPos].instantiatedRoom.transform.position;
        Vector3 doorWorldPos = GetDoorWorldPosition(roomWorldPos, direction);

        Sprite doorSprite;

        // 인접한 방이 있는지 확인
        if (generatedRooms.ContainsKey(adjacentRoomPos))
        {
            // 인접 방이 있으면 방향별 닫힌 문
            doorSprite = GetClosedDoorSprite(direction);
        }
        else
        {
            // 인접 방이 없으면 방향별 벽
            doorSprite = GetWallSprite(direction);
        }

        if (doorSprite != null)
        {
            GameObject door = new GameObject($"Door_{direction}_{roomPos.x}_{roomPos.y}");
            door.transform.position = doorWorldPos;
            door.transform.parent = generatedRooms[roomPos].instantiatedRoom.transform;


            SpriteRenderer spriteRenderer = door.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = doorSprite;

            // 정렬 순서 설정
            spriteRenderer.sortingOrder = 1;

            roomDoors[roomPos][direction] = door;
        }
    }


    Sprite GetWallSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return wallUpPrefab;
            case Direction.Down: return wallDownPrefab;
            case Direction.Left: return wallLeftPrefab;
            case Direction.Right: return wallRightPrefab;
            default: return null;
        }
    }


    Sprite GetClosedDoorSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return doorClosedUpPrefab;
            case Direction.Down: return doorClosedDownPrefab;
            case Direction.Left: return doorClosedLeftPrefab;
            case Direction.Right: return doorClosedRightPrefab;
            default: return null;
        }
    }

    Sprite GetOpenDoorSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return doorOpenUpPrefab;
            case Direction.Down: return doorOpenDownPrefab;
            case Direction.Left: return doorOpenLeftPrefab;
            case Direction.Right: return doorOpenRightPrefab;
            default: return null;
        }
    }

    Vector3 GetDoorWorldPosition(Vector3 roomWorldPos, Direction direction)
    {
        Vector3 doorLocalPos = Vector3.zero;

        switch (direction)
        {
            case Direction.Up:
                doorLocalPos = new Vector3(doorUpPos.x, doorUpPos.y, 0);
                break;
            case Direction.Down:
                doorLocalPos = new Vector3(doorDownPos.x, doorDownPos.y, 0);
                break;
            case Direction.Left:
                doorLocalPos = new Vector3(doorLeftPos.x, doorLeftPos.y, 0);
                break;
            case Direction.Right:
                doorLocalPos = new Vector3(doorRightPos.x, doorRightPos.y, 0);
                break;
        }

        return roomWorldPos + doorLocalPos;
    }
}