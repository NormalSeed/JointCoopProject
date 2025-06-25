using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDesc;
    private SkillItemType itemType;
    public SkillItemType _itemType { get { return itemType; } protected set { itemType = value; } }

    // layerSkill 처럼 skill별로 object 생성후 지정하는 방식으로
    [SerializeField] private SkillDataSO _skillData;
}
