using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float transitionSpeed = 2f;
    public bool constrainToRoom = true;
    public float roomPadding = 0.5f;
    
    private Camera cam;
    private MapGenerator mapGenerator;
    private Vector2Int currentRoom;
    private bool isTransitioning = false;
    private Bounds currentRoomBounds;

    void Start()
    {
        cam = GetComponent<Camera>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        
        if (mapGenerator != null)
        {
            currentRoom = mapGenerator.startPosition;

            StartCoroutine(MoveToStartRoomOnStart());
        }
    }

    IEnumerator MoveToStartRoomOnStart()
    {
        yield return new WaitForEndOfFrame();
        
        InstantMoveToRoom(mapGenerator.startPosition);
    }

    void LateUpdate()
    {
        if (constrainToRoom)
        {
            ConstrainCameraToRoom();
        }
    }

    public void SetupForRoom(Vector2Int roomPos)
    {
        currentRoom = roomPos;
        SetRoomBounds(roomPos);
        MoveToRoomInstant(roomPos);
    }

    public void TransitionToRoom(Vector2Int roomPos)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionCoroutine(roomPos));
    }

    public void InstantMoveToRoom(Vector2Int roomPos)
    {
        currentRoom = roomPos;
        SetRoomBounds(roomPos);
        MoveToRoomInstant(roomPos);
    }

    IEnumerator TransitionCoroutine(Vector2Int targetRoom)
    {
        isTransitioning = true;
        
        Vector3 targetPos = CalculateRoomCameraPosition(targetRoom);
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
        currentRoom = targetRoom;
        SetRoomBounds(targetRoom);
        
        isTransitioning = false;
    }

    void MoveToRoomInstant(Vector2Int roomPos)
    {
        Vector3 targetPos = CalculateRoomCameraPosition(roomPos);
        transform.position = targetPos;
    }

    Vector3 CalculateRoomCameraPosition(Vector2Int roomPos)
    {
        if (mapGenerator == null || !mapGenerator.generatedRooms.ContainsKey(roomPos))
            return transform.position;

        var roomData = mapGenerator.generatedRooms[roomPos];
        if (roomData.instantiatedRoom == null)
            return transform.position;

        Vector3 roomCenter = roomData.instantiatedRoom.transform.position;
        roomCenter += new Vector3(mapGenerator.prefabSize.x * 0.5f, mapGenerator.prefabSize.y * 0.5f, 0);
        
        return new Vector3(roomCenter.x, roomCenter.y, transform.position.z);
    }

    void SetRoomBounds(Vector2Int roomPos)
    {
        if (mapGenerator == null || !mapGenerator.generatedRooms.ContainsKey(roomPos))
            return;

        var roomData = mapGenerator.generatedRooms[roomPos];
        if (roomData.instantiatedRoom == null)
            return;

        Vector3 roomPosition = roomData.instantiatedRoom.transform.position;
        Vector3 roomSize = new Vector3(mapGenerator.prefabSize.x, mapGenerator.prefabSize.y, 0);
        Vector3 roomCenter = roomPosition + roomSize * 0.5f;
        
        currentRoomBounds = new Bounds(roomCenter, roomSize);
    }

    void ConstrainCameraToRoom()
    {
        if (currentRoomBounds.size == Vector3.zero) return;

        float cameraHeight = cam.orthographicSize * 2f;
        float cameraWidth = cameraHeight * cam.aspect;
        
        float minX = currentRoomBounds.min.x + cameraWidth * 0.5f + roomPadding;
        float maxX = currentRoomBounds.max.x - cameraWidth * 0.5f - roomPadding;
        float minY = currentRoomBounds.min.y + cameraHeight * 0.5f + roomPadding;
        float maxY = currentRoomBounds.max.y - cameraHeight * 0.5f - roomPadding;
        
        if (minX >= maxX) minX = maxX = currentRoomBounds.center.x;
        if (minY >= maxY) minY = maxY = currentRoomBounds.center.y;
        
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    public bool IsTransitioning() => isTransitioning;
    public Vector2Int GetCurrentRoom() => currentRoom;
}