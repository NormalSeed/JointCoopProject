using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
public class GenerateLevel : MonoBehaviour
{

    public string StartRoomType = "StartRooms";
    public string StartRoomNumber = "0";

    public Sprite CurrentRoom;
    public Sprite BRoom;
    public Sprite EmptyRoom;
    public Sprite ShopRoom;
    public Sprite TreasureRoom;
    public Sprite UnexploredRoom;
    public Sprite SecretRoom;

    public GameObject DebugCheatMenu;

    private bool BossRoomGenerated = false;


    void DrawRoomOnMap(MapGenerator_Room R)
    {
        if (R.AnchorRoom)
        {
            string TileName = "MapTile";
            if (R.RoomNumber == 1) TileName = "BossRoomTile";
            if (R.RoomNumber == 2) TileName = "ShopRoomTile";
            if (R.RoomNumber == 3) TileName = "ItemRoomTile";
            if (R.RoomType == "2x2Rooms") TileName = "2x2Room";
            GameObject MapTile = new GameObject(TileName);
            Image RoomImage = MapTile.AddComponent<Image>();
            RoomImage.sprite = R.RoomSprite;
            R.RoomImage = RoomImage;
            RectTransform rectTransform = RoomImage.GetComponent<RectTransform>();
            if (R.RoomType == "2x2Rooms")
            {

                rectTransform.sizeDelta = (new Vector2(MapGenerator_Level.height * 2, MapGenerator_Level.width * 2) * MapGenerator_Level.IconScale + 
                    new Vector2(MapGenerator_Level.padding * MapGenerator_Level.height, MapGenerator_Level.padding * MapGenerator_Level.height)) * MapGenerator_Level.Scale;
               
                float offset = (((MapGenerator_Level.height * MapGenerator_Level.IconScale) + ((MapGenerator_Level.padding * MapGenerator_Level.height))) /2) * MapGenerator_Level.Scale ;


                rectTransform.position = ((R.Location * (MapGenerator_Level.IconScale * MapGenerator_Level.height + (MapGenerator_Level.padding * MapGenerator_Level.height * MapGenerator_Level.Scale))) + 
                    new Vector2(offset, offset))*MapGenerator_Level.Scale;
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(MapGenerator_Level.height, MapGenerator_Level.width) * MapGenerator_Level.IconScale * MapGenerator_Level.Scale;
                rectTransform.position = (R.Location * (MapGenerator_Level.IconScale * MapGenerator_Level.height + (MapGenerator_Level.padding * MapGenerator_Level.height))) * MapGenerator_Level.Scale ;
            }


            RoomImage.transform.SetParent(transform, false);
        }
        MapGenerator_Level.Rooms.Add(R);
       // Debug.Log("Drawing Room:" + R.RoomNumber + " at location:" + R.Location);
    }


    int RandomRoomNumber()
    {
        return Random.Range(6,GameObject.Find("Rooms").transform.childCount+6);
    }


    bool CheckIfRoomExists(Vector2 v)
    {
        return (MapGenerator_Level.Rooms.Exists(x => x.Location == v));
    }


