using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    static GameSceneManager instance;
    // UI를 Stack으로 관리
    Stack<GameObject> _UiStack = new Stack<GameObject>();

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

    // UI 열기
    public void OpenUi(UIKeyList uiName)
    {
        GameObject openUi = UIManager.Instance.GetUI(uiName);
        if(openUi == null)
        {
            return;
        }

        if (_UiStack.Count > 0)
        {
            // 열려있는 UI가 있으면 숨김
            _UiStack.Peek().SetActive(false);
        }
        openUi.SetActive(true);
        _UiStack.Push(openUi);
    }

    // UI 닫기
    public void CloseUi()
    {
        if (_UiStack.Count == 0)
        {
            return;
        }
        GameObject closeUi = _UiStack.Pop();
        closeUi.SetActive(false);

        if (_UiStack.Count > 0)
        {
            _UiStack.Peek().SetActive(true);
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
        LoadScene("Rear_InGame");
    }

    // 클리어 씬 전환
    public void LoadClearScene()
    {
        LoadScene("Rear_Clear");
    }

    // 스테이지 1 전환
    public void LoadStage1Scene()
    {
        LoadScene("Rear_Stage1");
    }

    // 스테이지 2 전환
    public void LoadStage2Scene()
    {
        LoadScene("Rear_Stage2");
    }

    // 스테이지 3 전환
    public void LoadStage3Scene()
    {
        LoadScene("Rear_Stage3");
    }

    // 스테이지 4 전환
    public void LoadStage4Scene()
    {
        LoadScene("Rear_Stage4");
    }

    // 스테이지 5 전환
    public void LoadStage5Scene()
    {
        LoadScene("Rear_Stage5");
    }

    // 스테이지 6 전환
    public void LoadStage6Scene()
    {
        LoadScene("Rear_Stage6");
    }

    // 스테이지 7 전환
    public void LoadStage7Scene()
    {
        LoadScene("Rear_Stage7");
    }

}
