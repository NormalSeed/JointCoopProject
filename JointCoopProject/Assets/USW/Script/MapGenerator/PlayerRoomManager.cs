
using System.Collections;
using UnityEngine;

public class PlayerRoomMovement : MonoBehaviour
{
    [Header("Door Detection")]
    public Vector2 topDoorPos = new Vector2(7, 8);
    public Vector2 bottomDoorPos = new Vector2(7, 1);
    public Vector2 leftDoorPos = new Vector2(1, 4);
    public Vector2 rightDoorPos = new Vector2(14, 5);
    public float doorDetectionDistance = 1.5f;
    
    [Header("Movement Settings")]
    public float transitionSpeed = 2f;
    public bool useIsaacStyle = true; // true: 문 감지, false: 자동 감지
    
    // 시스템 참조
    private MapGenerator mapGenerator;
    private CameraController cameraSystem;
    private MinimapManager minimapManager;
    
    // 현재 상태
    private Vector2Int currentRoom;
    private bool isTransitioning = false;
    
    void Start()
    {
        // 자동으로 시스템 찾기
        mapGenerator = FindObjectOfType<MapGenerator>();
        cameraSystem = FindObjectOfType<CameraController>();
        minimapManager = FindObjectOfType<MinimapManager>();
        
        if (mapGenerator != null)
        {
            currentRoom = mapGenerator.startPosition;
        }
    }
    
    void Update()
    {
        if (mapGenerator == null || isTransitioning) return;
        
        if (useIsaacStyle)
        {
            CheckDoorProximity();
        }
        else
        {
            CheckRoomChange(); // 자동 감지
        }
    }
    
    void CheckDoorProximity()
    {
        if (!mapGenerator.generatedRooms.ContainsKey(currentRoom)) return;
        
        var roomData = mapGenerator.generatedRooms[currentRoom];
        if (roomData.instantiatedRoom == null) return;
        
        Vector3 roomWorldPos = roomData.instantiatedRoom.transform.position;
        Vector2 playerLocalPos = WorldToRoomLocal(transform.position, roomWorldPos);
        
        // 각 문 체크
        CheckDoor(playerLocalPos, topDoorPos, Vector2Int.up, "위");
        CheckDoor(playerLocalPos, bottomDoorPos, Vector2Int.down, "아래");
        CheckDoor(playerLocalPos, leftDoorPos, Vector2Int.left, "왼쪽");
        CheckDoor(playerLocalPos, rightDoorPos, Vector2Int.right, "오른쪽");
    }
    
    Vector2 WorldToRoomLocal(Vector3 worldPos, Vector3 roomWorldPos)
    {
        Vector3 localPos = worldPos - roomWorldPos;
        return new Vector2(localPos.x, localPos.y);
    }
    
    void CheckDoor(Vector2 playerPos, Vector2 doorPos, Vector2Int direction, string doorName)
    {
        float distance = Vector2.Distance(playerPos, doorPos);
        
        if (distance <= doorDetectionDistance)
        {
            Vector2Int targetRoom = currentRoom + direction;
            
            if (mapGenerator.generatedRooms.ContainsKey(targetRoom))
            {
                Debug.Log($"{doorName}쪽 문에서 방 이동: {currentRoom} → {targetRoom}");
                StartRoomTransition(targetRoom, direction);
            }
        }
    }
    

    
    void CheckRoomChange()
    {
        Vector2Int detectedRoom = WorldPosToGridPos(transform.position);
        
        if (detectedRoom != currentRoom)
        {
            if (mapGenerator.generatedRooms.ContainsKey(detectedRoom))
            {
                Vector2Int direction = detectedRoom - currentRoom;
                StartRoomTransition(detectedRoom, direction);
            }
        }
    }
    
    Vector2Int WorldPosToGridPos(Vector3 worldPos)
    {
        int gridX = Mathf.RoundToInt(worldPos.x / mapGenerator.prefabSize.x);
        int gridY = Mathf.RoundToInt(worldPos.y / mapGenerator.prefabSize.y);
        return new Vector2Int(gridX, gridY);
    }
    

    
    void StartRoomTransition(Vector2Int targetRoom, Vector2Int direction)
    {
        if (isTransitioning) return;
        
        // 방 클리어 체크 (나중에 구현)
        if (!IsCurrentRoomCleared())
        {
            return;
        }
        
        StartCoroutine(RoomTransitionCoroutine(targetRoom, direction));
    }
    
