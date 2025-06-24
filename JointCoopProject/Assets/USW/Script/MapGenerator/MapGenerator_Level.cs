using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator_Level
{
    public static float height = 100;
    public static float width = 100;

    public static float Scale = 1.0f;
    public static float IconScale = 0.1f;
    public static float padding = 0.01f;
    
    public static float RoomGenerationChance = 0.5f;

    public static Sprite TreasureRoomIcon;
    public static Sprite BossRoomIcon;
    public static Sprite ShopRoomIcon;
    public static Sprite DefaultRoomIcon;
    public static Sprite UnexploredRoomIcon;
    public static Sprite CurrentRoomIcon;
}


public class MapGenerator_Room
{
    public int RoomNumber = 0;
    public Vector2 Location;
    //public sprite RoomIcon;
}