using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SlotResult
{ 
    Item, Coin, Fail
}
public class SlotMachine : MonoBehaviour
{
    [SerializeField] private int fee;
    [SerializeField] private GameItem[] _itemArray; // index를 사용하기 위함(Intialize 이후 멤버의 추가 및 삭제가 발생하지 않음)
    [SerializeField] private int _minCoin;
    [SerializeField] private int _maxCoin;
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private GameItem _coinPrefab;
    private SlotResult _slotResult;
    private const int ITEM_PROBABILITY = 12;
    private const int COIN_PROBABILITY = 30; // 18
    private System.Random _randomInstance = new System.Random((int)GetUnixTimeStamp());

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
        else if (randomNumber < COIN_PROBABILITY)
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
