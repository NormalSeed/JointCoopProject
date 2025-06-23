using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 현재 보유한 스킬 목록들을 보여줍니다.
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
   // 플레이어가 현재 보유 중인 스킬 목록
   private List<SkillDataSO> ownedSkills = new();

   
   /// <summary>
   /// 자동으로 발동되는 스킬 정보를 저장하는 클래스
   /// </summary>
   [System.Serializable]
   public class AutoSkill
   {
      public SkillDataSO skill;     // 자동 발동할 스킬 데이터
      public float cooldown = 5f;   // 쿨다운 간격 (초단위입니다!)
      public float timer;           // 현재 남은 시간 (Update에서 감소합니다)
   }
   
   // 자동 발동 스킬들의 리스트
   public List<AutoSkill> autoskills = new();

   
   /// <summary>
   /// 매 프레임마다 자동 스킬 타이머를 감소시키고 , 조건을 만족하면 스킬을 발동.
   /// </summary>
   private void Update()
   {
      foreach (var skill in autoskills)
      {
         skill.timer -= Time.deltaTime;
         if (skill.timer <= 0)
         {
            skill.skill.UseSkill(transform);
            skill.timer = skill.cooldown;
         }
      }
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
}
