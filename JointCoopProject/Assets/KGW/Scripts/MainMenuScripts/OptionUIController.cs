using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIController : MonoBehaviour
{
    [Header("Dropdowns UI Reference")]
    [SerializeField] TMP_Dropdown _disPlayDropdown;
    [SerializeField] TMP_Dropdown _resolutionDropdown;

    [Header("Sound Slider UI Reference")]
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _soundEffectSlider;

    Resolution[] _resolution = new Resolution[] { };
    bool _isFullScreen = true;

    private void Start()
    {
        InitDisplayMode();
        InitResolusion();
        InitSoundVolume();      
    }

    private void Update()
    {
        Debug.Log($"사운드 볼륨 조절 : {_musicSlider.value}");
        Debug.Log($"이펙트 볼륨 조절 : {_soundEffectSlider.value}");
    }

    // 디스플레이 초기화
    private void InitDisplayMode()
    {
        _disPlayDropdown.ClearOptions();    // Clear
        _disPlayDropdown.AddOptions(new List<string>
        {
            "Full Screen", "Windowed"
        });

        _disPlayDropdown.onValueChanged.AddListener(SetDisplayMode);

        // 현재 모드 설정
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:   // 전체화면
                _disPlayDropdown.value = 0;
                _isFullScreen = true;
                break;
            case FullScreenMode.Windowed:           // 창 화면
                _disPlayDropdown.value = 1;
                _isFullScreen = false;
                break;
            default:
                _disPlayDropdown.value = 0;         // 기본은 전체화면
                _isFullScreen = true;
                break;
        }
    }

    // 디스플레이 설정
    private void SetDisplayMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    // 해상도 초기화
    private void InitResolusion()
    {
        _resolutionDropdown.ClearOptions();
        _resolution = Screen.resolutions;

        var option = new List<string>();
        var _resolutionSet = new HashSet<string>();
        int currentIndex = 0;

        for (int i = 0; i < _resolution.Length; i++)
        {
            string resolusionString = $"{_resolution[i].width} x {_resolution[i].height}";
            

            if (_resolutionSet.Add(resolusionString))   // 중복 제거
            {
                option.Add(resolusionString);

                if ((_resolution[i].width == Screen.currentResolution.width) && (_resolution[i].height == Screen.currentResolution.height))
                {
                    currentIndex = option.Count - 1;
                }
            }
        }

        _resolutionDropdown.AddOptions(option);
        _resolutionDropdown.value = currentIndex;
        _resolutionDropdown.RefreshShownValue();
        _resolutionDropdown.onValueChanged.AddListener(SetResolusion);
    }

    // 해상도 설정
    private void SetResolusion(int index)
    {
        Resolution resolution = _resolution[index];
        Screen.SetResolution(resolution.width, resolution.height, _isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    // BGM, SFX Sound 초기화
    private void InitSoundVolume()
    {
        _musicSlider.value = SoundManager.Instance.audioBgm.volume;
        _soundEffectSlider.value = SoundManager.Instance.audioSfx.volume;

        _musicSlider.onValueChanged.AddListener((value) => SoundManager.Instance.BGM_Volume(value));
        _soundEffectSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SFX_Volume(value));
    }
}
