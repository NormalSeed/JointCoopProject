using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/Item Level Up")]
public class ItemLevelUpSkillSO : SkillDataSO
{
    private System.Random _randomInstance = new System.Random((int)ItemUtil.GetUnixTimeStamp());
    
    public override void UseSkill(Transform caster, out bool useResult)
    {
        ItemSlot[] _upgradeSkillList = GetUpgradeSkillList();
        if (_upgradeSkillList.Length > 0)
        {
            int randomItemIndex = _randomInstance.Next(0, _upgradeSkillList.Length);
            _upgradeSkillList[randomItemIndex].UpgradeStackCount();
            useResult = true;
        }
        else
        {
            useResult = false;
        }

    }
    public ItemSlot[] GetUpgradeSkillList()
    {
        List<ItemSlot> returnList = new();
        foreach (ItemSlot item in TempManager.inventory._visItemList)
        {
            if (item.itemStackCount < 5)
            {
                returnList.Add(item);
            }
        }
        return returnList.OfType<ItemSlot>().ToArray();
    }
}
