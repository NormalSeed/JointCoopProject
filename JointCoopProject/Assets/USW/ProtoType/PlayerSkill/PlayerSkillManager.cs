using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 현재 보유한 스킬 목록들을 보여줍니다.
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
   private List<SkillDataSO> ownedSkills = new();

   [System.Serializable]
   public class AutoSkill
   {
      public SkillDataSO skill;
      public float cooldown = 5f;
      public float timer;
   }
   
   public List<AutoSkill> autoskills = new();

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


   public void AddSkill(SkillDataSO newSkill)
   {
      // 여기에다가 contains 써서 ? 
      // add newskill 느낌으로다가 리스트에 집어넣어주는거지
      // 어쩌피 뺼것도 아니라서 대충 짜도 되려나 
      if (!ownedSkills.Contains(newSkill))
      {
         ownedSkills.Add(newSkill);
      }
   }

   public void UseSkill(string skillName)
   {
      var skill = ownedSkills.Find(s=>s.skillName == skillName);

      if (skill != null)
      {
         // 만약에 Input 넣으면 ? useskill 로 문자열찾고 그다음에 발동위치 넘겨준다는 마인드인데 ? 왜 transform 이 안되지 ? 
         skill.UseSkill(transform);
      }
   
   }
}
