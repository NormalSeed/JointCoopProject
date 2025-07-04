using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 딕셔너리 키 입력에 대한 오타 방지로 enum 사용
public enum UIKeyList
{
    // MainMenu UIKeyList
    mainOption, credit,
    // InGame UIKeyList
    playerHp, activeSkill, gold, bomb, inventory, confirmWindow, optionWindow, deathWindow, fortune, itemTitle, itemDescription
}

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    Dictionary<UIKeyList, GameObject> _UiDictionary = new Dictionary<UIKeyList, GameObject>();

    [Header("InGame UI References")]
    [SerializeField] GameObject _playerHpUI;    // 플레이어 체력 UI
    [SerializeField] GameObject _activeSkillUI; // 액티브 아이템 UI
    [SerializeField] GameObject _goldUI;        // 획득한 골드 UI
    [SerializeField] GameObject _bombUI;        // 획득한 폭탄 UI
    [SerializeField] GameObject _inventoryUI;   // 인벤토리 UI
    [SerializeField] GameObject _confirmWindowUI;   // 확인창 UI
    [SerializeField] GameObject _optionWindowUI;   // 옵션창 UI
    [SerializeField] GameObject _deathWindowUI;   // 캐릭터 죽음창 UI
    [SerializeField] GameObject _fortuneUI;   // 오늘의 운세창 UI
    [SerializeField] GameObject _itemTitleUI;   // 아이템 타이틀 UI
    [SerializeField] GameObject _itemDescriptionUI;   // 아이템 설명 UI

    [Header("MainMenu UI References")]
    [SerializeField] GameObject _mainOptionUI;    // 메인 옵션 창 UI
    [SerializeField] GameObject _creditUI;      // 메인 크래딧 UI

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
        _UiDictionary[UIKeyList.playerHp] = _playerHpUI;
        _UiDictionary[UIKeyList.activeSkill] = _activeSkillUI;
        _UiDictionary[UIKeyList.gold] = _goldUI;
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

    // 
    public GameObject GetUI(UIKeyList uiName)
    {
        GameObject uiKeyName = _UiDictionary.ContainsKey(uiName) ? _UiDictionary[uiName] : null;
        if(uiKeyName == null)
        {
            Debug.Log($"요청하신 UI({uiName})가 존재하지 않습니다.");
        }
        
        return uiKeyName;
    }
    

}
