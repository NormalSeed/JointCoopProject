using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameItem[] _itemList;

    private Dictionary<Transform, GameItem> _itemSpawnPairs = new(3);
    private Dictionary<Transform, GameItem> _secretSpawnPairs = new(3);

    private Transform _itemRoom;
    private Transform _secretRoom;

    private MapGenerator _mapGenerator;

    private Coroutine _spawnRoutine;
    private System.Random _randomInstance;

    private Regex _itemRoomName = new Regex(@"ItemRoom_[0-9]{1}_[0-9]{1}");
    private Regex _secretRoomName = new Regex(@"SecretRoom_[0-9]{1}_[0-9]{1}");
    private Regex _spawnPointName = new Regex(@"ItemSpawnP_[0-9]{1}");

    void Awake()
    {
        _mapGenerator = FindObjectOfType<MapGenerator>();
        _randomInstance = new System.Random();
    }
    void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnItemToRoom());
    }
    private IEnumerator SpawnItemToRoom()
    {
        // 맵 생성 후 방 초기화까지 기다림 => WaitUntil()로 조건을 걸면 가장 베스트이나 기존 코드를 손대지 않는 선에서 작업하는 것이 필요함
        yield return new WaitForEndOfFrame();

        FindSpawnRooms();
        if (_itemRoom != null)
        {
            _itemSpawnPairs = GetRandomSpawnPair(_itemRoom);
        }
        if (_secretRoom != null)
        {
            _secretSpawnPairs = GetRandomSpawnPair(_secretRoom, true);
        }
        SpawnItemsInItemRoom();
        SpawnItemsInSecretRoom();
        StopCoroutine(_spawnRoutine);
        _spawnRoutine = null;
    }
    private void FindSpawnRooms()
    {
        Transform[] allRooms = GameObject.Find("Map").transform.GetComponentsInChildren<Transform>();
        foreach (Transform room in allRooms)
        {
            if (_itemRoomName.IsMatch(room.name))
            {
                _itemRoom = room;
            }
            else if (_secretRoomName.IsMatch(room.name))
            {
                _secretRoom = room;
            }
        }
    }

    private Dictionary<Transform, GameItem> GetRandomSpawnPair(Transform searchRoom, bool isSecret = false)
    {
        Dictionary<Transform, GameItem> returnDict = new();
        int maxRepeat = (isSecret) ? 3 : GetSpawnCount();
        int repeatCount = 0;

        Transform[] allRooms = searchRoom.Find("Grid").transform.GetComponentsInChildren<Transform>();
        foreach (Transform room in allRooms)
        {
            if (repeatCount < maxRepeat && _spawnPointName.IsMatch(room.name))
            {
                returnDict.Add(room, GetRandomItemInPool());
                repeatCount++;
            }
        }
        return returnDict;
    }

    private GameItem GetRandomItemInPool()
    {
        return _itemList[_randomInstance.Next(0, _itemList.Length)];
    }

    private int GetSpawnCount()
    {
        return _randomInstance.Next(1, 4);
    }

    private void SpawnItemsInItemRoom()
    {
        if (_itemSpawnPairs.Count > 0)
        {
            foreach (KeyValuePair<Transform, GameItem> item in _itemSpawnPairs)
            {
                Instantiate(item.Value, item.Key);
            }
        }
    }
    private void SpawnItemsInSecretRoom()
    {
        if (_secretSpawnPairs.Count > 0)
        {
            foreach (KeyValuePair<Transform, GameItem> item in _secretSpawnPairs)
            {
                Instantiate(item.Value, item.Key);
            }
        }
    }
}
