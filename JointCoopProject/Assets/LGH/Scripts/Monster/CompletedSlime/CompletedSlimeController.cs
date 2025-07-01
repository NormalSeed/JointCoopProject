using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedSlimeController : MonsterBase
{
    [SerializeField] private GameObject _compoundedSlime;
    protected override void Init()
    {
        base.Init();
        _monsterID = 10303;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SpawnCompoundedSlime()
    {
        UnactivateSelf();
        Vector2 spawnPos = transform.position;
        Instantiate(_compoundedSlime, spawnPos, Quaternion.identity);
    }
}
