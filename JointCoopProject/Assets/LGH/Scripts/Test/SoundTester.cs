using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundTester : MonoBehaviour
{
    private SoundManager _soundManager;

    private void Awake() => Init();

    private void Init()
    {
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Start()
    {
        _soundManager.PlayBGM(SoundManager.EBgm.BGM_Stage1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _soundManager.PlaySFX(SoundManager.ESfx.SFX_PlayerAttack);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            float volume = _soundManager.audioBgm.volume;
            _soundManager.BGM_Volume(volume -0.1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            float volume = _soundManager.audioBgm.volume;
            _soundManager.BGM_Volume(volume + 0.1f);
        }
    }
}
