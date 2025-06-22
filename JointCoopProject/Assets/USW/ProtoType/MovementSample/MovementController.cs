using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    
    [SerializeField]
    private List<MonoBehaviour> movementScripts;

    // 현재 활성화된 스크립트의 인덱스를 추적
    private int activeScriptIndex = -1;
    
    

    void Start()
    {
        // 게임 시작 시 모든 움직임 스크립트를 일단 비활성화합니다.
        DeactivateAllMovementScripts();

        // 만약 할당된 스크립트가 있다면 첫 번째 스크립트를 기본으로 활성화
        if (movementScripts != null && movementScripts.Count > 0)
        {
            SetActiveMovementScript(0); // 0번 인덱스의 스크립트 활성화
        }
    }

    // 모든 움직임 스크립트를 비활성화하는 내부 함수
    private void DeactivateAllMovementScripts()
    {
        if (movementScripts == null) return;

        foreach (MonoBehaviour script in movementScripts)
        {
            if (script != null)
            {
                script.enabled = false; // 스크립트의 'enabled' 속성을 false로 설정하여 비활성화
            }
        }
    }

    /// <summary>
    /// 특정 인덱스의 움직임 스크립트를 활성화하고, 다른 모든 스크립트를 비활성화
    /// 이 함수를 각 버튼에 연결
    /// </summary>
    public void SetActiveMovementScript(int indexToActivate)
    {
        
        DeactivateAllMovementScripts();

        // 지정된 스크립트를 활성화
        MonoBehaviour scriptToActivate = movementScripts[indexToActivate];
        if (scriptToActivate != null)
        {
            scriptToActivate.enabled = true; // 스크립트 활성화
            activeScriptIndex = indexToActivate; // 활성화된 스크립트 인덱스 업데이트
        }
       
    }

    /// <summary>
    /// 다음 움직임 스크립트로 순환하여 전환
    /// </summary>
    public void ActivateNextMovementScript()
    {
        int nextIndex = (activeScriptIndex + 1) % movementScripts.Count;
        SetActiveMovementScript(nextIndex);
    }
}
    

