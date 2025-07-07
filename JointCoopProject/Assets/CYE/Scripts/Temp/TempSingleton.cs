using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TempSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static void CreateInstance()
    {
        if(_instance == null)
        {
            T prefab = Resources.Load<T>(typeof(T).Name);
            _instance = Instantiate(prefab);
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    public static void ReleaseInstance()
    {
        if(_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }

    public static T GetInstance()
    {
        return _instance;
    }
}
