using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 획득시 스킬을 얻는 스크립트입니다.
/// </summary>
public class SkillPickupItem : MonoBehaviour
{
   public SkillDataSO skillToGive;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent(out PlayerSkillManager playerSkillManager))
      {
         playerSkillManager.AddSkill(skillToGive);
         Destroy(gameObject);
      }
   }
}
