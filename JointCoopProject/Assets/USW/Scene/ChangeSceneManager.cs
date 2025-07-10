using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChangeSceneManager : MonoBehaviour
{
    public bool useNextStage = true;
    public string targetSceneName = "";

    public bool isInGame = false;
    public bool isCutScene = false;
    public bool isMainMenu = false;

    private bool isLoading = false;
    private bool hasSkipped = false;

    [Header("로딩 설정")] public bool useAsyncLoading = true;
    public GameObject loadingUI;
    public Slider loadingBar;
    public Text loadingText;
    public float minLoadingTime = 2f;

    public VideoPlayer videoPlayer;
    
    public bool allowClickSkip = false;
    public int _CursceneIndex;  // 현재 씬 저장

    public Button gameStartButton;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInGame || isLoading) return;


        if (!other.CompareTag("Player")) return;

        var playerMovement = other.GetComponent<PlayerRoomMovement>();
        if (playerMovement != null)
        {
            playerMovement.PrepareForScene();
        }


        GoToNextScene();
    }

    void SetupCutScene()
    {
        if (videoPlayer != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;    // 오디오 소스 사용

            // Sound Manager의 컷씬 BGM 재생
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_OP_Audio);

            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void SetUpMainMenu()
    {
        if (gameStartButton != null)
        {
            gameStartButton.onClick.AddListener(StartGame);
        }   
    }

    private void Start()
    {
        SetupManager();
    }

    void SetupManager()
    {
        if (loadingUI != null)
        {
            loadingUI = Instantiate(loadingUI);
        }
        if (loadingUI != null)
        {
            loadingUI.SetActive(false);
        }
        if (isCutScene)
        {
            SetupCutScene();
        }
        else if (isMainMenu)
        {
            SetUpMainMenu();
        }
    }

    private void Update()
    {
        if (isCutScene && !hasSkipped)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || allowClickSkip && Input.GetMouseButtonDown(0))
            {
                SkipCutScene();
            }
        }
    }

    // 컷씬 
    //
    void SkipCutScene()
    {
        if (hasSkipped || isLoading) return;
        hasSkipped = true;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        GoToNextScene();
    }

    public void GoToNextScene()
    {
        if (isLoading) return;

        if (useNextStage)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                if (useAsyncLoading)
                {
                    StartCoroutine(LoadSceneAsync(nextIndex));
                }
                else
                {
                    SceneManager.LoadScene(nextIndex);
                }
            }
            else
            {
                if (useAsyncLoading)
                {
                    StartCoroutine(LoadSceneAsync("Real_MainMenu"));
                }
                else
                {
                    SceneManager.LoadScene("Real_MainMenu");
                }
            }
        }
        else if (!string.IsNullOrEmpty(targetSceneName))
        {
            if (useAsyncLoading)
            {
                StartCoroutine(LoadSceneAsync(targetSceneName));
            }
            else
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }
    


    // 다음씬으로 넘어가는거 하나 
    // 메인메뉴 가는거 하나하고 
    // 재시작 넣으려나 ? 

    // 재시작 기능 넣는다면 넣기. 
    //public static void ReStartScene()
    //{
    //   SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

   
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //   if (other.CompareTag("Player"))
    //   {
    //      SceneManager.LoadScene("Real_Stage2");
    //   }
    //}


    IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;

        if (loadingUI != null)
        {
            loadingUI.SetActive(true);
        }

        float minTimeTimer = 0f;
        
        //비동기 로딩 시작
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        
        // 로딩 진행률 표시
        while (asyncOperation.progress < 0.9f)
        {
            minTimeTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            
            if (minTimeTimer < minLoadingTime)
            {
                progress = Mathf.Min(progress, minTimeTimer / minLoadingTime);
            }

            UpdateLoadingUI(progress);

            yield return null;
        }

        while (minTimeTimer < minLoadingTime)
        {
            minTimeTimer += Time.deltaTime;
            
            float progress = minTimeTimer / minLoadingTime;
            
            UpdateLoadingUI(progress);
            
            yield return null;
        }
        
        UpdateLoadingUI(1f);
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        
        
        
        if (loadingUI != null)
        {
            loadingUI.SetActive(false);
        }

        isLoading = false;
        
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        isLoading = true;

        if (loadingUI != null)
        {
            loadingUI.SetActive(true);
        }

        float minTimeTimer = 0f;
        
        //비동기 로딩 시작
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        
      
        asyncOperation.allowSceneActivation = false;

        
        // 로딩 진행률 표시
        while (asyncOperation.progress < 0.9f)
        {
            minTimeTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            
            if (minTimeTimer < minLoadingTime)
            {
                progress = Mathf.Min(progress, minTimeTimer / minLoadingTime);
            }

            UpdateLoadingUI(progress);

            yield return null;
        }

        while (minTimeTimer < minLoadingTime)
        {
            minTimeTimer += Time.deltaTime;
            
            float progress = minTimeTimer / minLoadingTime;
            
            UpdateLoadingUI(progress);
            
            yield return null;
        }
        
        UpdateLoadingUI(1f);

        switch (sceneIndex)
        {
            case 1:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Title); // Title Scene BGM ON
                break;
            case 2:
            case 3:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage1);    // Stage1,2 BGM ON
                _CursceneIndex = sceneIndex;
                break;
            case 4:
            case 5:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage2);    // stage3,4 BGM ON
                _CursceneIndex = sceneIndex;
                break;
            case 6:
            case 7:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage3);    // stage5,6 BGM ON
                _CursceneIndex = sceneIndex;
                break;
            default:
                break;
        }

        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
      
        if (loadingUI != null)
        {
            loadingUI.SetActive(false);
        }
        isLoading = false;   
    }

    void UpdateLoadingUI(float progress)
    {
        if (loadingBar != null)
        {
            loadingBar.value = progress;
        }
        // text 도 넣을꺼닞 고민 한번 해야할듯
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }

        if (gameStartButton != null)
        {
            gameStartButton.onClick.RemoveAllListeners();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (hasSkipped || isLoading) return;
        GoToNextScene();
    }

    public void StartGame()
    {
        PlayerStatManager.Instance.CreatePlayerStatManager();   // Player Stat Reset
        PlayerStatManager.Instance._alive = true;
        // InventoryManager.GetInstance().Init();
        //InventoryManager.CreateInstance();
        ItemManager.inventory.Init();
        if (isLoading) return;
        GoToNextScene();
    }
}