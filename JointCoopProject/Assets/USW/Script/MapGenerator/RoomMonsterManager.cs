using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMonsterManager : MonoBehaviour
{
    // 방 위치별로 몬스터 리스트 저장
    public Dictionary<Vector2Int, List<MonsterBase>> roomMonsters = new Dictionary<Vector2Int, List<MonsterBase>>();
    
    MapGenerator mapGen;
    PlayerRoomMovement playerMove;
    Vector2Int currentRoom = new Vector2Int(-1, -1); // 현재 활성화된 방
    
    void Start()
    {
        // 필요한 컴포넌트들 찾기
        mapGen = FindObjectOfType<MapGenerator>();
        playerMove = FindObjectOfType<PlayerRoomMovement>();
        
        // 맵 생성 완료 후 몬스터 설정
        StartCoroutine(SetupMonsters());
    }
    
    IEnumerator SetupMonsters()
    {
        yield return new WaitForEndOfFrame();
        
        FindAllMonsters();      // 모든 몬스터 찾아서 방별로 분류
        TurnOffAllMonsters();   // 전부 비활성화
        
        // 시작 방만 활성화
        if (mapGen != null)
        {
            TurnOnMonstersInRoom(mapGen.startPosition);
            currentRoom = mapGen.startPosition;
        }
    }
    
    void Update()
    {
        // PlayerRoomMovement 못 찾았으면 다시 찾기
        if (playerMove == null)
        {
            playerMove = FindObjectOfType<PlayerRoomMovement>();
            if (playerMove == null)
            {
                return; // 못 찾으면 그냥 리턴
            }
        }
        
        Vector2Int playerRoom = playerMove.GetCurrentRoom();
        
        // 플레이어가 다른 방으로 이동했으면
        if (playerRoom != currentRoom)
        {
            // 이전 방 몬스터들 끄고
            TurnOffMonstersInRoom(currentRoom);
            
            // 새 방 몬스터들 켜기
            TurnOnMonstersInRoom(playerRoom);
            
            currentRoom = playerRoom;
        }
    }
    
    // 씬에 있는 모든 몬스터 찾아서 방별로 분류하기
    void FindAllMonsters()
    {
        // 비활성화된 몬스터까지 포함해서 찾기
        MonsterBase[] monsters = FindObjectsOfType<MonsterBase>(true);
        
        // 각 몬스터가 어느 방에 있는지 계산
        for(int i = 0; i < monsters.Length; i++)
        {
            MonsterBase monster = monsters[i];
            Vector2Int roomPos = GetRoomPosition(monster.transform.position);
            
           
            
            // 해당 방에 리스트가 없으면 새로 만들기
            if (!roomMonsters.ContainsKey(roomPos))
            {
                roomMonsters[roomPos] = new List<MonsterBase>();
            }
            
            // 몬스터를 해당 방 리스트에 추가
            roomMonsters[roomPos].Add(monster);
        }
    }
    
    // 몬스터 월드 좌표로 어느 방에 있는지 계산
    Vector2Int GetRoomPosition(Vector3 monsterPos)
    {
        if (mapGen == null) return Vector2Int.zero;
        
        // 방 크기로 나눠서 그리드 좌표 구하기
        int x = Mathf.FloorToInt(monsterPos.x / mapGen.prefabSize.x);
        int y = Mathf.FloorToInt(monsterPos.y / mapGen.prefabSize.y);
        
        return new Vector2Int(x, y);
    }
    
    // 모든 몬스터 비활성화 (게임 시작할 때)
    void TurnOffAllMonsters()
    {
        int count = 0;
        
        foreach (var monsters in roomMonsters.Values)
        {
            for(int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] != null)
                {
                    monsters[i].gameObject.SetActive(false);
                    count++;
                }
            }
        }
    }
    
    // 특정 방의 몬스터들 활성화
    void TurnOnMonstersInRoom(Vector2Int roomPos)
    {
        if (roomMonsters.ContainsKey(roomPos))
        {
            List<MonsterBase> monsters = roomMonsters[roomPos];
            int activated = 0;
            
            for(int i = 0; i < monsters.Count; i++)
            {
                MonsterBase monster = monsters[i];
                
                // 몬스터가 살아있으면 활성화
                if (monster != null && !monster._isDead)
                {
                    monster.gameObject.SetActive(true);
                    activated++;
                }
            }
            
        }
       
    }
    
    // 특정 방의 몬스터들 비활성화
    void TurnOffMonstersInRoom(Vector2Int roomPos)
    {
        if (roomMonsters.ContainsKey(roomPos))
        {
            List<MonsterBase> monsters = roomMonsters[roomPos];
            int deactivated = 0;
            
            for(int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] != null)
                {
                    monsters[i].gameObject.SetActive(false);
                    deactivated++;
                }
            }
        }
    }
    
    // 몬스터가 죽었을 때 리스트에서 제거 
    public void MonsterDied(MonsterBase monster, Vector2Int roomPos)
    {
        if (roomMonsters.ContainsKey(roomPos))
        {
            roomMonsters[roomPos].Remove(monster);
        }
    }
    
    // 해당 방의 모든 몬스터가 죽었는지 확인
    public bool IsRoomClear(Vector2Int roomPos)
    {
        // 그 방에 몬스터가 없으면 클리어된 걸로 처리
        if (!roomMonsters.ContainsKey(roomPos))
        {   Debug.Log("몬스터 없음 클리어로 처리");
            return true;
        }

        List<MonsterBase> monsters = roomMonsters[roomPos];
        // 살아있는 몬스터가 하나라도 있으면 아직 클리어 안됨
        for(int i = 0; i < monsters.Count; i++)
        {
            MonsterBase monster = monsters[i];
            if (monster != null && !monster._isDead && monster.gameObject.activeInHierarchy)
            {
                return false; // 살아있는 몬스터 발견
            }
        }
        Debug.Log("몬스터 다죽음");
        return true; // 모든 몬스터가 죽음
    }
}