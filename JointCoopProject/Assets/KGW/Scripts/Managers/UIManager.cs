using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 딕셔너리 키 입력에 대한 오타 방지로 enum 사용
public enum UIKeyList
{
    // MainMenu UIKeyList
    mainOption, credit,
    // InGame UIKeyList
    miniMap, playerHp, activeItem, activeItemGuage, Chip, bomb, inventory, confirmWindow, optionWindow, deathWindow, fortune, itemInfo
}

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    Dictionary<UIKeyList, GameObject> _UiDictionary = new Dictionary<UIKeyList, GameObject>();
    // UI를 Stack으로 관리
    Stack<GameObject> _UiStack = new Stack<GameObject>();

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
    [SerializeField] GameObject _itemInfoUI;   // 아이템 정보 창 UI

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
    [SerializeField] Button _deathMainMenuButton;    // 메인 화면 이동 버튼 (죽은 후)

    [Header("Player Heart Controll")]
    [SerializeField] PlayerHeartController _playerHeartController;  // 플레이어 하트 컨트롤 스크립트
    [SerializeField] ItemGuageController _itemGuageController;  // 아이템 게이지 컨트롤 스크립트

    // 초기 UIManager 생성
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬 이동 시 UIManager의 참조가 None으로 되는 현상으로 UIManager를 프리팹으로 만들어 생성
                GameObject UIManagerPrefab = Resources.Load<GameObject>("UIManager");

                if (UIManagerPrefab == null)
                {
                    Debug.LogError("UIManager 프리팹을 Resources/UIManager 에서 찾을 수 없습니다.");
                    return null;
                }

                GameObject gameObject = Instantiate(UIManagerPrefab);
                instance = gameObject.GetComponent<UIManager>();

                DontDestroyOnLoad(gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        CreateUIManager();
        Init();
        // 씬 Change 시 UI 재참조 진행
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (_inventoryUI != null)
        {
            OnInventoryOpen();
            StatUpdateUI();
        }
        if (_playerHpUI != null)
        {
            HeartUpdateUI(PlayerStatManager.Instance._playerHp, PlayerStatManager.Instance._playerMaxHp);
        }
        if (_activeItemGuageUI != null)
        {
            GetItem();
            UseItem();
        }
        ChipUpdateUI();
        BombUpdateUI();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        if (_deathMainMenuButton != null)
        {
            _deathMainMenuButton.onClick.RemoveAllListeners();
        }
    }

    private void CreateUIManager()
    {
        if(instance == null)    // 생성이 되어 있으면 더 만들지 않고 사용
        {
            instance = this;
            Init();
        }
        else if(instance != this)   // 인스펙터 값 보존
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
        _UiDictionary[UIKeyList.itemInfo] = _itemInfoUI;

        // Main Menu UI 추가
        _UiDictionary[UIKeyList.mainOption] = _mainOptionUI;
        _UiDictionary[UIKeyList.credit] = _creditUI;        
    }

    private void ReferenceUIReflesh()
    {
        _miniMapUI = _miniMapUI != null ? _inventoryUI : GameObject.Find("miniMap");
        _playerHpUI = _playerHpUI != null ? _playerHpUI : GameObject.Find("Frame_Hearts");
        _activeItemUI = _activeItemUI != null ? _activeItemUI : GameObject.Find("ActiveItemImage");
        _activeItemGuageUI = _activeItemGuageUI != null ? _activeItemGuageUI : GameObject.Find("ActiveItemGuage");
        _ChipUI = _ChipUI != null ? _ChipUI : GameObject.Find("Frame_Chip");
        _bombUI = _bombUI != null ? _bombUI : GameObject.Find("Frame_Bomb");
        _inventoryUI = _inventoryUI != null ? _inventoryUI : GameObject.Find("Inventory_Window");
        _confirmWindowUI = _confirmWindowUI != null ? _confirmWindowUI : GameObject.Find("Confirm_Window");
        _optionWindowUI = _optionWindowUI != null ? _optionWindowUI : GameObject.Find("Option_Window");
        _deathWindowUI = _deathWindowUI != null ? _deathWindowUI : GameObject.Find("DeathPanel");
        _fortuneUI = _fortuneUI != null ? _fortuneUI : GameObject.Find("Fortune_Description");
        _itemInfoUI = _itemInfoUI != null ? _itemInfoUI : GameObject.Find("Item_Info");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 전환 {scene.name} UI 재참조");
        ReferenceUIReflesh();
        Init();

        InitDeathPanel();
    }

    // Death Panel 세팅 및 메인메뉴 전환
    private void InitDeathPanel()
    {
        _deathMainMenuButton.onClick.AddListener(() =>
        {
            PlayerStatManager.Instance._playerHp = 3;
            
            SceneManager.LoadScene("Real_MainMenu");
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Title);
        });
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
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OpenUi(UIKeyList.inventory);
                // 인벤토리 창
                _inventoryCloseButton.onClick.AddListener(() => UIManager.Instance.CloseUi());   // 인벤토리 창 닫음
                _optionWindowButton.onClick.AddListener(() => UIManager.Instance.OpenUi(UIKeyList.optionWindow));    // 옵션 창 열기
                _mainMenuButton.onClick.AddListener(() => UIManager.Instance.OpenUi(UIKeyList.confirmWindow));   // 확인 창 열기

                // 확인 창
                _yesButton.onClick.AddListener(() => SceneManager.LoadScene("Real_MainMenu"));    // Yes 버튼 : 메인 창 전환
                _noButton.onClick.AddListener(() => UIManager.Instance.CloseUi()); // No 버튼 : 인벤토리 창 전환

                // 옵션 창
                _optionCloseButton.onClick.AddListener(() => UIManager.Instance.CloseUi());  // 옵션 창 닫음 : 인벤토리 창 전환
            }
            else
            {
                Debug.LogWarning("GameSceneManager가 초기화되지 않았습니다.");
                return;
            }
        }
    }

    // 플레이어 체력 UI 업데이트
    public void HeartUpdateUI(float playerCurrentHp, float playerMaxHp)
    {
        _playerHeartController.HeartsUpdate(playerCurrentHp, playerMaxHp);
    }

    // 플레이어 스탯 UI 업데이트
    public void StatUpdateUI()
    {
        GameObject statUI = UIManager.instance.GetUI(UIKeyList.inventory);
        TMP_Text[] texts = statUI.GetComponentsInChildren<TMP_Text>();

        // UI 하위 텍스트에서 이름으로 설정
        TMP_Text speedText = texts.First(t => t.name == "SpeedValue");
        TMP_Text atkText = texts.First(t => t.name == "ATKValue");
        TMP_Text asText = texts.First(t => t.name == "ASValue");
        TMP_Text rangeText = texts.First(t => t.name == "RangeValue");
        TMP_Text luckText = texts.First(t => t.name == "LuckValue");
        
        // 설정된 텍스트에 플레이어 스텟 연결
        speedText.text = (PlayerStatManager.Instance._moveSpeed).ToString();
        atkText.text = (PlayerStatManager.Instance._attackDamage).ToString();
        asText.text = (PlayerStatManager.Instance._attackSpeed).ToString();
        rangeText.text = (PlayerStatManager.Instance._attackRange).ToString();
        luckText.text = (PlayerStatManager.Instance._playerLuck).ToString();
    }

    // 칩 획득량 UI 업데이트
    public void ChipUpdateUI()
    {
        GameObject curCoin = UIManager.Instance.GetUI(UIKeyList.Chip);
        TMP_Text curCoinCount = curCoin.GetComponentInChildren<TMP_Text>();
        curCoinCount.text = TempManager.inventory._coinCount.ToString();

    }

    // 폭탄 획득량 UI 업데이트
    public void BombUpdateUI()
    {
        GameObject curBomb = UIManager.Instance.GetUI(UIKeyList.bomb);
        TMP_Text curBombCount = curBomb.GetComponentInChildren<TMP_Text>();
        curBombCount.text = TempManager.inventory._bombCount.ToString();
    }

    // TODO : 아이템 사용 관련 Item Guage UI 확인용 테스트 함수
    public void GetItem()
    {
        // 아이템 획득
        if (Input.GetKeyDown(KeyCode.O))
        {
            _itemGuageController.GetItme();
            Debug.Log("아이템을 얻었습니다");
        }
    }

    // TODO : 아이템 사용 관련 Item Guage UI 확인용 테스트 함수
    public void UseItem()
    {
        // 아이템 게이지 애니메이션 동작 테스트
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_itemGuageController._canUseItem)
            {
                _itemGuageController.ItemUse();
            }
            else
            {
                Debug.Log("아이템을 사용할 수 없습니다");
            }
        }
    }



    // UI 열기
    public void OpenUi(UIKeyList uiName)
    {
        GameObject openUi = GetUI(uiName);
        if (openUi == null)
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
}
