using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 딕셔너리 키 입력에 대한 오타 방지로 enum 사용
public enum UIKeyList
{
    // MainMenu UIKeyList
    mainOption, credit,
    // InGame UIKeyList
    miniMap, playerHp, activeItem, activeItemGuage, Chip, bomb, inventory, confirmWindow, optionWindow, deathWindow, fortune, itemTitle, itemDescription
}

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    Dictionary<UIKeyList, GameObject> _UiDictionary = new Dictionary<UIKeyList, GameObject>();

    [Header("InGame UI References")]
    [SerializeField] GameObject _miniMapUI;   // 미니맵 UI
    [SerializeField] GameObject _playerHpUI;    // 플레이어 체력 UI
    [SerializeField] GameObject _activeItemUI; // 액티브 아이템 UI
    [SerializeField] GameObject _activeItemGuageUI; // 액티브 아이템 UI
    [SerializeField] GameObject _ChipUI;        // 획득한 골드 UI
    [SerializeField] GameObject _bombUI;        // 획득한 폭탄 UI
    [SerializeField] GameObject _inventoryUI;   // 인벤토리 UI
    [SerializeField] GameObject _confirmWindowUI;   // 확인 창 UI
    [SerializeField] GameObject _optionWindowUI;   // 옵션 창 UI
    [SerializeField] GameObject _deathWindowUI;   // 캐릭터 죽음 창 UI
    [SerializeField] GameObject _fortuneUI;   // 오늘의 운세 창 UI
    [SerializeField] GameObject _itemTitleUI;   // 아이템 타이틀 UI
    [SerializeField] GameObject _itemDescriptionUI;   // 아이템 설명 UI

    [Header("MainMenu UI References")]
    [SerializeField] GameObject _mainOptionUI;    // 메인 옵션 창 UI
    [SerializeField] GameObject _creditUI;      // 메인 크래딧 UI

    [Header("Button UI Reference")]
    [SerializeField] Button _optionWindowButton;    // 옵션 창 열림 버튼
    [SerializeField] Button _mainMenuButton;    // 메인 화면 이동 버튼
    [SerializeField] Button _inventoryCloseButton;    // 인벤토리 닫기 버튼
    [SerializeField] Button _yesButton;    // Yes 버튼
    [SerializeField] Button _noButton;    // No 버튼
    [SerializeField] Button _optionCloseButton;    // 옵션 닫기 버튼

    [Header("Player Heart Controll")]
    [SerializeField] PlayerHeartController _playerHeartController;  // 플레이어 하트 컨트롤 스크립트

    // 초기 UIManager 생성
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("UIManager");
                instance = gameObject.AddComponent<UIManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        CreateUIManager();
        Init();
    }

    private void Update()
    {
        OnInventoryOpen();
        HeartUpdateUI(PlayerStatManager.Instance._playerHp, PlayerStatManager.Instance._playerMaxHp);
    }

    private void CreateUIManager()
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

    // 딕셔너리에 UI 추가 (오타 방지로 키 == enum UIKeyList)
    private void Init()
    {
        // InGame UI 추가
        _UiDictionary[UIKeyList.miniMap] = _miniMapUI;
        _UiDictionary[UIKeyList.playerHp] = _playerHpUI;
        _UiDictionary[UIKeyList.activeItem] = _activeItemUI;
        _UiDictionary[UIKeyList.activeItemGuage] = _activeItemGuageUI;
        _UiDictionary[UIKeyList.Chip] = _ChipUI;
        _UiDictionary[UIKeyList.bomb] = _bombUI;
        _UiDictionary[UIKeyList.inventory] = _inventoryUI;
        _UiDictionary[UIKeyList.confirmWindow] = _confirmWindowUI;
        _UiDictionary[UIKeyList.optionWindow] = _optionWindowUI;
        _UiDictionary[UIKeyList.deathWindow] = _deathWindowUI;
        _UiDictionary[UIKeyList.fortune] = _fortuneUI;
        _UiDictionary[UIKeyList.itemTitle] = _itemTitleUI;
        _UiDictionary[UIKeyList.itemDescription] = _itemDescriptionUI;

        // Main Menu UI 추가
        _UiDictionary[UIKeyList.mainOption] = _mainOptionUI;
        _UiDictionary[UIKeyList.credit] = _creditUI;

    }

    // UI가져오기
    public GameObject GetUI(UIKeyList uiName)
    {
        GameObject uiKeyName = _UiDictionary.ContainsKey(uiName) ? _UiDictionary[uiName] : null;
        if (uiKeyName == null)
        {
            Debug.Log($"요청하신 UI({uiName})가 존재하지 않습니다.");
        }

        return uiKeyName;
    }

    // 인벤토리 창 오픈
    public void OnInventoryOpen()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameSceneManager.Instance != null)
            {
                GameSceneManager.Instance.OpenUi(UIKeyList.inventory);
                // 인벤토리 창
                _inventoryCloseButton.onClick.AddListener(() => GameSceneManager.Instance.CloseUi());   // 인벤토리 창 닫음
                _optionWindowButton.onClick.AddListener(() => GameSceneManager.Instance.OpenUi(UIKeyList.optionWindow));    // 옵션 창 열기
                _mainMenuButton.onClick.AddListener(() => GameSceneManager.Instance.OpenUi(UIKeyList.confirmWindow));   // 확인 창 열기

                // 확인 창
                _yesButton.onClick.AddListener(() => GameSceneManager.Instance.LoadMainScene());    // Yes 버튼 : 메인 창 전환
                _noButton.onClick.AddListener(() => GameSceneManager.Instance.CloseUi()); // No 버튼 : 인벤토리 창 전환

                // 옵션 창
                _optionCloseButton.onClick.AddListener(() => GameSceneManager.Instance.CloseUi());  // 옵션 창 닫음 : 인벤토리 창 전환
            }
            else
            {
                Debug.LogWarning("GameSceneManager가 초기화되지 않았습니다.");
                return;
            }
        }
    }

    public void HeartUpdateUI(float playerCurrentHp, float playerMaxHp)
    {
        _playerHeartController.HeartsUpdate(playerCurrentHp, playerMaxHp);
    }
}
