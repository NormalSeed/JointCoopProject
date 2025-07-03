using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum EBgm
    {
        BGM_Title,
        BGM_Stage1,
        BGM_Stage2,
        BGM_Stage3,
        BGM_BossStage,
        BGM_LastBossStage,
        BGM_FortuneTellersShop,
        BGM_SecretRoom,
        BGM_OP_Audio,
        BGM_ED_Audio
    }

    public enum ESfx
    {
        SFX_GUNSHOT
    }

    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    [SerializeField] public AudioSource audioBgm;
    [SerializeField] public AudioSource audioSfx;

    public void PlayBGM(EBgm bgm)
    {
        audioBgm.clip = bgms[(int)bgm];
        audioBgm.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }

    public void PlaySFX(ESfx sfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)sfx]);
    }
}
