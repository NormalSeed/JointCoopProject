using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public enum SlotResult
{ 
    Item, Coin, Fail
}
public class SlotMachine : MonoBehaviour
{
    [SerializeField]
    [Tooltip("1회당 사용료를 지정합니다.(단위: 1코인)")]
    private int fee;

    [SerializeField]
    [Tooltip("당첨시 결과 Object가 발생할 위치를 지정합니다.")]
     private Transform _dropPoint;

    [Header("Result Items Info")]
    [SerializeField]
    [Tooltip("당첨시 지급되는 아이템의 목록을 지정합니다.")]
    private GameItem[] _itemArray; // index를 사용하기 위해 배열 사용.(Intialize 이후 멤버의 추가 및 삭제가 발생하지 않음)

    [Space(10)]
    [SerializeField]
    [Tooltip("당첨시 지급되는 코인의 Object를 지정합니다.")]
     private GameItem _coinPrefab;

    [SerializeField]
    [Tooltip("코인 당첨시 지급되는 최저량을 지정합니다.(단위: 1코인)")]
    private int _minCoin;

    [SerializeField]
    [Tooltip("코인 당첨시 지급되는 최고량을 지정합니다.(단위: 1코인)")]
    private int _maxCoin;

    private const int ITEM_PROBABILITY = 12;
    private const int COIN_PROBABILITY = 18;

    private SlotResult _slotResult;
    private System.Random _randomInstance;

    private void Awake()
    {
        CheckValidation();
        Init();
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 슬롯머신 애니메이션 재생(있다면)
            if (CheckPayFee())
            {
                _slotResult = GetRandomSlotResult();
                switch (_slotResult)
                {
                    case SlotResult.Item:
                        GetRandomItem();
                        break;
                    case SlotResult.Coin:
                        GetRandomCoin();
                        break;
                    case SlotResult.Fail:
                        // 꽝 애니메이션 재생(있다면)
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void Init()
    {
        _randomInstance = new System.Random((int)GetUnixTimeStamp());
    }
    private void CheckValidation()
    {
        if (_minCoin > _maxCoin)
        { 
            throw new Exception("Invalid value specified: _minCoin, _maxCoin");
        }
        
    }
    private bool CheckPayFee()
    {
        // return TempManager.inventory._coin >= fee;
        return true; // For Test
    }
    private SlotResult GetRandomSlotResult()
    {
        int randomNumber = _randomInstance.Next(0, 100);
        if (randomNumber < ITEM_PROBABILITY)
        {
            return SlotResult.Item;
        }
        else if (randomNumber < (ITEM_PROBABILITY + COIN_PROBABILITY))
        {
            return SlotResult.Coin;
        }
        else
        {
            return SlotResult.Fail;
        }
    }
    private void GetRandomItem()
    {
        int randomItemIndex = _randomInstance.Next(0, _itemArray.Length);
        _itemArray[randomItemIndex].Drop(_dropPoint);
    }
    private void GetRandomCoin()
    {
        int randomCoinCount = _randomInstance.Next(_minCoin, _maxCoin + 1);
        for (int cnt = 0; cnt < randomCoinCount; cnt++)
        {
            _coinPrefab.Drop(_dropPoint);
        }
    }
    private static float GetUnixTimeStamp()
    {
        TimeSpan timestamp = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        return (float)timestamp.TotalSeconds;
    }
}
