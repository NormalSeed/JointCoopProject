using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeartController : MonoBehaviour
{
    [Header("Player Heart UI")]
    [SerializeField] Sprite _fullHeart;  // 가득찬 하트 (풀하트)
    [SerializeField] Sprite _halfHeart;  // 반 하트 (하프 하트)    
    [SerializeField] Sprite _emptyHeart;  // 빈 하트

    [Header("Heart Object Reference")]
    [SerializeField] List<Image> _heartList = new List<Image>();    // Heart Object 연결 리스트

    public void HeartsUpdate(float playerCurrentHp, float playerMaxHp)
    {
        for(int i = 0; i < _heartList.Count; i++)
        {
            float heartstate = Mathf.Clamp(playerCurrentHp - i, 0f, 1f);

            if (i < Mathf.CeilToInt(playerMaxHp))
            {
                _heartList[i].gameObject.SetActive(true);

                if (heartstate >= 1f)   // 하트의 상태가 풀하트
                {
                    _heartList[i].sprite = _fullHeart;
                }
                else if (heartstate == 0.5f)    // 하트의 상태가 하프 하트
                {
                    _heartList[i].sprite = _halfHeart;
                }
                else   // 하트의 상태가 빈 하트
                {
                    _heartList[i].sprite = _emptyHeart;
                    _heartList[i].gameObject.SetActive(false);
                }
            }
            else
            {
                _heartList[i].gameObject.SetActive(false);
            }      
        }      
    }
}
