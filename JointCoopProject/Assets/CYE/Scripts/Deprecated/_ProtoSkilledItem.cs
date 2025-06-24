using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ProtoSkilledItem : ProtoItem
{
    #region // SkilledItem Info
    protected bool _canCast;

    [SerializeField] private float _totalCooldown; // 스킬 쿨다운
    private float _currentCooldown;
    #endregion


    #region // SkilledItem Events
    public UnityEvent _onSkillCasted; // 스킬 시전 시작시 발생 이벤트
    public UnityEvent _onSkillProceed; // 스킬 시전 완료시 발생 효과
    #endregion

    // Skilled Item Init
    protected void InitSkilledItem()
    {
        _canCast = false;
        ResetCooldown();
    }

    // Cast Skill
    protected void Cast()
    {
        _onSkillCasted?.Invoke();
        _onSkillProceed?.Invoke();
    }

    // Count Current Cooldown
    protected void CountCooldown()
    {
        _currentCooldown += Time.deltaTime;
        if (_currentCooldown >= _totalCooldown)
        {
            _canCast = true;
        }
    }

    // Reset Current Cooldown by zero
    protected void ResetCooldown()
    {
        _currentCooldown = 0f;
    }
}
