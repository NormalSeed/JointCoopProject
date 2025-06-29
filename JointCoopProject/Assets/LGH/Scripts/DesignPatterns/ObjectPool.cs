using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Stack<PooledObject> pool;
    private PooledObject prefab;
    private GameObject poolObject;

    public ObjectPool(Transform parent, PooledObject targetPrefab, int initSize) => Init(parent, targetPrefab, initSize);

    private void Init(Transform parent, PooledObject targetPrefab, int initSize)
    {
        pool = new Stack<PooledObject>(initSize);
        prefab = targetPrefab;
        poolObject = new GameObject($"{targetPrefab.name}.pool");
        poolObject.transform.parent = parent;

        for (int i = 0; i < initSize; i++)
        {
            CreatePooledObject();
        }
    }

    public void PushPool(PooledObject target)
    {
        target.transform.parent = poolObject.transform;
        target.gameObject.SetActive(false);
        pool.Push(target);
    }

    public PooledObject PopPool()
    {
        if (pool.Count == 0) CreatePooledObject();

        PooledObject obj = pool.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }

    private void CreatePooledObject()
    {
        PooledObject obj = MonoBehaviour.Instantiate(prefab);
        obj.PooledInit(this);
        PushPool(obj);
    }
}
