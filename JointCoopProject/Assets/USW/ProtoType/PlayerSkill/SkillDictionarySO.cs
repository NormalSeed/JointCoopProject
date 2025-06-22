using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 모든 스킬이 담겨져있는 Dictionary 라고 생각하시면 됩니다 ( ScriptableObject )
/// </summary>
[CreateAssetMenu(menuName = "PlayerSkill/SkillDictionarySO")]
public class SkillDictionarySO : ScriptableObject
{
    public List<SkillDataSO> skillList;

    private Dictionary<string, SkillDataSO> _skilldictionary;

    public void Init()
    {
        _skilldictionary = skillList.ToDictionary(s=>s.skillName, s=>s);
    }

    public SkillDataSO GetSkill(string name)
    {
        if(_skilldictionary ==null) Init();
        return _skilldictionary.TryGetValue(name, out var skill) ? skill : null;
    }
}
