using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionTrigger : MonoBehaviour
{
    [Header("Room Info")]
    public Vector2Int roomGridPosition;
    public bool autoDetectRoomPosition = true;
    
    private CameraController cameraController;
    private MinimapManager minimapManager;
    
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        minimapManager = FindObjectOfType<MinimapManager>();
        
        if (autoDetectRoomPosition)
        {
            DetectRoomPosition();
        }
    }
    
    void DetectRoomPosition()
    {
        TilemapLevelGenerator levelGen = FindObjectOfType<TilemapLevelGenerator>();
        if (levelGen != null)
        {
            // 현재 위치를 기반으로 방 그리드 위치 계산
            Vector3 worldPos = transform.position;
            int gridX = Mathf.RoundToInt(worldPos.x / levelGen.prefabSize.x);
            int gridY = Mathf.RoundToInt(worldPos.y / levelGen.prefabSize.y);
            roomGridPosition = new Vector2Int(gridX, gridY);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cameraController != null)
            {
                cameraController.TransitionToRoom(roomGridPosition);
            }
            
            if (minimapManager != null)
            {
                minimapManager.SetCurrentRoom(roomGridPosition);
            }
            
            Debug.Log($"플레이어가 방 {roomGridPosition}에 입장");
        }
    }
}