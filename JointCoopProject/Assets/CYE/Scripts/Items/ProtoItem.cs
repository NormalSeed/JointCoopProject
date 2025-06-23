using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ProtoItem : MonoBehaviour
{
    /* 아이템 요구사항
     * 종류: active/passive/one time/weapon(deprecated)
     * 
     * Item - weapon(deprecated)
     * 획득 후 player의 공격 방식 변경
     *  -> 변경할 공격 방식
     * 
     * Item - active (추가 스킬 생성)
     * 획득 후 특정 키입력(space)을 통해 사용
     *  -> 아이템별로 사용시 발생하는 효과 존재
     *  -> 사용을 위한 게이지(변경 가능성 있음)의 경우 inventory에서 관리
     *  -> 획득할때 유지할지 교체할지 선택하는 기능 필요
     *  e.g. 캐릭터 회피 기능, 반사 등
     * 
     * Item - passive(3 types)
     *  type AT - attack passive(갯수 제한 있음/교체에 대한 내용은 구상중)
     *  기본 공격 강화(공격 방식 추가)
     *  type AU - auto passive
     *   -> player attack시 발동/개별 cooldown 존재(활성화된 아이템 1개에 한하여)
     *   -> 개별 cooldown마다 발동(활성화된 아이템 n개에 한하여)
     *   
     * Item - one time
     * coin 등 소모성 재화에 대한 수치 변경 //player의 hp나 max hp,
     *  -> 획득시 소멸(인벤토리에 남지 않음)
     * 
     * npc(상인)가 플레이어 stats의 향샹을 계획/아이템
     * type ST - stats change
     *   -> 두가지 구현방식 가능(일회성: 획득 당시의 stats을 기반으로 스탯 변화 후 종료/지속형: 획득 후 player의 stats에 지속적으로 영향을 끼침(player의 행동시 마다 수치를 재계산하여 가져오게 됨/이경우 아이템의 효과 반영에 대한 우선순위가 필요함))
     *   -> 인벤토리 보유시 player의 스탯(attack point, range, speed 등)에 영향을 줌
     */

    #region // Item Status
    [SerializeField] private string _itemName; // 아이템 이름
    [SerializeField] private string _itemDescription; // 아이템 설명
    private Sprite _itemSprite; // 아이템 모양    
    #endregion

    #region // Events
    [HideInInspector] public UnityEvent OnUsed; // 아아템 사용시 발생
    [HideInInspector] public UnityEvent OnAcquired; // 아이템 획득시 발생
    #endregion

    #region // Properties
    protected Sprite ItemSprite { get { return _itemSprite; } private set { _itemSprite = value; } }
    #endregion

    protected void InitItemBase() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        ItemSprite = spriteRenderer.sprite;
    }


    protected abstract void Use();
    protected abstract void Acquire();

}
