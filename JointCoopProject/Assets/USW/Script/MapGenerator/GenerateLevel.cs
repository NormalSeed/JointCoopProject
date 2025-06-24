using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateLevel : MonoBehaviour
{
    public Sprite EmptyRoom;
    public Sprite BossRoom;
    public Sprite ShopRoom;
    public Sprite UnexploredRoom;
    public Sprite CurrentRoom;
    public Sprite TreasureRoom;


    void DrawRoomOnMap(Sprite s,Vector2 Location)
    {
        GameObject MapTile = new GameObject("MapTile");
        Image roomImage = MapTile.AddComponent<Image>();
        roomImage.sprite = s;
        RectTransform rectTransform = roomImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(MapGenerator_Level.height, MapGenerator_Level.width) * MapGenerator_Level.IconScale;
        
        // 아이콘 스케일 0.1 , height = 500 , scale = 1 
        rectTransform.position = Location * (MapGenerator_Level.IconScale * MapGenerator_Level.height * MapGenerator_Level.Scale+(MapGenerator_Level.padding * MapGenerator_Level.Scale * MapGenerator_Level.height));
        roomImage.transform.SetParent(transform, false);
    }

    void Generate(Vector2 Location)
    {
        //왼쪽
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, Location + new Vector2(-1,0));
            Generate(Location + new Vector2(-1,0));
        }
        //오른쪽
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, Location + new Vector2(1,0));
            Generate(Location + new Vector2(1,0));
        }
        //위
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, Location + new Vector2(0,1));
            Generate(Location + new Vector2(0,1));
        }
        //아래
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, Location + new Vector2(0,-1));
            Generate(Location + new Vector2(0,-1));
        }
    }
    private void Start()
    {
        MapGenerator_Level.DefaultRoomIcon = EmptyRoom;
        MapGenerator_Level.BossRoomIcon = BossRoom;
        MapGenerator_Level.ShopRoomIcon = ShopRoom;
        MapGenerator_Level.UnexploredRoomIcon = UnexploredRoom;
        MapGenerator_Level.CurrentRoomIcon = CurrentRoom;
        MapGenerator_Level.TreasureRoomIcon = TreasureRoom;
        
        //시각적
        DrawRoomOnMap(MapGenerator_Level.CurrentRoomIcon, new Vector2(0,0));
        
        //왼쪽
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, new Vector2(-1,0));
            Generate(new Vector2(-1,0));
        }
        //오른쪽
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, new Vector2(1,0));
            Generate(new Vector2(1,0));
        }
        //위
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, new Vector2(0,1));
            Generate(new Vector2(0,1));
        }
        //아래
        if (UnityEngine.Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            DrawRoomOnMap(MapGenerator_Level.UnexploredRoomIcon, new Vector2(0,-1));
            Generate(new Vector2(0,-1));
        }
    }
}
