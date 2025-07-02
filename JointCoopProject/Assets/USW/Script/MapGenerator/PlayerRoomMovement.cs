using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRoomMovement : MonoBehaviour
{
    [Header("문 위치 설정")]
    public Vector2 topDoorPos = new Vector2(7, 8);         // 위쪽 문 위치
    public Vector2 bottomDoorPos = new Vector2(7, 1);      // 아래쪽 문 위치
    public Vector2 leftDoorPos = new Vector2(1, 4);        // 왼쪽 문 위치
    public Vector2 rightDoorPos = new Vector2(14, 5);      // 오른쪽 문 위치
    public float doorDetectionDistance = 1.5f;             // 문 감지 거리
    
    [Header("이동 설정")]
    public float transitionSpeed = 2f;                     // 방 전환 속도
    public bool doorActive = true;                         // true: 문 감지, false: 자동 감지
    
    // 시스템들 참조
    private MapGenerator mapGen;                           // 맵 생성기
    private CameraController camSystem;                    // 카메라 컨트롤러
    private MinimapManager minimap;                        // 미니맵 매니저
    
    // 현재 상태
    private Vector2Int currentRoom;                        // 현재 방 위치
    private bool moving = false;                           // 이동 중인지 여부
    
    // 디버그용
    public bool debugMode = false;                         // 디버그 모드
    private int transitionCount = 0;                       // 방 이동 횟수
    
    void Start()
    {
        // 필요한 컴포넌트들 찾기
        mapGen = FindObjectOfType<MapGenerator>();
        camSystem = FindObjectOfType<CameraController>();
        minimap = FindObjectOfType<MinimapManager>();
        
        if (mapGen != null)
        {
            currentRoom = mapGen.startPosition;
        }
    }
    
    void Update()
    {
        if (mapGen == null || moving) return;
        
        // 디버그 키 (나중에 지울 예정)
        if (Input.GetKeyDown(KeyCode.F2))
        {
            debugMode = !debugMode;
            Debug.Log("디버그 모드: " + debugMode);
        }
        
        if (doorActive)
        {
            CheckDoors();           // 문 감지 방식
        }
        else
        {
            CheckAutoTransition();  // 자동 감지 방식
        }
    }
    
    void CheckDoors()
    {
        if (!mapGen.generatedRooms.ContainsKey(currentRoom)) return;
        
        var roomData = mapGen.generatedRooms[currentRoom];
        if (roomData.instantiatedRoom == null) return;
        
        Vector3 roomPos = roomData.instantiatedRoom.transform.position;
        Vector2 playerLocalPos = GetLocalPos(transform.position, roomPos);
        
        // 각 문별로 체크
        CheckSingleDoor(playerLocalPos, topDoorPos, Vector2Int.up, "위쪽");
        CheckSingleDoor(playerLocalPos, bottomDoorPos, Vector2Int.down, "아래쪽");
        CheckSingleDoor(playerLocalPos, leftDoorPos, Vector2Int.left, "왼쪽");
        CheckSingleDoor(playerLocalPos, rightDoorPos, Vector2Int.right, "오른쪽");
    }
    
    Vector2 GetLocalPos(Vector3 worldPos, Vector3 roomWorldPos)
    {
        Vector3 localPos = worldPos - roomWorldPos;
        return new Vector2(localPos.x, localPos.y);
    }
    
    void CheckSingleDoor(Vector2 playerPos, Vector2 doorPos, Vector2Int direction, string doorName)
    {
        float distance = Vector2.Distance(playerPos, doorPos);
        
        if (distance <= doorDetectionDistance)
        {
            Vector2Int targetRoom = currentRoom + direction;
            
            if (mapGen.generatedRooms.ContainsKey(targetRoom))
            {
                StartTransition(targetRoom, direction);
            }
        }
    }
    
    void CheckAutoTransition()
    {
        Vector2Int detectedRoom = GetGridPos(transform.position);
        
        if (detectedRoom != currentRoom)
        {
            if (mapGen.generatedRooms.ContainsKey(detectedRoom))
            {
                Vector2Int direction = detectedRoom - currentRoom;
                StartTransition(detectedRoom, direction);
            }
        }
    }
    
    Vector2Int GetGridPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / mapGen.prefabSize.x);
        int y = Mathf.RoundToInt(worldPos.y / mapGen.prefabSize.y);
        return new Vector2Int(x, y);
    }
    
    void StartTransition(Vector2Int targetRoom, Vector2Int direction)
    {
        if (moving) return;
        
        // 방 클리어 체크 (아직 구현 안됨)
        if (!CanEnterRoom())
        {
            return;
        }
        
        StartCoroutine(DoTransition(targetRoom, direction));
    }
    
    IEnumerator DoTransition(Vector2Int targetRoom, Vector2Int direction)
    {
        moving = true;
        transitionCount++;
        
        
        // 플레이어 이동 (문 모드일 때만)
        if (doorActive)
        {
            Vector3 newPos = GetNewPlayerPos(targetRoom, direction);
            yield return StartCoroutine(MovePlayerToPos(newPos));
        }
        
        // 카메라 전환
        if (camSystem != null)
        {
            camSystem.TransitionToRoom(targetRoom);
            
            // 카메라 이동 완료까지 대기
            while (camSystem.IsTransitioning())
            {
                yield return null;
            }
        }
        
        // 현재 방 업데이트
        currentRoom = targetRoom;
        
        // 미니맵 업데이트
        if (minimap != null)
        {
            minimap.SetCurrentRoom(currentRoom);
        }
        
        // 방 입장 처리
        HandleRoomEnter(targetRoom);
        
        moving = false;
    }
    
    Vector3 GetNewPlayerPos(Vector2Int targetRoom, Vector2Int direction)
    {
        var roomData = mapGen.generatedRooms[targetRoom];
        Vector3 roomPos = roomData.instantiatedRoom.transform.position;
        
        // 반대편 문으로 이동
        Vector2 targetDoor = Vector2.zero;
        
        if (direction == Vector2Int.up) targetDoor = bottomDoorPos;      // 위로 갔으면 아래 문으로
        else if (direction == Vector2Int.down) targetDoor = topDoorPos;  // 아래로 갔으면 위 문으로
        else if (direction == Vector2Int.left) targetDoor = rightDoorPos;  // 왼쪽으로 갔으면 오른쪽 문으로
        else if (direction == Vector2Int.right) targetDoor = leftDoorPos;  // 오른쪽으로 갔으면 왼쪽 문으로
        
        return roomPos + new Vector3(targetDoor.x, targetDoor.y, 0);
    }
    
    IEnumerator MovePlayerToPos(Vector3 targetPos)
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
    
    void HandleRoomEnter(Vector2Int roomPos)
    {
        if (!mapGen.generatedRooms.ContainsKey(roomPos)) return;
        
        var roomData = mapGen.generatedRooms[roomPos];
        
        // 방 타입별 처리
        switch (roomData.roomType)
        {
            case MapGenerator.RoomType.Boss:
                // TODO: 보스 스폰
                break;
                
            case MapGenerator.RoomType.Shop:
                // TODO: 상점 UI 표시
                break;
                
            case MapGenerator.RoomType.Item:
                // TODO: 아이템 생성
                break;
                
            case MapGenerator.RoomType.Secret:
                // TODO: 비밀방 로직
                break;
        }
    }
    
    bool CanEnterRoom()
    {
        // TODO: 진짜 방 클리어 조건 구현하기
        if (mapGen.generatedRooms.ContainsKey(currentRoom))
        {
            var roomData = mapGen.generatedRooms[currentRoom];
            if (roomData.roomType == MapGenerator.RoomType.Start)
                return true;
        }
        
        // 지금은 모든 방이 클리어된 것으로 처리
        return true;
    }
    
    // Public 메서드들
    public Vector2Int GetCurrentRoom() 
    { 
        return currentRoom; 
    }
    
    public bool IsMoving() 
    { 
        return moving; 
    }
    
    public int GetTransitionCount() 
    { 
        return transitionCount; 
    }
}