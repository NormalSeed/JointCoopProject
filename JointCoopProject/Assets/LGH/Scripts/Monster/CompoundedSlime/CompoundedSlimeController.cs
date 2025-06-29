using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundedSlimeController : MonsterBase
{
    [SerializeField] private GameObject _infectedSlime1;
    [SerializeField] private GameObject _infectedSlime2;
    protected override void Init()
    {
        base.Init();
        _monsterID = 10201;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Die();
        }
    }

    public void SpawnInfectedSlime()
    {
        Debug.Log("InfectedSlimeµéÀÌ »ý¼ºµÊ");
        Vector2 spawnPos = transform.position;
        Instantiate(_infectedSlime1, spawnPos + new Vector2(-0.3f, 0), Quaternion.identity);
        Instantiate(_infectedSlime2, spawnPos + new Vector2(0.3f, 0), Quaternion.identity);
    }
}