    bool CheckIfRoomsAroundGeneratedRoom(Vector2 v, string direction)
    {

        switch (direction)
        {
            case "Right":
                {
                    //Check Down,left,and up
                    if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x - 1, v.y)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y - 1)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y + 1)))
                        return true;
                    break;
                }
            case "Left":
                {
                    //Check Down,Right,and up
                    if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x + 1, v.y)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y - 1)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y + 1)))
                        return true;
                    break;
                }
            case "Up":
                {
                    //Check Down,Right,and Left
                    if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x + 1, v.y)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x -1, v.y)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y - 1)))
                        return true;
                    break;
                }
            case "Down":
                {
   
                    if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y+1)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x -1, v.y)) ||
                       MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(v.x + 1, v.y)))
                        return true;
                    break;
                }

        }



        return false;
    }



    int failsafe = 0;


    void Generate(Room room)
    {
        failsafe++;
        if (failsafe > 50)
        {
            return;
        }

        DrawRoomOnMap(room);


        //Left
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(-1, 0) + room.Location;
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();

            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Right"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < MapGenerator_Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < MapGenerator_Level.RoomLimit)
                    {
                   
                        Generate(newRoom);
                    }

                }
            }
        }

        //Right
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(1, 0) + room.Location;
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Left"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < MapGenerator_Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < MapGenerator_Level.RoomLimit)
                    {
          
                        Generate(newRoom);
                    }
                }
            }
        }

        //Up
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, 1) + room.Location;
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Down"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < MapGenerator_Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < MapGenerator_Level.RoomLimit)
                    {
         
                        Generate(newRoom);
                    }
                }
            }
        }
        //Down
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, -1) + room.Location;
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Up"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < MapGenerator_Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < MapGenerator_Level.RoomLimit)
                    {
          
                        Generate(newRoom);
                    }
                }
            }
        }


        
    }

    private void GenerateBossRoom()
    {
        float MaxNumber = 0;
        Vector2 FarthestRoom = Vector2.zero;

        foreach(Room R in MapGenerator_Level.Rooms)
        {
            if(Mathf.Abs(R.Location.x) + Mathf.Abs(R.Location.y) >= MaxNumber)
            {
                MaxNumber = Mathf.Abs(R.Location.x) + Mathf.Abs(R.Location.y);
                FarthestRoom = R.Location;
            }
            

        }

  
        Room BossRoom = new Room();
        BossRoom.RoomSprite = MapGenerator_Level.BossRoomIcon;
        BossRoom.RoomType = "BossRooms";
        BossRoom.RoomNumber = Random.Range(0,GameObject.Find("BossRooms").transform.childCount);
        Debug.Log("The boss room is:" + BossRoom.RoomNumber);
        BossRoom.SpecialRoom = true;
        //Left
        if (!CheckIfRoomExists(FarthestRoom + new Vector2(-1, 0)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(FarthestRoom + new Vector2(-1, 0), "Right"))
            {
                BossRoom.Location = FarthestRoom + new Vector2(-1, 0);
            }
        }

        //Right
        if (!CheckIfRoomExists(FarthestRoom + new Vector2(1, 0)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(FarthestRoom + new Vector2(1, 0), "Left"))
            {
                BossRoom.Location = FarthestRoom + new Vector2(1, 0);
            }
        }

        //Up
        if (!CheckIfRoomExists(FarthestRoom + new Vector2(0, 1)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(FarthestRoom + new Vector2(0, 1), "Down"))
            {
                BossRoom.Location = FarthestRoom + new Vector2(0, 1);
            }
        }
        //Down
        if (!CheckIfRoomExists(FarthestRoom + new Vector2(0, -1)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(FarthestRoom + new Vector2(0, -1), "Up"))
            {
                BossRoom.Location = FarthestRoom + new Vector2(0, -1);
            }
        }

        DrawRoomOnMap(BossRoom);

    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

    }

    private bool GenerateSpecialRoom(Sprite MapIcon, string RoomType)
    {
        List<Room> ShuffledList = new List<Room>(MapGenerator_Level.Rooms);
        ShuffleList(ShuffledList);

        Room SpecialRoom = new Room();
        SpecialRoom.RoomSprite = MapIcon;
        SpecialRoom.RoomNumber = Random.Range(0, GameObject.Find(RoomType).transform.childCount);
        SpecialRoom.RoomType = RoomType;
        SpecialRoom.SpecialRoom = true;

        bool FoundAvailableLocation = false;

        foreach (Room R in ShuffledList)
        {
            Vector2 SpecialRoomLocation = R.Location;


            if (R.RoomNumber < 6) continue;


            //Left
            if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(-1, 0)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(SpecialRoomLocation + new Vector2(-1, 0), "Right"))
                {
                    SpecialRoom.Location = SpecialRoomLocation + new Vector2(-1, 0);
                    FoundAvailableLocation = true;
                }
            }

            //Right
            else if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(1, 0)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(SpecialRoomLocation + new Vector2(1, 0), "Left"))
                {
                    SpecialRoom.Location = SpecialRoomLocation + new Vector2(1, 0);
                    FoundAvailableLocation = true;
                }
            }

            //Up
            else if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(0, 1)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(SpecialRoomLocation + new Vector2(0, 1), "Down"))
                {
                    SpecialRoom.Location = SpecialRoomLocation + new Vector2(0, 1);
                    FoundAvailableLocation = true;
                }
            }
            //Down
            else if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(0, -1)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(SpecialRoomLocation + new Vector2(0, -1), "Up"))
                {
                    SpecialRoom.Location = SpecialRoomLocation + new Vector2(0, -1);
                    FoundAvailableLocation = true;
                }
            }

            if (FoundAvailableLocation) { 
                DrawRoomOnMap(SpecialRoom);
                return true;
            }

        }

        return false;

    }

    private bool GenerateSecretRoom()
    {
        List<Room> ShuffledList = new List<Room>(MapGenerator_Level.Rooms);
        ShuffleList(ShuffledList);

        foreach (Room R in ShuffledList)
        {
            // x and y < 3 and > -3 starting room is at 0,0
            if (Mathf.Abs(R.Location.x) > 2 || 
                Mathf.Abs(R.Location.y) > 2 || 
                R.Location == Vector2.zero || 
                (R.RoomType != "Rooms" && !R.RoomType.ToLower().Contains("2x2")))
            {
                continue;
            }

            // Define the directions
            Vector2[] directions = { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

            foreach (Vector2 direction in directions)
            {
                Vector2 newLocation = R.Location + direction;

                // Check if a room already exists at the new location
                if (!MapGenerator_Level.Rooms.Exists(x => x.Location == newLocation))
                {
                    if (Mathf.Abs(newLocation.x) > 1 || Mathf.Abs(newLocation.y) >1) //Prevents it from being drawn next to the start room.
                    {
                        bool foundSpecialRoom = false;
                        foreach (Vector2 d in directions)
                        {
                            Vector2 CheckSpecialRoom = newLocation + direction;
                            if (MapGenerator_Level.Rooms.Exists(x => x.Location == CheckSpecialRoom))
                            {
                                string type = MapGenerator_Level.Rooms.First(x => x.Location == CheckSpecialRoom).RoomType.ToLower();
                                if (type != "rooms" && type != "2x2rooms") foundSpecialRoom = true;
                            }
                        }
                        if (!foundSpecialRoom)
                        {
                            CreateNewRoom(newLocation);
                            return true;
                        }
                    }
                }
            }
        }


        return false;
    }


    //Used for Secret Room
    void CreateNewRoom(Vector2 location)
    {
        Room SR = new Room
        {
            Location = location,
            RoomSprite = MapGenerator_Level.SecretRoom,
            Explored = false,
            Revealed = false,
            SpecialRoom = true,
            RoomType = "SecretRooms",
            RoomNumber = Random.Range(1, GameObject.Find("SecretRooms").transform.childCount + 1)
        };

        DrawRoomOnMap(SR);
    }

    private void Awake()
    {
        MapGenerator_Level.DefaultRoomIcon = EmptyRoom;
        MapGenerator_Level.BossRoomIcon = BRoom;
        MapGenerator_Level.CurrentRoomIcon = CurrentRoom;
        MapGenerator_Level.ShopRoomIcon = ShopRoom;
        MapGenerator_Level.TreasureRoomIcon = TreasureRoom;
        MapGenerator_Level.UnexploredRoom = UnexploredRoom;
        MapGenerator_Level.SecretRoom = SecretRoom;
    }

    int maxtries = 0;

    void Start()
    {

        maxtries++;

        Room StartRoom = new Room();
        StartRoom.Location = new Vector2(0, 0);
        StartRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
        StartRoom.Explored = true;
        StartRoom.Revealed = true;
        StartRoom.SpecialRoom = true;
        StartRoom.RoomNumber = Random.Range(0,GameObject.Find("StartRooms").transform.childCount);

       

        StartRoom.RoomType = "StartRooms";

        Player.CurrentRoom = StartRoom;

        //Drawing the starting room
        DrawRoomOnMap(StartRoom);

        //Left
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(-1, 0);
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Right"))
                    Generate(newRoom);
            } 
        }

        //Right
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(1, 0);
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Left"))
                    Generate(newRoom);
            }
        }

        //Up
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, 1);
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
                {
                    if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Down"))
                        Generate(newRoom);
                }
        }
        //Down
        if (Random.value > MapGenerator_Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, -1);
            newRoom.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomExists(newRoom.Location))
                    {
                        if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Up"))
                            Generate(newRoom);
                    }
        }

        GenerateBossRoom();

       bool treasure = GenerateSpecialRoom(MapGenerator_Level.TreasureRoomIcon, "ItemRooms");
       bool shop = GenerateSpecialRoom(MapGenerator_Level.ShopRoomIcon, "ShopRooms");
        bool secret = GenerateSecretRoom();

        if (!treasure || !shop || !secret)
        {
            if (maxtries > 30)
            {
                Debug.Log("The maximum number of tries was hit. Aborting map generation");
                return;
            }

            Regenerate();

        }
        else
        {

            UpdateRooms();

            ChangeRooms.RevealRooms(StartRoom);
            ChangeRooms.RedrawRevealedRooms();

            //only enable the start room after the regenerations have finished.
            if (StartRoomType == "") StartRoomType = "StartRooms";

            Transform StartRooms = GameObject.Find(StartRoomType).transform;
            if (StartRoomNumber == "") StartRooms.Find(StartRoom.RoomNumber.ToString()).gameObject.SetActive(true);
            else
            {
               GameObject t = StartRooms.Find(StartRoomNumber).gameObject;
                t.SetActive(true);
                Transform enemiesTransform = t.transform.Find("Enemies");
                Player.CurrentRoomGameObject = t;
                if (enemiesTransform != null)
                {
                    int childCount = enemiesTransform.childCount;
                    MapGenerator_Level.EnemyCount = childCount;

                }
             
            }


            //Select a Devil and Angel Room
            {
                Transform DevilTransform = GameObject.Find("DevilRooms").transform;
                int rndDevil = Random.Range(1, DevilTransform.childCount+1);
                MapGenerator_Level.DevilRoom = DevilTransform.Find(rndDevil.ToString()).gameObject;
                Room DevilRoom = new Room();
                DevilRoom.RoomType = "DevilRooms";
                DevilRoom.RoomNumber = rndDevil;
                MapGenerator_Level.Rooms.Add(DevilRoom);
            }
            {
                Transform AngelTransform = GameObject.Find("AngelRooms").transform;
                int rndAngel = Random.Range(1, AngelTransform.childCount+1);
                MapGenerator_Level.AngelRoom = AngelTransform.Find(rndAngel.ToString()).gameObject;
                Room AngelRoom = new Room();
                AngelRoom.RoomType = "AngelRooms";
                AngelRoom.RoomNumber = rndAngel;
                MapGenerator_Level.Rooms.Add(AngelRoom);
            }



        }

    }

    public void UpdateRooms()
    {
        try
        {
           

            List<Room> RoomsToReplace = new List<Room>();



            foreach(Room room in MapGenerator_Level.Rooms)
            {
                if (room.SpecialRoom || room.RoomType == "2x2Rooms") continue;

                bool Above = MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(0, 1) && x.SpecialRoom);
                bool Right = MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(1, 0) && x.SpecialRoom);
                bool RightCornerwise = MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(1, 1) && x.SpecialRoom);

                if ((!Above && !Right && !RightCornerwise))
                {
                    if(Random.value > .7f) //30% chance
                    {
                        
                        RoomsToReplace.Add(room);


                    }
                }
            }

            foreach(Room room in RoomsToReplace)
            {

                {
                    Room matchingroom = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location);
                    if (matchingroom != null && matchingroom.RoomType == "2x2Rooms") continue;
                }
                {
                    Room matchingroom = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location + new Vector2(0, 1));
                    if (matchingroom != null && matchingroom.RoomType == "2x2Rooms") continue;
                }
                {
                    Room matchingroom = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location + new Vector2(1, 0));
                    if (matchingroom != null && matchingroom.RoomType == "2x2Rooms") continue;
                }
                {
                    Room matchingroom = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location + new Vector2(1, 1));
                    if (matchingroom != null && matchingroom.RoomType == "2x2Rooms") continue;
                }

                //check all 8 directions for the boss room
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(-1, 1) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(-1, 0) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(0, 2) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(1, 2) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(2, 1) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(2, 0) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(0, -1) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;
                if (MapGenerator_Level.Rooms.Exists(x => x.Location == room.Location + new Vector2(1, -1) && (x.RoomType == "BossRooms" || x.RoomType == "2x2Rooms"))) continue;




                {
                    Room r = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location);
                    if (r != null) { r.RoomImage.color = new Color(0, 0, 0, 0); }
                }
                {
                    Room r = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location + new Vector2(0, 1));
                    if (r != null) { r.RoomImage.color = new Color(0, 0, 0, 0); }
                }
                {
                    Room r = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location + new Vector2(1, 0));
                    if (r != null) { r.RoomImage.color = new Color(0, 0, 0, 0); }
                }
                {
                    Room r = MapGenerator_Level.Rooms.FirstOrDefault(x => x.Location == room.Location + new Vector2(1, 1));
                    if (r != null) { r.RoomImage.color = new Color(0, 0, 0, 0); }
                }


                MapGenerator_Level.Rooms.RemoveAll(x => x.Location == room.Location);
                MapGenerator_Level.Rooms.RemoveAll(x => x.Location == room.Location + new Vector2(0, 1));
                MapGenerator_Level.Rooms.RemoveAll(x => x.Location == room.Location + new Vector2(1, 0));
                MapGenerator_Level.Rooms.RemoveAll(x => x.Location == room.Location + new Vector2(1, 1));


                int roomNumber = Random.Range(0, GameObject.Find("2x2Rooms").transform.childCount);

                Room AnchRoom;

                {
                    Room R = new Room();
                    R.Location = room.Location;
                    R.RoomType = "2x2Rooms";
                    R.RoomNumber = roomNumber;
                    R.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
                    R.AnchorR = R;
                    AnchRoom = R;
                    DrawRoomOnMap(R);
                }
                {
                    Room R = new Room();
                    R.Location = room.Location + new Vector2(0, 1);
                    R.RoomType = "2x2Rooms";
                    R.RoomNumber = roomNumber;
                    R.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
                    R.AnchorRoom = false;
                    R.AnchorR = AnchRoom;
                    DrawRoomOnMap(R);
                }
                {
                    Room R = new Room();
                    R.Location = room.Location + new Vector2(1, 0);
                    R.RoomType = "2x2Rooms";
                    R.RoomNumber = roomNumber;
                    R.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
                    R.AnchorRoom = false;
                    R.AnchorR = AnchRoom;
                    DrawRoomOnMap(R);
                }
                {
                    Room R = new Room();
                    R.Location = room.Location + new Vector2(1, 1);
                    R.RoomType = "2x2Rooms";
                    R.RoomNumber = roomNumber;
                    R.TopRight = true;
                    R.RoomSprite = MapGenerator_Level.DefaultRoomIcon;
                    R.AnchorRoom = false;
                    R.AnchorR = AnchRoom;
                    DrawRoomOnMap(R);
                }


            }


            foreach (Room R in MapGenerator_Level.Rooms)
            {
                if (R.RoomType != "Rooms") continue;

                bool top = MapGenerator_Level.Rooms.Exists(x => x.Location == R.Location + new Vector2(0, 1));
                bool bottom = MapGenerator_Level.Rooms.Exists(x => x.Location == R.Location + new Vector2(0, -1));
                bool left = MapGenerator_Level.Rooms.Exists(x => x.Location == R.Location + new Vector2(-1, 0));
                bool right = MapGenerator_Level.Rooms.Exists(x => x.Location == R.Location + new Vector2(1, 0));

                if (top && bottom && !left && !right)
                {
                    R.RoomType = "TopBottomRooms";
                    R.RoomNumber = Random.Range(0, GameObject.Find("TopBottomRooms").transform.childCount);
                }

                else if (!top && !bottom && left && right)
                {
                    R.RoomType = "LeftRightRooms";
                    R.RoomNumber = Random.Range(0, GameObject.Find("LeftRightRooms").transform.childCount);
                }

                else if (top && !bottom && left && !right)
                {
                    R.RoomType = "TopLeftRooms";
                    R.RoomNumber = Random.Range(0, GameObject.Find("TopLeftRooms").transform.childCount);
                }

                else if (top && !bottom && !left && right)
                {
                    R.RoomType = "TopRightRooms";
                    R.RoomNumber = Random.Range(0, GameObject.Find("TopRightRooms").transform.childCount);
                }

                else if (!top && bottom && left && !right)
                {
                    R.RoomType = "BottomLeftRooms";
                    R.RoomNumber = Random.Range(0, GameObject.Find("BottomLeftRooms").transform.childCount);
                }

                else if (!top && bottom && !left && right)
                {
                    R.RoomType = "BottomRightRooms";
                    R.RoomNumber = Random.Range(0, GameObject.Find("BottomRightRooms").transform.childCount);
                }

            }

        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
   


    bool regenerating = false;
    void StopRegenerating()
    {
        regenerating = false;
    }

   public void Regenerate()
    {
        regenerating = true;
        failsafe = 0;
        MapGenerator_Level.Rooms.Clear();
        Invoke(nameof(StopRegenerating), 1);
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }


        Start();

    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab) && !regenerating)
        {

            maxtries = 0;
            Transform Doors = GameObject.Find("StartRooms").transform.Find(Player.CurrentRoom.RoomNumber.ToString()).transform.Find("Doors");
            for (int i = 0; i < Doors.childCount; i++)
            {
                Doors.GetChild(i).gameObject.SetActive(false);
            }
            Transform StartRooms = GameObject.Find("StartRooms").transform;
            for (int i = 0; i < StartRooms.childCount; i++)
            {
                StartRooms.GetChild(i).gameObject.SetActive(false);
            }
            Regenerate();


            if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(1,0)))
            {
                Doors.Find("RightDoor").gameObject.SetActive(true);
            }
            if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(-1, 0)))
            {
                Doors.Find("LeftDoor").gameObject.SetActive(true);
            }
            if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(0,1)))
            {
                Doors.Find("TopDoor").gameObject.SetActive(true);
            }
            if (MapGenerator_Level.Rooms.Exists(x => x.Location == new Vector2(0,-1)))
            {
                Doors.Find("BottomDoor").gameObject.SetActive(true);
            }
        }

        if (Input.GetKey(KeyCode.P) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);

            string log = "Room List:\n-----------------\n";
     
            foreach (Room R in MapGenerator_Level.Rooms)
            {
                if(R.AnchorRoom)
                log += "Room Type:" + R.RoomType
                    + " Room#:" + R.RoomNumber 
                    + " Location: " + R.Location
                    + " Revealed: " + R.Revealed.ToString()
                    + " Explored: " + R.Explored.ToString()
                    + "\n";
            }

            log +="Current Room: " + "Room Type:" + Player.CurrentRoom.RoomType
                    + " Room#:" + Player.CurrentRoom.RoomNumber
                    + " Location: " + Player.CurrentRoom.Location
                    + " Revealed: " + Player.CurrentRoom.Revealed.ToString()
                    + " Explored: " + Player.CurrentRoom.Explored.ToString()
                    + "\n";
            Debug.Log(log);

            Debug.Log("The # of enemies is: " + MapGenerator_Level.EnemyCount);

            Debug.Log(Player.State);
            
        }

        if(Input.GetKey(KeyCode.M) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);

            foreach(Room R in MapGenerator_Level.Rooms)
            {
                R.Revealed = true;
                R.Explored = true;
                ChangeRooms.RedrawRevealedRooms();
            }

        }

        if (Input.GetKey(KeyCode.J) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);

            Player.CanJump = true;
        }

            if (Input.GetKey(KeyCode.O) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);
            if (DebugCheatMenu.activeInHierarchy == true) DebugCheatMenu.SetActive(false);
            else DebugCheatMenu.SetActive(true);
        }

        if(Input.GetKey(KeyCode.I) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);

            foreach (Room R in MapGenerator_Level.Rooms)
            {
                if(R.RoomType == "ItemRooms")
                {
                    Transform T2 = GameObject.Find(Player.CurrentRoom.RoomType.ToString()).transform;
                    T2.Find(Player.CurrentRoom.RoomNumber.ToString()).gameObject.SetActive(false);

                    GameObject.Find("ItemRooms").transform.Find(R.RoomNumber.ToString()).gameObject.SetActive(true);

                    Player.CurrentRoom = R;
                }
            }
        }

        if (Input.GetKey(KeyCode.R) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);


            Player.transform.GetComponent<InitalizePlayer>().InitializeItemRoomItem();
          
        }
        
        if(Input.GetKey(KeyCode.Z) && !regenerating)
        {
            Player.Damage = 1000;
            Player.AttackSpeed = 20;
            Player.animator.SetFloat("AttackSpeed", 20);
            Player.Health = 20;
            Player.MaxHealth = 20;
            Player.Speed = 80;
            CoroutineManager.Instance.StartCoroutine(HeartScript.WaitAndRedrawHearts());
            Player.transform.localScale = new Vector3(3, 3, 3);

        }
    }

}

*/
