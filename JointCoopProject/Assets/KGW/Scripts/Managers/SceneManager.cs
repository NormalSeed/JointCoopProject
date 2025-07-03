using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    static SceneManager instance;

    // 초기 SceneManager 생성
    public static SceneManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameObject = new GameObject("SceneManager");
                instance = gameObject.AddComponent<SceneManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        CreateSceneManager();
    }

    private void CreateSceneManager()
    {
        if(instance == null)    // 생성이 되어 있으면 더 만들지 않고 사용
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else    // 중복 삭제
        {
            Destroy(gameObject);
        }
            
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadCutScene()
    {
        LoadScene("Real_CutScene");
    }

    public void LoadMainScene()
    {
        LoadScene("Real_MainMenu");
    }

    public void LoadIngameScene()
    {
        LoadScene("Rear_InGame");
    }

    public void LoadClearScene()
    {
        LoadScene("Rear_Clear");
    }

    //public void LoadDeathScene()
    //{
    //    LoadScene("DeathScene");
    //}
}
