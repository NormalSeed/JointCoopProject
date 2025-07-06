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

    private void Start()
    {
        InitDisplayMode();
        InitResolusion();
        InitSoundVolume();
    }

    // 디스플레이 초기화
    private void InitDisplayMode()
    {
        _disPlayDropdown.ClearOptions();    // Clear
        _disPlayDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Full Screen", "Windowed"
        });

        _disPlayDropdown.onValueChanged.AddListener(SetDisplayMode);

        // 현재 모드 설정
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:   // 전체화면
                _disPlayDropdown.value = 0;
                break;
            case FullScreenMode.Windowed:           // 창 화면
                _disPlayDropdown.value = 1;
                break;
            default:
                _disPlayDropdown.value = 0;         // 기본은 전체화면
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

        var option = new System.Collections.Generic.List<string>();

        int currentIndex = 0;

        for (int i = 0; i < _resolution.Length; i++)
        {
            string resolusionString = $"{_resolution[i].width} x {_resolution[i].height}";
            option.Add(resolusionString);

            if ((_resolution[i].width == Screen.currentResolution.width) && (_resolution[i].height == Screen.currentResolution.height))
            {
                currentIndex = i;
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
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
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
