using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ChangeSceneManager에서 사용

//public class GameSceneManager : MonoBehaviour
//{
//    static GameSceneManager instance;

//    // 초기 SceneManager 생성
//    public static GameSceneManager Instance
//    {
//        get
//        {
//            if(instance == null)
//            {
//                GameObject gameObject = new GameObject("GameSceneManager");
//                instance = gameObject.AddComponent<GameSceneManager>();
//            }
//            return instance;
//        }
//    }

//    private void Awake()
//    {
//        CreateSceneManager();
//    }

//    private void CreateSceneManager()
//    {
//        if(instance == null)    // 생성이 되어 있으면 더 만들지 않고 사용
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else    // 중복 삭제
//        {
//            Destroy(gameObject);
//        }

//    }

//    // 입력한 씬 전환 (이름)
//    public void LoadScene(string sceneName)
//    {
//        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
//    }

//    // 입력한 씬 전환 (숫자)
//    public void LoadScene(int sceneNumber)
//    {
//        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
//    }
//}
