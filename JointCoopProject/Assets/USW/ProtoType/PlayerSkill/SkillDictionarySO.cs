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
    
    // 조회를 위해 사용하는 딕셔너리 (key - skill name, value - SkillDataSO 
    private Dictionary<string, SkillDataSO> _skilldictionary;

    
    /// <summary>
    /// skillList를 바탕으로 딕셔너리 초기화
    /// </summary>
    public void Init()
    {
        _skilldictionary = skillList.ToDictionary(s=>s.skillName, s=>s);
    }

    
    /// <summary>
    /// 스킬 이름으로 SkillDataSO 조회함.
    /// </summary>
    /// <param name="찾고자 하는 스킬의 이름"></param>
    /// <returns>해당 이름의 SkillDataSO 객체</returns>
    public SkillDataSO GetSkill(string name)
    {
        if(_skilldictionary ==null) Init();
        return _skilldictionary.TryGetValue(name, out var skill) ? skill : null;
    }
}
