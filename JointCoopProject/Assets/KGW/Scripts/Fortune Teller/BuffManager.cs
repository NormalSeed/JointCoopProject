using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    static BuffManager instance;

    public static BuffManager Instance  // 프로퍼티로 접근
    {
        get
        {
            if (instance == null)   // 초기에 BuffManager가 없으면 생성
            {
                GameObject gameObject = new GameObject("BuffManager");
                instance = gameObject.AddComponent<BuffManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        CreateBuffManager();
    }

    private void CreateBuffManager()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 게임 종료까지 데이터 저장
        }
        else
        {
            Destroy(gameObject);    // 중복으로 생성이 안되도록 삭제
        }
    }

    List<IBuff> _applyBuffList = new List<IBuff>();
    public string _FortuneExplanation;

    public void ApplyBuff(IBuff applyBuff, PlayerStatManager playerStatus)
    {
        // 꽝이면 리턴
        if (applyBuff._buffCategory == BuffCategory.Nothing)
        {
            Debug.Log($"어머~~~?? 꽝이네요 >3<");
            return;
        }

        _applyBuffList.Add(applyBuff);  // 적용된 버퍼 리스트에 적용 버퍼 추가
        applyBuff.BuffReceive(playerStatus);    // 버프를 플레이어에 적용
    }

    public void FortuneConfirm(string explanation)
    {
        _FortuneExplanation = explanation;
    }

    public void ClearBuff()
    {
        _applyBuffList.Clear();
        Debug.Log("모든 버퍼 초기화 완료!!");

        
    }
}
