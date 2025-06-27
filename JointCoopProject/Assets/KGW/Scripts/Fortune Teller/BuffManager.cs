using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private static BuffManager instance;

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

    private List<IBuff> _applyBuffList = new List<IBuff>();

    public void ApplyBuff(IBuff applyBuff, PlayerStatus playerStatus)
    {
        // 꽝이면 리턴
        if (applyBuff._buffCategory == BuffCategory.Nothing)
        {
            Debug.Log($"어머~~~?? 꽝이네요 >3<");
            return;
        }

        _applyBuffList.Add(applyBuff);  // 적용된 버퍼 리스트에 적용 버퍼 추가
        applyBuff.BuffReceive(playerStatus);    // 버프를 플레이어에 적용
        Debug.Log($"띠링~~ 띠링~~!! 플레이어가 {applyBuff._buffLevel} {applyBuff._buffName}를 받았습니다!!.");
        Debug.Log($"받은 버프는 플레이어의 {applyBuff._buffDescription}");
    }

    public void ClearBuff()
    {
        _applyBuffList.Clear();
        Debug.Log("모든 버퍼 초기화 완료!!");
    }
}
