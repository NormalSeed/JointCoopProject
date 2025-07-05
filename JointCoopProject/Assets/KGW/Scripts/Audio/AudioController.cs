using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer), typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    VideoPlayer _videoPlayer;
    AudioSource _audioSource;
    public AudioMixer _audioMixer;
    public AudioMixerGroup _videoGroup;

    private void Start()
    {
        Init();
        VideoSetting();
    }

    private void Update()
    {
        // 스킵 키
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameSceneManager.Instance.LoadMainScene();    
        }
    }

    private void Init()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = _videoGroup;
    }

    private void VideoSetting()
    {
        // 비디오 세팅
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;    // 영상에 저장된 오디오 사용

        // 트랙 확인
        if (_videoPlayer.audioTrackCount > 0 )
        {
            Debug.Log("트랙 재생");
            _videoPlayer.EnableAudioTrack(0, true); // 0번 Track 사용 (True)
            _videoPlayer.SetTargetAudioSource(0, _audioSource);
        }
        else
        {
            Debug.Log("트랙 없음");
        }

        // 영상 끝난 후 Main Scene전환 이벤트 등록
        _videoPlayer.loopPointReached -= SceneChange;   // 중복 등록 문제로 인하여 일단 삭제 후 추가
        _videoPlayer.loopPointReached += SceneChange;

        _videoPlayer.Play();
    }

    // Scene 전환
    private void SceneChange(VideoPlayer videoPlayer)
    {
        GameSceneManager.Instance.LoadMainScene();
    }

}
