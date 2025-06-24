using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; 
using UnityEngine;
using UnityEngine.Video;

/*
public class Managers : MonoBehaviour
{
    public static event Action OnManagersInitialized;
    
    public static GameManager Instance;

    /// <summary>
    /// Manger 구분, MonoBehaviour 상속받는 매니저와 아닌 것들로 구분.
    /// 직접 인스턴스화가 필요한 친구들과 유니티기능이 필요로하는 친구들은 상속. 
    /// </summary>

    ///<summary>
    /// MonoBehavior 필요한친구들.
    /// </summary>
    
    // private PoolManager _pool;
    
    /// <summary>
    /// 참조용 프로퍼티
    /// </summary>
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; 
        
        DontDestroyOnLoad(gameObject);
        
        InitManagers();
    }

    public void InitManagers()
    {
        // Monobehaviour 상속받지 않는 매니저들 인스턴스화 및 초기화
        _pool = new PoolManager();
       
        if (dataManager != null)
        {
            //dataManager.InitManagers(); 
        }
    }
    
    public void Clear()
    {
        // 게임 종료시 매니저들 클리어 해주기.
        //dataManager?.Clear();
    }
}
*/