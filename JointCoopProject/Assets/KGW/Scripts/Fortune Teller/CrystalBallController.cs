using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalBallController : MonoBehaviour
{
    [SerializeField] ChangeSceneManager _changeSceneManager;
    [SerializeField] Animator _effectAni;
    [SerializeField] float _FortuneTextTime = 3f;

    bool _isContact = false;
    float _timer;
    bool _isFortuneUiOpen = false;

    readonly int Bless_Common_Effect = Animator.StringToHash("BlessCommonEffect");
    readonly int Bless_Rare_Effect = Animator.StringToHash("BlessRareEffect");
    readonly int Bless_Legend_Effect = Animator.StringToHash("BlessLegendEffect");
    readonly int Curs_Common_Effect = Animator.StringToHash("CursCommonEffect");
    readonly int Curs_Rare_Effect = Animator.StringToHash("CursRareEffect");
    readonly int Curs_Legend_Effect = Animator.StringToHash("CursLegendEffect");
    readonly int Nothing_Effect = Animator.StringToHash("NothingEffect");

    private void Awake()
    {
        _timer = _FortuneTextTime;
    }

    private void OnEnable()
    {
        // 생성시 점술가, 상점방 BGM ON
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_FortuneTellersShop);
    }

    private void Update()
    {
        if (_isFortuneUiOpen)
        {
            _timer -= Time.deltaTime;

            if ( _timer < 0)
            {
                OnFortuneUiClose();
            }
        }        
    }

    private void OnDisable()
    {
        switch (_changeSceneManager._CursceneIndex)
        {
            case 2:
            case 3:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage1);    // Stage1,2 BGM ON
                break;
            case 4:
            case 5:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage2);    // stage3,4 BGM ON
                break;
            case 6:
            case 7:
                SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage3);    // stage5,6 BGM ON
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체가 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 한번 접촉했으면 재접촉 불가
            if (_isContact)
            {
                // TODO : 사용했다는 문구 출력?
                Debug.Log("이미 버프를 받았습니다.");
                return;
            }

            Debug.Log("버프 생성");
            // 랜덤 버퍼 룰렛 생성
            IBuff randomBuff = BuffCreateFactory.BuffRoulette();
            // 버퍼 적용
            BuffManager.Instance.ApplyBuff(randomBuff, PlayerStatManager.Instance);
            // 버프를 받고 수정구슬은 사라지지 않고 남아있지만 다시 접촉을해도 상호작용 없음
            //_isContact = true;

            // 예언 문구 출력
            if(randomBuff._buffType == BuffType.Fortune)
            {
                OnFortuneUiOpen();
            }

            // 접촉시 사운드와 이펙트
            // 축복 이펙트
            if (randomBuff._buffCategory == BuffCategory.Blessing)
            {
                if (randomBuff._buffLevel == 1 ||  randomBuff._buffLevel == 2)
                {
                    _effectAni.Play(Bless_Common_Effect);   // 축복 Level1,2 이펙트
                }
                else if (randomBuff._buffLevel == 3 || randomBuff._buffLevel == 4)
                {
                    _effectAni.Play(Bless_Rare_Effect);     // 축복 Level3,4 이펙트
                }
                else
                {
                    _effectAni.Play(Bless_Legend_Effect);   // 축복 Level5 이펙트
                }
            }
            // 저주 이펙트
            if (randomBuff._buffCategory == BuffCategory.Curs)
            {
                if (randomBuff._buffLevel == 1 || randomBuff._buffLevel == 2)
                {
                    _effectAni.Play(Curs_Common_Effect);    // 저주 Level1,2 이펙트
                }
                else if (randomBuff._buffLevel == 3 || randomBuff._buffLevel == 4)
                {
                    _effectAni.Play(Curs_Rare_Effect);      // 저주 Level3,4 이펙트
                }
                else
                {
                    _effectAni.Play(Curs_Legend_Effect);    // 저주 Level5 이펙트
                }
            }
            // 꽝 이펙트
            if(randomBuff._buffCategory == BuffCategory.Nothing)
            {
                _effectAni.Play(Nothing_Effect);            // 꽝 이펙트
            }
        }
    }

    // 운세 UI 열림
    private void OnFortuneUiOpen()
    {
        // 타이머 리셋
        _timer = _FortuneTextTime;
        // 운세 UI 열림
        _isFortuneUiOpen = true;

        UIManager.Instance.OpenUi(UIKeyList.fortune);
        GameObject fortuneUi = UIManager.Instance.GetUI(UIKeyList.fortune);
        
        if (fortuneUi != null)
        {
            TMP_Text fortuneText = fortuneUi.GetComponentInChildren<TMP_Text>(true);    // true로 해야 비활성화 상태에서도 찾아서 가져올수 있다.

            if (fortuneText != null)
            {
                fortuneText.text = BuffManager.Instance._FortuneExplanation;
            }
        }      
    }

    // 지정된 시간이 지나면 운세 UI 닫힘
    private void OnFortuneUiClose()
    {
        UIManager.Instance.CloseUi();
        _isFortuneUiOpen = false;
    }
}
