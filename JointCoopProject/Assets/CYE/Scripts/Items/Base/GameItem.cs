using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public class GameItem : Item, IPickable
{
    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    #endregion

    private void UpgradePassiveSkill(Transform pickupPos)
    {
        int currentGrade = ItemManager.inventory.GetItemSkillGrade(_itemData);
        PlayerSkillManager playerSkillManager = pickupPos.gameObject.GetComponentInChildren<PlayerSkillManager>();
        if (currentGrade > 1)
        {
            playerSkillManager.RemoveSkill(_itemSkill[currentGrade - 2], _itemData._itemType);
            playerSkillManager.AddSkill(_itemSkill[currentGrade - 1]);
        }
        else
        {
            playerSkillManager.AddSkill(_itemSkill[0]); // 첫번째 스킬을 넣음
        }
    }
    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        bool insertResult = ItemManager.inventory.TryGetItem(this, transform);
        if (insertResult)
        {
            if (_itemData._itemType == ItemType.PassiveAttack || _itemData._itemType == ItemType.PassiveAuto)
            {

                UpgradePassiveSkill(pickupPos);
            }
            Destroy(gameObject);
        }
    }
    public void Drop(Transform dropPos)
    {
        Vector2 dropOffset = dropPos.up * 0.6f;
        GameObject itemObject = Instantiate(gameObject, (Vector2)dropPos.position + dropOffset, Quaternion.identity);
        itemObject.SetActive(true);

        Rigidbody2D itemRigid = itemObject.GetComponent<Rigidbody2D>();

        itemRigid.gravityScale = 0f; // 중력 제거
        Vector2 forceDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), 1f).normalized;
        itemRigid.AddForce(forceDir * 3f, ForceMode2D.Impulse);

        // 일정 시간 후 속도 정지
        itemObject.GetComponent<MonoBehaviour>().StartCoroutine(StopMovement(itemRigid));

        itemObject.GetComponent<Rigidbody2D>().AddForce(2f * transform.forward);

    }

    private IEnumerator StopMovement(Rigidbody2D itemRigid)
    {
        yield return new WaitForSeconds(0.2f);
        itemRigid.velocity = Vector2.zero;
    }
    #endregion 
}
