using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SkillDictionarySO globalSkillDictionarySo;


    private void Awake()
    {
        globalSkillDictionarySo.Init();
        
        
    }

    public SkillDataSO GetGlobalSkill(string name)
    {
        return globalSkillDictionarySo.GetSkill(name);
    }
}
