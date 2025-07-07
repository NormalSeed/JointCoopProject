using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Reference Button")]
    [SerializeField] Button _playButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _creditsButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _mainMenuButton1;
    [SerializeField] Button _mainMenuButton2;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // 클릭 이벤트 초기화 세팅
        // Open Button
        _playButton.onClick.AddListener(() =>
        {
            GameSceneManager.Instance.LoadStage1Scene();
        });
        _optionButton.onClick.AddListener(() => UIManager.Instance.OpenUi(UIKeyList.mainOption));
        _creditsButton.onClick.AddListener(() => UIManager.Instance.OpenUi(UIKeyList.credit));
        _exitButton.onClick.AddListener(() => Application.Quit());

        // Close Button
        _mainMenuButton1.onClick.AddListener(() => UIManager.Instance.CloseUi());
        _mainMenuButton2.onClick.AddListener(() => UIManager.Instance.CloseUi());

    }
}
