using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Vector3 = UnityEngine.Vector3;


public class BossRoomLadderManager : MonoBehaviour
{
    [Header("보스 방 사물들 ")] public MonsterBase bossMonster;
    public SpriteRenderer ladder;

    public Vector3 ladderStartPos = new Vector3(0f, 6.5f, 0f);
    public Vector3 ladderEndPos = new Vector3(0f, 3f, 0f);

    public float ladderDropTime = 2f;

    public bool useNextScene = true;
    public string targetSceneName = "";
    
    private bool ladderDropped = false;


    private void Start()
    {
        SetupLadder();
        
        SetupBoss();

        StartCoroutine(CheckIsBossDead());
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
                // Debug에서도 된다 했짢아 다해줫짢아
                
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
        
        // 보스룸 기준으로 월드 좌표 계산
        
        Vector3 roomPosition = transform.position;
        Vector3 startPos = roomPosition + ladderStartPos;
        Vector3 endPos = roomPosition + ladderEndPos;
        

        ladder.transform.position = startPos;
        

        float elapsedTime = 0f;
        while (elapsedTime < ladderDropTime)
        {
            elapsedTime += Time.deltaTime;
            // smoothstep 에서 츤츤히 가기.
            float lerpProgress = Mathf.SmoothStep(0f, 1f, elapsedTime / ladderDropTime);
            ladder.transform.position = Vector3.Lerp(startPos, endPos, lerpProgress);
            yield return null;
        }

        ladder.transform.position = endPos;
    }
    
}