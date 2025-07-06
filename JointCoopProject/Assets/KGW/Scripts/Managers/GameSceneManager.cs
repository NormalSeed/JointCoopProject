using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    static GameSceneManager instance;

    // 초기 SceneManager 생성
    public static GameSceneManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameObject = new GameObject("GameSceneManager");
                instance = gameObject.AddComponent<GameSceneManager>();
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

    // 입력한 씬 전환 (이름)
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // 입력한 씬 전환 (숫자)
    public void LoadScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }

    // 컷 씬 전환
    public void LoadCutScene()
    {
        LoadScene("Real_CutScene");
    }

    // 메인 씬 전환
    public void LoadMainScene()
    {
        LoadScene("Real_MainMenu");
    }

    // 인 게임 씬 전환
    public void LoadIngameScene()
    {
        LoadScene("Real_InGame");
    }

    // 클리어 씬 전환
    public void LoadClearScene()
    {
        LoadScene("Real_Clear");
    }

    // 스테이지 1 전환
    public void LoadStage1Scene()
    {
        LoadScene("Real_Stage1");
    }

    // 스테이지 2 전환
    public void LoadStage2Scene()
    {
        LoadScene("Real_Stage2");
    }

    // 스테이지 3 전환
    public void LoadStage3Scene()
    {
        LoadScene("Real_Stage3");
    }

    // 스테이지 4 전환
    public void LoadStage4Scene()
    {
        LoadScene("Real_Stage4");
    }

    // 스테이지 5 전환
    public void LoadStage5Scene()
    {
        LoadScene("Real_Stage5");
    }

    // 스테이지 6 전환
    public void LoadStage6Scene()
    {
        LoadScene("Real_Stage6");
    }

    // 스테이지 7 전환
    public void LoadStage7Scene()
    {
        LoadScene("Real_Stage7");
    }

}
