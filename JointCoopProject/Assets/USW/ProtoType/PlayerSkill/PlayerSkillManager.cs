using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 플레이어가 현재 보유한 스킬 목록들을 보여줍니다.
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
    // 플레이어가 현재 보유 중인 스킬 목록
    private List<SkillDataSO> ownedSkills = new();
    [SerializeField] PlayerMovement _playerMove;

    public bool _isAttack = false;

   
    /// <summary>
    /// 자동으로 발동되는 스킬 정보를 저장하는 클래스
    /// </summary>
    [System.Serializable]
    public class AutoSkill
    {
        public SkillDataSO skill;     // 자동 발동할 스킬 데이터
        public float cooldown = 5f;   // 쿨다운 간격 (초단위입니다!)
        public float timer;           // 현재 남은 시간
    }

    /// <summary>
    /// 기존 검 공격 강화 스킬 정보를 저장하는 클래스
    /// </summary>
    [System.Serializable]
    public class SwordUpgradeSkill
    {
        public SkillDataSO swordSkill;  // 강화할 스킬 데이터
    }

    // 자동 발동 스킬들의 리스트
    public List<AutoSkill> autoskills = new();
    // 기본 공격 강화 스킬들의 리스트
    public List<SwordUpgradeSkill> swordUpgradeskills = new();

   
    /// <summary>
    /// 매 프레임마다 자동 스킬 타이머를 감소시키고 , 조건을 만족하면 스킬을 발동.
    /// </summary>
    private void Update()
    {
        AutoSkillAttack();
    }
   
    /// <summary>
    /// 스킬 이름으로 스킬을 찾아서 발동함.
    /// </summary>
    /// <param name="skillName">발동하려는 스킬 이름</param>
    public void AddSkill(SkillDataSO newSkill)
    {
        if (!ownedSkills.Contains(newSkill))
        {
            ownedSkills.Add(newSkill);
        }
        // 획득한 스킬이 기본 공격 강화 스킬이면 리스트에 추가
        if (newSkill._isSwordAttack)
        {
            SwordUpgradeSkill swordUpgradeSkill = new SwordUpgradeSkill()
            {
                swordSkill = newSkill
            };

            swordUpgradeskills.Add(swordUpgradeSkill);
            Debug.Log($"공격 강화 스킬 {newSkill.skillName} 추가됨");
        }
        else    // 공격 강화 스킬이 아니면 패시브 리스트에 추가
        {
            AutoSkill autoSkill = new AutoSkill()
            {
                skill = newSkill
            };

            autoskills.Add(autoSkill);
            Debug.Log($"패시브 스킬 {newSkill.skillName} 추가됨");
        }
    }
   
    /// <summary>
    /// 스킬 이름으로 스킬을 찾아서 발동합니다.
    /// </summary>
    /// <param name="skillName">발동하고자 하는 스킬 이름</param>
    public void UseSkill(string skillName)
    {

        // 보유 스킬 목록에서 이름이 일치하는 스킬을 찾음.
        var skill = ownedSkills.Find(s=>s.skillName == skillName);
      
        if (skill != null)
        {
            // 만약에 Input 넣으면 ? useskill 로 문자열찾고 그다음에 발동위치 넘겨준다는 마인드인데 ? 왜 transform 이 안되지 ? 
            skill.UseSkill(transform);
        }
   
    }

    /// <summary>
    /// 플레이어의 패시브 스킬 공격
    /// </summary>
    public void AutoSkillAttack()
    {
        foreach (var skill in autoskills)
        {
            skill.timer -= Time.deltaTime;
            if (skill.timer <= 0)
            {
                if (!skill.skill._isTrace)
                {
                    Vector3 throwDir = _playerMove._moveInput;
                    // 플레이어가 움직이지 않으면 오른쪽으로 공격함
                    if (throwDir == Vector3.zero)
                    {
                        throwDir = Vector3.right;
                    }
                    // 플레이어 방향으로 스킬 사용
                    skill.skill.UseSkill(transform, throwDir);
                }
                else
                {
                    // 적 추적 스킬 사용
                    skill.skill.UseSkill(transform);
                }
                // 스킬 쿨타임
                skill.timer = skill.cooldown;
            }
        }
    }

    /// <summary>
    /// 플레이어의 기존 검 공격 강화 스킬
    /// </summary>
    public void SwordUpgradeAttack()
    {
        Debug.Log("근접공격 맞음");
        foreach (var skill in swordUpgradeskills)
        {
            Vector3 attackDir = _playerMove._attackDirection;

            skill.swordSkill.UseSkill(transform, attackDir);
        }
    }
}

