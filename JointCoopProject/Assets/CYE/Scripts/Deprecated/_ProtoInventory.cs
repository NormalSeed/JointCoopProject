using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoInventory : MonoBehaviour
{
    private ActiveSkillItem _activeSlot;
    private List<PassiveSkillItem> _passiveSlots;
    [SerializeField] private TraitSkill[] _traitSlots = new TraitSkill[5];


    private void InsertItemToActSlot(ActiveSkillItem activeSkill)
    {
        _activeSlot = activeSkill;
    }

    private void InsertItemToPasSlot(PassiveSkillItem passsiveSkill)
    {
        _passiveSlots.Add(passsiveSkill);
    }

    private void UpgradeTrait(TraitSkill traitSkill)
    {
        
    }

    protected void AddSkillDataList()
    { 
        
    }
}
