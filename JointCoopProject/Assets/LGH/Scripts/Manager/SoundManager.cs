using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : TestSingleton<SoundManager>
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
        SFX_Dagger,
        SFX_Thunder,
        SFX_Flame,
        SFX_LaserMulti,
        SFX_SpinArrow,
        SFX_DashAttack,
        SFX_Parry,
        SFX_Predetor,
        SFX_Berserk,
        SFX_ExplosionShield,
        SFX_PickupHeart,
        SFX_PickupPenny,
        SFX_PickupBomb,
        SFX_SetBomb,
        SFX_BombExplode,
        SFX_PlayerAttack,
        SFX_PlayerDamage,
        SFX_GetItem,
        SFX_OrcAttack,
        SFX_OrcDie,
        SFX_SkeletonArcherAttack,
        SFX_SkeletonDie,
        SFX_DragonAttack,
        SFX_DragonDie,
        SFX_GolluxAttack,
        SFX_GolluxDie,
        SFX_GreatSwordSkeletonAttack,
        SFX_EliteOrcAttack,
        SFX_EliteOrcDie,
        SFX_WerewolfAttack,
        SFX_WerewolfDie,
        SFX_ShardSlayerAttack1,
        SFX_ShardSlayerAttack2,
        SFX_ShardSlayerDie,
        SFX_WarriorAttack1,
        SFX_WarriorAttack2,
        SFX_WarriorDie,
        SFX_InfectedGhostAttack,
        SFX_InfectedGhostDie,
        SFX_InfectedGoblinAttack,
        SFX_InfectedGoblinDie,
        SFX_OrcRiderThrow,
        SFX_IncubusAttack1,
        SFX_IncubusAttack2,
        SFX_IncubusDie,
        SFX_GoblinKingAttack1,
        SFX_GoblinKingAttack2,
        SFX_GoblinKingDie,
        SFX_StrayCatAttack1,
        SFX_StrayCatAttack2,
        SFX_StrayCatAttack3,
        SFX_StrayCatDie,
        SFX_CoinSlot,
        SFX_GachaSuccess,
        SFX_GachaFail
    }

    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    [SerializeField] public AudioSource audioBgm;
    [SerializeField] public AudioSource audioSfx;

    // TestSingleton 초기 설정
    private void Awake() => Init();

    private void Init()
    {
        base.SingletonInit();
    }
    // 합칠 때 이부분만 지워주시면 됩니다.

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

    public void BGM_Volume(float volume)
    {
        audioBgm.volume = volume;
    }

    public void SFX_Volume(float volume)
    {
        audioSfx.volume = volume;
    }
}
