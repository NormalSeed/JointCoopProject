using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomController : MonoBehaviour
{
    private MapGenerator.RoomData roomData;
    
    public void SetRoomData(MapGenerator.RoomData data)
    {
        roomData = data;
    }
    
    public MapGenerator.RoomData GetRoomData() => roomData;
    public bool IsDeadEnd() => roomData?.isDeadEnd ?? false;
    public MapGenerator.RoomType GetRoomType() => roomData?.roomType ?? MapGenerator.RoomType.Default;
}