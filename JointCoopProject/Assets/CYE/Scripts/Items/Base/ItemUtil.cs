using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 유틸리티 클래스
/// </summary>
public static class ItemUtil
{
    /// <summary>
    /// 현재 시간을 기준으로 unix timestamp값을 float형태로 반환합니다.
    /// </summary>
    /// <returns>float형식의 unix timestamp</returns>
    public static float GetUnixTimeStamp()
    {
        TimeSpan timestamp = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        return (float)timestamp.TotalSeconds;
    }
}
public interface IPickable
{
    public void PickUp(Transform pickupPos);
    public void Drop(Transform dropPos);
}
public enum ItemType
{
    Active, PassiveAttack, PassiveAuto, shop, Expend
}
public interface IInstallable
{
    public void Install(Transform setUpPos);
}