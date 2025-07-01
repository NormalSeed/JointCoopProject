using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 정보를 담는 역할을 합니다.
/// 확장성을 위해 스킬데이터 스크립트에이블오브젝트를 생성하였습니다.
/// </summary>

[CreateAssetMenu(menuName = "PlayerSkill/Skill DataSO")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public float skillCooldown;
    public float skillPossibility;
    public Sprite skillIcon;
    public bool _isTrace;
    //public string skillDescription;
    public GameObject skillPrefab;
    
    // 위치만 설정
    public virtual void UseSkill(Transform caster)
    {
        GameObject skillInstance = Instantiate(this.skillPrefab); 
    }

    // 이동 방향 필요 시 설정
    public virtual void UseSkill(Transform caster, Vector3 direction)
    {
        GameObject skillInstance = Instantiate(this.skillPrefab);
    }
}

