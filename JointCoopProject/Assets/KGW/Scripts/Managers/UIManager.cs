using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ��ųʸ� Ű �Է¿� ���� ��Ÿ ������ enum ���
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
    // UI�� Stack���� ����
    public Stack<GameObject> _UiStack = new Stack<GameObject>();

    [Header("InGame UI References")]
    [SerializeField] GameObject _miniMapUI;   // �̴ϸ� UI
    [SerializeField] GameObject _playerHpUI;    // �÷��̾� ü�� UI
    [SerializeField] GameObject _activeItemUI; // ��Ƽ�� ������ UI
    [SerializeField] GameObject _activeItemGuageUI; // ��Ƽ�� ������ UI
    [SerializeField] GameObject _ChipUI;        // ȹ���� ��� UI
    [SerializeField] GameObject _bombUI;        // ȹ���� ��ź UI
    [SerializeField] GameObject _inventoryUI;   // �κ��丮 UI
    [SerializeField] GameObject _confirmWindowUI;   // Ȯ�� â UI
    [SerializeField] GameObject _optionWindowUI;   // �ɼ� â UI
    [SerializeField] GameObject _deathWindowUI;   // ĳ���� ���� â UI
    [SerializeField] GameObject _fortuneUI;   // ������ � â UI
    [SerializeField] GameObject _itemInfoUI;   // ������ ���� â UI

    [Header("MainMenu UI References")]
    [SerializeField] GameObject _mainOptionUI;    // ���� �ɼ� â UI
    [SerializeField] GameObject _creditUI;      // ���� ũ���� UI

    [Header("Button UI Reference")]
    [SerializeField] Button _optionWindowButton;    // �ɼ� â ���� ��ư
    [SerializeField] Button _mainMenuButton;    // ���� ȭ�� �̵� ��ư
    [SerializeField] Button _inventoryCloseButton;    // �κ��丮 �ݱ� ��ư
    [SerializeField] Button _yesButton;    // Yes ��ư
    [SerializeField] Button _noButton;    // No ��ư
    [SerializeField] Button _optionCloseButton;    // �ɼ� �ݱ� ��ư
    [SerializeField] Button _deathMainMenuButton;    // ���� ȭ�� �̵� ��ư (���� ��)

    [Header("Player Heart Controll")]
    [SerializeField] PlayerHeartController _playerHeartController;  // �÷��̾� ��Ʈ ��Ʈ�� ��ũ��Ʈ
    [SerializeField] public ItemGuageController _itemGuageController;  // ������ ������ ��Ʈ�� ��ũ��Ʈ

    [Header("Active Item Setting")]
    [SerializeField] Image _activeItemImage;    // ��Ƽ�� ������ �̹���

    bool _isInventoryOpen = false;

    // �ʱ� UIManager ����
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // �� �̵� �� UIManager�� ������ None���� �Ǵ� �������� UIManager�� ���������� ����� ����
                GameObject UIManagerPrefab = Resources.Load<GameObject>("UIManager");

                if (UIManagerPrefab == null)
                {
                    Debug.LogError("UIManager �������� Resources/UIManager ���� ã�� �� �����ϴ�.");
                    return null;
                }

                GameObject gameObject = Instantiate(UIManagerPrefab);
                instance = gameObject.GetComponent<UIManager>();

                DontDestroyOnLoad(gameObject);
            }
            return instance;
        }
    }

    private void Start()
    {
        CreateUIManager();
        Init();
        // �� Change �� UI ������ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnInventoryOpen();
        }
        if (_playerHpUI != null)
        {
            HeartUpdateUI(PlayerStatManager.Instance._playerHp, PlayerStatManager.Instance._playerMaxHp);
        }
        // if (_activeItemGuageUI != null)
        // {
        //     UseItem();
        // }
        if (_ChipUI != null)
        {
            ChipUpdateUI();
        }
        if (_bombUI != null)
        {
            BombUpdateUI();
        }        
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
        if(instance == null)    // ������ �Ǿ� ������ �� ������ �ʰ� ���
        {
            instance = this;
            Init();
        }
        else if(instance != this)   // �ν����� �� ����
        {
            Destroy(gameObject);
        }
    }

    // ��ųʸ��� UI �߰� (��Ÿ ������ Ű == enum UIKeyList)
    private void Init()
    {
        // InGame UI �߰�
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

        // Main Menu UI �߰�
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
        _activeItemImage = _activeItemImage != null ? _activeItemImage : GameObject.Find("IconImage")?.GetComponent<Image>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"�� ��ȯ {scene.name} UI ������");
        ReferenceUIReflesh();
        Init();

        InitDeathPanel();
    }

    // Death Panel ���� �� ���θ޴� ��ȯ
    private void InitDeathPanel()
    {

        _deathMainMenuButton.onClick.AddListener(() =>
        {
            PlayerStatManager.Instance._playerHp = 6;

            SceneManager.LoadScene("Real_MainMenu");
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Title);
        });
    }

    // UI��������
    public GameObject GetUI(UIKeyList uiName)
    {
        GameObject uiKeyName = _UiDictionary.ContainsKey(uiName) ? _UiDictionary[uiName] : null;
        if (uiKeyName == null)
        {
            Debug.Log($"��û�Ͻ� UI({uiName})�� �������� �ʽ��ϴ�.");
        }

        return uiKeyName;
    }

    // �κ��丮 â ����
    public void OnInventoryOpen()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogWarning("GameSceneManager�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ����
        StatUpdateUI();

        // �κ��丮 ����
        UIManager.Instance.OpenUi(UIKeyList.inventory);

        // �κ��丮 ���� ����
        InventorySlotUpdateUI();

        if (!_isInventoryOpen)
        {
            // �κ��丮 â
            _inventoryCloseButton.onClick.AddListener(() => UIManager.Instance.CloseUi());   // �κ��丮 â ����
            _optionWindowButton.onClick.AddListener(() => UIManager.Instance.OpenUi(UIKeyList.optionWindow));    // �ɼ� â ����
            _mainMenuButton.onClick.AddListener(() => UIManager.Instance.OpenUi(UIKeyList.confirmWindow));   // Ȯ�� â ����

            // Ȯ�� â
            _yesButton.onClick.AddListener(() => SceneManager.LoadScene("Real_MainMenu"));    // Yes ��ư : ���� â ��ȯ
            _noButton.onClick.AddListener(() => UIManager.Instance.CloseUi()); // No ��ư : �κ��丮 â ��ȯ

            // �ɼ� â
            _optionCloseButton.onClick.AddListener(() => UIManager.Instance.CloseUi());  // �ɼ� â ���� : �κ��丮 â ��ȯ

            _isInventoryOpen = true;
        }
    }
    // �κ��丮 ���� �� ���� ����
    private void InventorySlotUpdateUI()
    {
        GameObject inventoryUI = UIManager.Instance.GetUI(UIKeyList.inventory);

        if (inventoryUI == null)
        {
            return;
        }

        InventorySlotController controller = inventoryUI.GetComponentInChildren<InventorySlotController>();

        if (controller != null)
        {
            controller.ItemSlotUIUpdate();
        }
        else
        {
            Debug.LogWarning("InventorySlotController�� ������� �ʾҽ��ϴ�.");
        }
    }

    // �÷��̾� ü�� UI ������Ʈ
    public void HeartUpdateUI(float playerCurrentHp, float playerMaxHp)
    {
        _playerHeartController.HeartsUpdate(playerCurrentHp, playerMaxHp);
    }

    // �÷��̾� ���� UI ������Ʈ
    public void StatUpdateUI()
    {
        GameObject statUI = UIManager.instance.GetUI(UIKeyList.inventory);
        TMP_Text[] texts = statUI.GetComponentsInChildren<TMP_Text>();

        // UI ���� �ؽ�Ʈ���� �̸����� ����
        TMP_Text speedText = texts.First(t => t.name == "SpeedValue");
        TMP_Text atkText = texts.First(t => t.name == "ATKValue");
        TMP_Text asText = texts.First(t => t.name == "ASValue");
        TMP_Text rangeText = texts.First(t => t.name == "RangeValue");
        TMP_Text luckText = texts.First(t => t.name == "LuckValue");
        
        // ������ �ؽ�Ʈ�� �÷��̾� ���� ����
        speedText.text = (PlayerStatManager.Instance._moveSpeed).ToString();
        atkText.text = (PlayerStatManager.Instance._attackDamage).ToString();
        asText.text = (PlayerStatManager.Instance._attackSpeed).ToString();
        rangeText.text = (PlayerStatManager.Instance._attackRange).ToString();
        luckText.text = (PlayerStatManager.Instance._playerLuck).ToString();
    }

    // Ĩ ȹ�淮 UI ������Ʈ
    public void ChipUpdateUI()
    {
        GameObject curCoin = UIManager.Instance.GetUI(UIKeyList.Chip);
        TMP_Text curCoinCount = curCoin.GetComponentInChildren<TMP_Text>();
        curCoinCount.text = ItemManager.inventory._coinCount.ToString();

    }

    // ��ź ȹ�淮 UI ������Ʈ
    public void BombUpdateUI()
    {
        GameObject curBomb = UIManager.Instance.GetUI(UIKeyList.bomb);
        TMP_Text curBombCount = curBomb.GetComponentInChildren<TMP_Text>();
        curBombCount.text = ItemManager.inventory._bombCount.ToString();
    }

    // ��Ƽ�� ������ ������ ����
    public void SetActiveItemImage(Sprite image)
    {
        if (_activeItemImage == null)
        {
            return;
        }

        _activeItemImage.sprite = image;
        _activeItemImage.enabled = image != null;   // �̹����� ������ �̹��� Ȱ��ȭ
    }

    // ��Ƽ�� ������ ���
    public void UseItem()
    {
        
        // ������ ������ �ִϸ��̼� ���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_itemGuageController._canUseItem)
            {
                Transform playerPos = GameObject.FindWithTag("Player").transform;   // �÷��̾� ��ġ
                ItemManager.inventory.UseActiveSkill(playerPos);
                
            }
            else
            {
                Debug.Log("�������� ����� �� �����ϴ�");
            }
        }
    }

    // UI ����
    public void OpenUi(UIKeyList uiName)
    {
        GameObject openUi = GetUI(uiName);
        if (openUi == null)
        {
            return;
        }

        if (_UiStack.Count > 0)
        {
            // �����ִ� UI�� ������ ����
            _UiStack.Peek().SetActive(false);
        }
        openUi.SetActive(true);
        _UiStack.Push(openUi);
    }

    // UI �ݱ�
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
