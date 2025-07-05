using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하려면 이 네임스페이스가 필요합니다.

public class LoadingTextAnimatorInfinite : MonoBehaviour
{
    public TextMeshProUGUI loadingText; // 인스펙터에서 TMP Text 컴포넌트를 연결할 변수
    public float delayBetweenDots = 0.2f; // 점이 나타나는 간격 (초)

    private string[] loadingMessages = { "Now Loading", "Now Loading.", "Now Loading..", "Now Loading..." };
    private int currentMessageIndex = 0;

    void Start()
    {
        // 로딩 텍스트 컴포넌트가 연결되었는지 확인
        if (loadingText == null)
        {
            Debug.LogError("Loading Text (TextMeshProUGUI)가 연결되지 않았습니다! 인스펙터에서 연결해주세요.");
            enabled = false; // 스크립트 비활성화
            return;
        }

        // 로딩 애니메이션 무한 반복 시작
        StartCoroutine(AnimateLoadingText());
    }

    // 로딩 텍스트 애니메이션 코루틴 (무한 반복)
    IEnumerator AnimateLoadingText()
    {
        while (true) // 이 루프는 스크립트가 비활성화되거나 게임 오브젝트가 파괴될 때까지 계속됩니다.
        {
            // 현재 메시지 인덱스의 텍스트를 표시
            loadingText.text = loadingMessages[currentMessageIndex];

            // 다음 메시지로 인덱스 업데이트 (0 -> 1 -> 2 -> 0 -> ...)
            currentMessageIndex = (currentMessageIndex + 1) % loadingMessages.Length;

            // 지정된 딜레이만큼 기다립니다.
            yield return new WaitForSeconds(delayBetweenDots);
        }
    }
}