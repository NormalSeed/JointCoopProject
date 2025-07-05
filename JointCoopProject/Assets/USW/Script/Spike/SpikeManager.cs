using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    private Dictionary<Vector2Int, List<GameObject>> roomSpikes = new Dictionary<Vector2Int, List<GameObject>>();
    private MapGenerator mapGen;
    private PlayerRoomMovement playerMove;
    private Vector2Int currentRoom = new Vector2Int(-1, -1);

    private void Start()
    {
        mapGen = FindObjectOfType<MapGenerator>();
        playerMove = FindObjectOfType<PlayerRoomMovement>();
        StartCoroutine(SetupSpikes());
        
    }

    private void Update()
    {
        if (playerMove == null)
        {
            playerMove = FindObjectOfType<PlayerRoomMovement>();
            if (playerMove == null) return; // 여전히 못 찾으면 그냥 리턴
        }
        
        Vector2Int playerRoom = playerMove.GetCurrentRoom();

        if (playerRoom != currentRoom)
        {
            Debug.Log("player!=currenroom");
            TurnOffSpikesInRoom(currentRoom);
            TurnOnSpikesInRoom(playerRoom);
            currentRoom = playerRoom;
            Debug.Log("다시 갱신");
        }
    }

    IEnumerator SetupSpikes()
    {
        yield return new WaitForEndOfFrame();
        
        FindAllRoomSpikes();
        TurnOffAllRoomSpikes();

        if (mapGen != null)
        {
            TurnOnSpikesInRoom(mapGen.startPosition);
            currentRoom = mapGen.startPosition;
        }
        
    }

    void FindAllRoomSpikes()
    {
        SpikeDamage[] spikes = FindObjectsOfType<SpikeDamage>(true);
        foreach (var spike in spikes)
        {
           Vector2Int roomPos = GetRoomPosition(spike.transform.position);
           if(!roomSpikes.ContainsKey(roomPos))
               roomSpikes[roomPos] = new List<GameObject>();
           roomSpikes[roomPos].Add(spike.gameObject);
        }
    }

    Vector2Int GetRoomPosition(Vector3 pos)
    {
        if (mapGen == null) return Vector2Int.zero;
        int x = Mathf.FloorToInt(pos.x / mapGen.prefabSize.x);
        int y = Mathf.FloorToInt(pos.y / mapGen.prefabSize.y);
        return new Vector2Int(x, y);
    }

    void TurnOffAllRoomSpikes()
    {
        foreach (var spikes in roomSpikes.Values)
        {
            foreach (var spike in spikes)
            {
                if (spike != null)
                    spike.SetActive(false);
            }
        }
    }

    void TurnOnSpikesInRoom(Vector2Int roomPos)
    {
        if (roomSpikes.ContainsKey(roomPos))
        {
            foreach (var spike in roomSpikes[roomPos])
            {
                if(spike != null)
                    spike.SetActive(true);
            }
        }
    }

    void TurnOffSpikesInRoom(Vector2Int roomPos)
    {
        if (roomSpikes.ContainsKey(roomPos))
        {
            foreach (var spike in roomSpikes[roomPos])
            {
                if(spike!=null)
                    spike.SetActive(false);
            }
        }
    }
}