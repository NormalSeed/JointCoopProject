using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossRoomLadderManager : MonoBehaviour
{
    [Header("보스 방 사물들 ")] public MonsterBase bossMonster;
    public SpriteRenderer ladder;

    public Vector3 ladderStartPos = new Vector3(7.5f, 11f, 0f);
    public Vector3 ladderEndPos = new Vector3(7.5f, 7.5f, 0f);

    public float ladderDropTime = 2f;

    public bool enablePlayerToggle = true;
    public float deactivateDelay = 1f;
    public float activateDelay = 1f;
    
    private bool ladderDropped = false;


    private void Start()
    {
        // 사다리 먼저 설정하고 
        // 보스방 연결하고 
        // 보스 죽었는지 확인
    }

    void SetupLadder()
    {
        if (ladder == null) return;

        ladder.gameObject.SetActive(false);
    }

    void SetupBoss()
    {
        if (bossMonster != null)
        {
            bossMonster.isBoss = true;
        }
    }

    IEnumerator CheckIsBossDead()
    {
        while (!ladderDropped)
        {
            yield return new WaitForSeconds(0.1f);

            if (bossMonster != null && bossMonster._isDead)
            {
                // 아니 맞잖아 보스몬스터 죽었는지 안죽었는지 확인하고 
                // 사다리 내리는거 코루틴 애니메이션 스근허이 내려와야하는데
                // 왜 애니메이션이 작동이 안하는거야 ? 
                // Debug 했짢아 다해줫짢아
                
                StartCoroutine(DropLadder());
                ladderDropped = true;
                yield break;
            }
        }
    }

    IEnumerator DropLadder()
    {
        yield return new WaitForSeconds(0.5f);

        ladder.gameObject.SetActive(true);
        ladder.transform.position = ladderStartPos;

        float elapsedTime = 0f;
        while (elapsedTime < ladderDropTime)
        {
            elapsedTime += Time.deltaTime;
            // smoothstep 에서 
            float lerpProgress = Mathf.SmoothStep(0f, 1f, elapsedTime / ladderDropTime);
            ladder.transform.position = Vector3.Lerp(ladderStartPos, ladderEndPos, lerpProgress);
            yield return null;
        }

        ladder.transform.position = ladderEndPos;
        
        //플레이어 토글 설정해보기
    }


    void SetupPlayerToggle()
    {
        // 콜라이더 붙여주는거하고 
        // 박스콜라이더도 설정해야하나 ? 
        // 여기에서 또 미리 설정해야하나 ? 
        
        //
    }
}