    IEnumerator RoomTransitionCoroutine(Vector2Int targetRoom, Vector2Int direction)
    {
        isTransitioning = true;
        
        Debug.Log($"방 전환 시작: {currentRoom} → {targetRoom}");
        
        // 플레이어 이동
        if (useIsaacStyle)
        {
            Vector3 newPlayerPos = CalculateNewPlayerPosition(targetRoom, direction);
            yield return StartCoroutine(MovePlayerSmooth(newPlayerPos));
        }
        
        // 카메라 전환
        if (cameraSystem != null)
        {
            cameraSystem.TransitionToRoom(targetRoom);
            
            // 카메라 전환 완료까지 대기
            while (cameraSystem.IsTransitioning())
            {
                yield return null;
            }
        }
        
        // 현재 방 업데이트
        currentRoom = targetRoom;
        
        // 미니맵 업데이트
        if (minimapManager != null)
        {
            minimapManager.SetCurrentRoom(currentRoom);
        }
        
        // 5. 방 입장 이벤트
        OnEnterRoom(targetRoom);
        
        isTransitioning = false;
    }
    
    Vector3 CalculateNewPlayerPosition(Vector2Int targetRoom, Vector2Int direction)
    {
        var targetRoomData = mapGenerator.generatedRooms[targetRoom];
        Vector3 targetRoomPos = targetRoomData.instantiatedRoom.transform.position;
        
        // 반대편 문으로 이동
        Vector2 targetDoorLocal = Vector2.zero;
        
        if (direction == Vector2Int.up) targetDoorLocal = bottomDoorPos;       // 위로 갔으면 아래 문으로
        else if (direction == Vector2Int.down) targetDoorLocal = topDoorPos;   // 아래로 갔으면 위 문으로
        else if (direction == Vector2Int.left) targetDoorLocal = rightDoorPos; // 왼쪽으로 갔으면 오른쪽 문으로
        else if (direction == Vector2Int.right) targetDoorLocal = leftDoorPos; // 오른쪽으로 갔으면 왼쪽 문으로
        
        return targetRoomPos + new Vector3(targetDoorLocal.x, targetDoorLocal.y, 0);
    }
    
    IEnumerator MovePlayerSmooth(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;
        float duration = 1f / transitionSpeed;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        
        transform.position = targetPos;
    }
 
    
    void OnEnterRoom(Vector2Int roomPos)
    {
        if (!mapGenerator.generatedRooms.ContainsKey(roomPos)) return;
        
        var roomData = mapGenerator.generatedRooms[roomPos];
        Debug.Log($"방 입장: {roomData.roomType} ({roomPos})");
        
        // 방 타입별 특수 처리
        switch (roomData.roomType)
        {
            case MapGenerator.RoomType.Boss:
                // TODO: 보스 스폰, BGM 변경 등
                break;
                
            case MapGenerator.RoomType.Shop:
                // TODO: 상점 NPC, UI 활성화
                break;
                
            case MapGenerator.RoomType.Item:
                // TODO: 아이템 생성
                break;
                
            case MapGenerator.RoomType.Secret:
                // TODO: 특별 보상
                break;
        }
    }
    
    bool IsCurrentRoomCleared()
    {
        // TODO: 방 클리어 조건 체크
        // 지금은 시작방만 항상 클리어된 것으로 처리
        if (mapGenerator.generatedRooms.ContainsKey(currentRoom))
        {
            var roomData = mapGenerator.generatedRooms[currentRoom];
            if (roomData.roomType == MapGenerator.RoomType.Start)
                return true;
        }
        
        // 임시로 모든 방이 클리어된 것으로 처리
        return true;
    }
    
   
    
    
    public Vector2Int GetCurrentRoom() => currentRoom;
    public bool IsTransitioning() => isTransitioning;
    
    
}