using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRoomMovement : MonoBehaviour
{
    [Header("문 위치 설정")] public Vector2 topDoorPos = new Vector2(7, 8); 
    public Vector2 bottomDoorPos = new Vector2(7, 1); 
    public Vector2 leftDoorPos = new Vector2(1, 4); 
    public Vector2 rightDoorPos = new Vector2(14, 5); 
    public float doorDetectionDistance = 1.0f; // 문 감지 거리

    [Header("이동 설정")] public float transitionSpeed = 2f;
    public bool doorActive = true;

    // 시스템들 참조
    private MapGenerator mapGen;
    private CameraController camSystem;
    private MinimapManager minimap;
    private RoomMonsterManager roomMonster;

    // 현재 상태
    private Vector2Int currentRoom;
    private bool moving = false;
    private bool isInitialized = false;
    private bool hasMapGenerator = false;

    // 유틸리티
    public bool disableRoomTransition = false;
    public bool autoDetectMapGenerator = true;


    private void Start()
    {
        InitialzedComponents();
    }

    void InitialzedComponents()
    {
        FindRequiredComponents();
        InitializedWithMapGenerator();
        RealSetFinal();
        
    }
    
    void FindRequiredComponents()
    {
        if (autoDetectMapGenerator)
        {
            mapGen = FindObjectOfType<MapGenerator>();
            camSystem = FindObjectOfType<CameraController>();
            minimap = FindObjectOfType<MinimapManager>();
            roomMonster = FindObjectOfType<RoomMonsterManager>();
        }
    }


    void InitializedWithMapGenerator()
    {
        if (mapGen != null)
        {
            hasMapGenerator = true;

            // 처음부터 초기화 ( 현재 노드에 따른 시작 포인트도 따라오는거면 offset 도 줘야하나 ? ) 

            currentRoom = mapGen.startPosition;
        }
        else
        {
            hasMapGenerator = false;

            // 방 전환 비활성화
            disableRoomTransition = true;
        }
    }

    void RealSetFinal()
    {
        isInitialized = true;

        if (!hasMapGenerator)
        {
            enabled = false;
        }
        
    }
    
    // required 넣고
    // 맵 초기화까지 넣어버리면 이게 
    // 아 방전환 기능자체를 비활성화 해서 하면 되긴해 , 그러면 여기에서 그걸 잡아도 되는걸까 ? 
    // 아 맞네 해야겠네 
    // null 하는게 좋겠지 ? 아 근데 굳이 할필요  없긴한것같아 신원아

    private void Update()
    {
        if (!isInitialized || moving) return;
        if (doorActive)
        {
            CheckDoors();
        }
        else
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
            //
            //  월드 좌표 그리드 좌표로 변환해서 그리드 가져와야하지않나 ?
            // 그러면 현재 위치 ( detected 가져와서 그리드 좌표로 변환하고
            // 감지된 방이 현재하고 다른지 체크하고 ? 
            // 그방이 진짜로 있는지 확인하고
            // 이동방향 계산치고 방 전환 하려면 아까 Start코루틴 가져오면 된다 신원아 
            // 아니 다해줬잖아 . 
            // 왜 안되는데
            // 왜 감지가 안되는데
            // Detectedroom 왜 안되는데 
        }
    }

    Vector2Int GetGridPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / mapGen.prefabSize.x);
        int y = Mathf.RoundToInt(worldPos.y / mapGen.prefabSize.y);
        return new Vector2Int(x, y);
    }
    

    void CheckDoors()
    {
        if (!mapGen.generatedRooms.ContainsKey(currentRoom)) return;
        
        var roomData = mapGen.generatedRooms[currentRoom];

        if (roomData.instantiatedRoom == null) return;
        
        Vector3 roomPos = roomData.instantiatedRoom.transform.position;
        Vector2 playerLocalPos = GetLocalPos(transform.position, roomPos);
        
        CheckSingleDoor(playerLocalPos,topDoorPos,Vector2Int.up);
        CheckSingleDoor(playerLocalPos,bottomDoorPos,Vector2Int.down);
        CheckSingleDoor(playerLocalPos,leftDoorPos,Vector2Int.left);
        CheckSingleDoor(playerLocalPos,rightDoorPos,Vector2Int.right);
    }

    Vector2 GetLocalPos(Vector3 worldPos, Vector3 roomWorldPos)
    {
        // 월드좌표 에서 룸 
        Vector3 localPos = worldPos - roomWorldPos;
        return new Vector2(localPos.x, localPos.y);
    }

    void CheckSingleDoor(Vector2 playerPos, Vector2 doorPos, Vector2Int direction)
    {
        float distance = Vector2.Distance(playerPos, doorPos);
        
        
        if (distance <= doorDetectionDistance)
        {
            Vector2Int targetRoom = currentRoom + direction;
            
            Debug.Log($"문 감지됨 , 현재방 {currentRoom}, 목표방 {targetRoom}");
            if (mapGen.generatedRooms.ContainsKey(targetRoom))
            {
                Debug.Log($"목표방 이동가능");
                StartTransition(targetRoom, direction);
            }
        }
    }

    void StartTransition(Vector2Int targetRoom, Vector2Int direction)
    {
        if (moving) return;

        if (!MayIEnterThisRoom(targetRoom)) return;
        
        StartCoroutine(Dotransition(targetRoom, direction));
    }

    IEnumerator Dotransition(Vector2Int targetRoom, Vector2Int direction)
    {
        moving = true;
        
        // 생각해보자 문이 열려있으면 일드 리턴을 또 player자체 pos 를 또 옮겨야하잖아 그 애니메이션이 
        if (doorActive)
        {
            Vector3 newPos = GetNewPlayerPos(targetRoom, direction);
            yield return StartCoroutine(MovePlayerToPos(newPos));
            // 여기에다가 는 또 player를 포지션까지 넘겨주는 코루틴 까지 해야되넹 애니메이션 주려면 ? 
        }

        if (camSystem != null)
        {
            camSystem.TransitionToRoom(targetRoom);

            while (camSystem.IsTransitioning())
            {
                yield return null;
            }
        }
        // 움직였으니깐 카메라도 넘어가고 
        // 미니맵도 넘어가는거 알려줘야함 . 
        // 잠시만 카메라 움직이고 ? 포지션 업데이트 했고 ? 현재 방 업데이트 했잖아 그러면 
        // 미니맵 업데이트 
        currentRoom = targetRoom;

        if (minimap != null)
        {
            minimap.SetCurrentRoom(currentRoom);
        }
        
        moving = false;
    }

    Vector3 GetNewPlayerPos(Vector2Int targetRoom, Vector2Int direction)
    {
        var roomData = mapGen.generatedRooms[targetRoom];
        Vector3 roomPos = roomData.instantiatedRoom.transform.position;

        Vector2 targetDoor = Vector2.zero;

        if (direction == Vector2Int.up) targetDoor = bottomDoorPos;
        else if (direction == Vector2Int.down) targetDoor = topDoorPos;
        else if (direction == Vector2Int.left) targetDoor = rightDoorPos;
        else if (direction == Vector2Int.right) targetDoor = leftDoorPos;
        // roompos 에서 잠시만 잠시만 잠시만 잠시만 roompos 에서 ? ? ? ? ? ?\
        // 음 음 음 음 음 으므므므므므므므므므므 아 근데 벡터3가 무조건 맞는데 이거 뭐지 뭐지 뭐지 qweqwewqewqeqweqwewqeqweqweqweqweqwe qwewqeqwe qwe qwe wqesafaxvadfasd faqwewqasfa qw eqwe as
        // 엥 ???? 아니 근데 이거 
        return roomPos + new Vector3(targetDoor.x, targetDoor.y,0);
    }

    IEnumerator MovePlayerToPos(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;
        float duration = 1f / transitionSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0,1,elapsed/duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        
        transform.position = targetPos;
    }

    bool MayIEnterThisRoom(Vector2Int targetRoom)
    {
        if (!mapGen.generatedRooms.ContainsKey(currentRoom)) return false;
        
        var roomData = mapGen.generatedRooms[currentRoom];

        if (roomData.roomType == MapGenerator.RoomType.Start)
        {
            if (IsSecretWallBlocking(currentRoom, targetRoom))
            {
                return false;
            }
            return true;
        }
       
        // mapGen 은 mapgenerator 의 객체
        // 하지만 MapGenerator 자체는 클래스니깐 
        // 객체하고 클래스맴버는 다르니깐 
        // 객체 enum 은 당연히 못쓰잖아. 
        // 그니깐 ? 클래스에서 직접 참조해서 enum 을 쓴다. 라고 말할수는 있을거야 하지만 ? 
        // 이해를 못함.
        // 약간 ㅌ뭔말알 ? 층류현상 ? 
        // 이게 뭐냐면 기름이 일정한 속력과 일정한 양이 뿜으면 멈춰져있는것처럼 보이는 현상을 층류 현상
        // 실제로 왜 이렇게 일어나는가에 대한거는 모르겠다. 
        //return roomMonster.IsRoomClear(currentRoom);
        bool isCleared = roomMonster.IsRoomClear(currentRoom);
        

        return isCleared;
    }
    
    bool IsSecretWallBlocking(Vector2Int fromRoom, Vector2Int toRoom)
    {
        // 목표방이 비밀방인지 확인함.
        if (!mapGen.generatedRooms.ContainsKey(toRoom) || mapGen.generatedRooms[toRoom].roomType 
            != MapGenerator.RoomType.Secret) return false;  
        
        Vector2Int secretDir = toRoom - fromRoom;

        MapGenerator.Direction wallDir;

        switch (secretDir.x,secretDir.y)
        {
            case (0,1): wallDir = MapGenerator.Direction.Up; break;
            case (0,-1): wallDir = MapGenerator.Direction.Down; break;
            case (-1,0): wallDir = MapGenerator.Direction.Left; break;
            case (1,0): wallDir = MapGenerator.Direction.Right; break;
            default: return false;
        }
        
        return mapGen.roomDoors.ContainsKey(fromRoom) &&
               mapGen.roomDoors[fromRoom].ContainsKey(wallDir)&&
               mapGen.roomDoors[fromRoom][wallDir]?.name.Contains("SecretWall")==true;
    }
    
    // 퍼블릭 메서드들 여기에서 가져갈것이..

    public Vector2Int GetCurrentRoom()
    {
        return currentRoom;
    }
    
    // 방이동중 플레이어 조작 막기 , 인벤토리 같은거 못열게 불러도 됨 
    public bool IsMoving()
    {
        return moving;
    }

    public void SetRoomTransitionEnabled(bool enable)
    {
        disableRoomTransition = !enable;
    }

    public void PrepareForScene()
    {
        StopAllCoroutines();
        moving = false;
    }

    public void PlayerRoomReinitialize()
    {
        isInitialized = false;
        hasMapGenerator = false;
        moving = false;
        disableRoomTransition = false;
        
        InitialzedComponents();
    }
}