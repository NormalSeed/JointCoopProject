using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MapGenerator_Level
{
    public static bool AlreadyStarted = false;

    public static float height = 100;
    public static float width = 100;

    public static float Scale = 1f;
    public static float IconScale = .1f;
    public static float padding = .01f;

    public static float RoomGenerationChance = .5f;

    public static int RoomLimit = 4;

    public static Sprite TreasureRoomIcon;
    public static Sprite BossRoomIcon;
    public static Sprite ShopRoomIcon;
    public static Sprite UnexploredRoomIcon;
    public static Sprite DefaultRoomIcon;
    public static Sprite CurrentRoomIcon;
    public static Sprite SecretRoom;

    public static GameObject DevilRoom;
    public static GameObject AngelRoom;

    public static GameObject SecretRoomExplosion;
    public static GameObject SecretRoomDoor;
    public static bool SecretRoomExploded = false;
    public static GameObject XMark;

    public static List<Room> Rooms = new List<Room>();

    public static float RoomChangeTime = .1f;

    public static int EnemyCount = 0;

    public static bool ItemRoomUnlocked = false;
    public static bool ShopRoomUnlocked = false;

    public static void LoadNextLevel()
    {
        EnemyCount = 0;
        Rooms.Clear();


        //ImpactReceiver.forcesOnGameObjects.Clear();
        //SceneManager.sceneLoaded += InitalizePlayer.OnSceneLoaded;
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(currentSceneIndex + 1);

    }

}




public class MapGenerator_Room
{
    public int RoomNumber = 6;
    public Vector2 Location;
    public Image RoomImage;
    public Sprite RoomSprite;
    public bool Revealed = false;
    public bool Explored = false;
    public bool Cleared = true;
    public string RoomType = "Rooms";
    public bool AnchorRoom = true;
    public Room AnchorR;
    public bool TopRight = false;
    public bool SpecialRoom = false;
}