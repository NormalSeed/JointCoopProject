using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

// ChangeSceneManager에서 사용

//public class AudioController : MonoBehaviour
//{
//    VideoPlayer _videoPlayer;

//    private void Start()
//    {
//        Init();
//        VideoSetting();
//    }

//    private void Update()
//    {
//        // 스킵 키
//        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
//        {
//            SkipCutScene();
//        }
//    }

//    private void Init()
//    {
//        _videoPlayer = GetComponent<VideoPlayer>();
//    }

//    private void VideoSetting()
//    {
//        // 비디오 세팅
        

        

//        // 영상 끝난 후 Main Scene전환 이벤트 등록
//        _videoPlayer.loopPointReached -= SceneChange;   // 중복 등록 문제로 인하여 일단 삭제 후 추가
//        _videoPlayer.loopPointReached += SceneChange;

//        _videoPlayer.Play();
//    }

//    private void SkipCutScene()
//    {
//        SoundManager.Instance.StopBGM();
//        GameSceneManager.Instance.LoadMainScene();
//    }

//    // Scene 전환
//    private void SceneChange(VideoPlayer videoPlayer)
//    {
//        SoundManager.Instance.StopBGM();
//        GameSceneManager.Instance.LoadMainScene();
//    }

//}
