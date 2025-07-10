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
    bool _isNoneCoin = false;

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
        // ������ ������, ������ BGM ON
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
        _isContact = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹ü�� �÷��̾����� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            if (InventoryManager.GetInstance()._coinCount < 5)
            {
                _isNoneCoin = true;
                OnFortuneUiOpen();
                return;
            }

            // �ѹ� ���������� ������ �Ұ�
            if (_isContact)
            {
                OnFortuneUiOpen();
                return;
            }

            InventoryManager.GetInstance().UseCoin(5);

            Debug.Log("���� ����");
            // ���� ���� �귿 ����
            IBuff randomBuff = BuffCreateFactory.BuffRoulette();
            // ���� ����
            BuffManager.Instance.ApplyBuff(randomBuff, PlayerStatManager.Instance);

            // ������ �ް� ���������� ������� �ʰ� ���������� �ٽ� �������ص� ��ȣ�ۿ� ����
            _isContact = true;

            // ���� ���� ���
            if(randomBuff._buffType == BuffType.Fortune)
            {
                OnFortuneUiOpen();
            }

            // ���˽� ����� ����Ʈ
            // �ູ ����Ʈ
            if (randomBuff._buffCategory == BuffCategory.Blessing)
            {
                if (randomBuff._buffLevel == 1 ||  randomBuff._buffLevel == 2)
                {
                    _effectAni.Play(Bless_Common_Effect);   // �ູ Level1,2 ����Ʈ
                }
                else if (randomBuff._buffLevel == 3 || randomBuff._buffLevel == 4)
                {
                    _effectAni.Play(Bless_Rare_Effect);     // �ູ Level3,4 ����Ʈ
                }
                else
                {
                    _effectAni.Play(Bless_Legend_Effect);   // �ູ Level5 ����Ʈ
                }
            }
            // ���� ����Ʈ
            if (randomBuff._buffCategory == BuffCategory.Curs)
            {
                if (randomBuff._buffLevel == 1 || randomBuff._buffLevel == 2)
                {
                    _effectAni.Play(Curs_Common_Effect);    // ���� Level1,2 ����Ʈ
                }
                else if (randomBuff._buffLevel == 3 || randomBuff._buffLevel == 4)
                {
                    _effectAni.Play(Curs_Rare_Effect);      // ���� Level3,4 ����Ʈ
                }
                else
                {
                    _effectAni.Play(Curs_Legend_Effect);    // ���� Level5 ����Ʈ
                }
            }
            // �� ����Ʈ
            if(randomBuff._buffCategory == BuffCategory.Nothing)
            {
                _effectAni.Play(Nothing_Effect);            // �� ����Ʈ
            }
        }
    }

    // � UI ����
    private void OnFortuneUiOpen()
    {
        // Ÿ�̸� ����
        _timer = _FortuneTextTime;
        // � UI ����
        _isFortuneUiOpen = true;

        UIManager.Instance.CloseUi();
        UIManager.Instance.OpenUi(UIKeyList.fortune);
        GameObject fortuneUi = UIManager.Instance.GetUI(UIKeyList.fortune);
        
        if (fortuneUi != null)
        {
            TMP_Text fortuneText = fortuneUi.GetComponentInChildren<TMP_Text>(true);    // true�� �ؾ� ��Ȱ��ȭ ���¿����� ã�Ƽ� �����ü� �ִ�.

            if (_isNoneCoin)
            {
                fortuneText.text = "���� �ְ� ���� ���޶�� �ϰŶ�~!";
                _isNoneCoin = false;
            }
            else if (_isContact && !_isNoneCoin)
            {
                fortuneText.text = "�̹� ������ �޾��ݴ�?? ������̱���?";
            }
            else if (!_isNoneCoin && !_isContact)
            {
                fortuneText.text = BuffManager.Instance._FortuneExplanation;
            }
        }      
    }

    // ������ �ð��� ������ � UI ����
    private void OnFortuneUiClose()
    {
        UIManager.Instance.CloseUi();
        _isFortuneUiOpen = false;
    }
}
