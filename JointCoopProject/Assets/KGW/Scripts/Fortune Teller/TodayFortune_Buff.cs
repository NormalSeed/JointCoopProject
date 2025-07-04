using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodayFortune_Buff : IBuff
{
    public string _buffName => $"오늘의 덕담";
    public string _buffDescription => "플레이어한테 좋은 얘기를 전달해 줍니다.";
    public BuffCategory _buffCategory { get; private set; }
    public BuffType _buffType => BuffType.Fortune;
    public int _buffLevel {  get; private set; }

    public TodayFortune_Buff(BuffCategory category, int level)
    {
        _buffCategory = category;
        _buffLevel = level;
    }

    public void BuffReceive(PlayerStatManager playerStatus)
    {
        if (_buffCategory == BuffCategory.Blessing)
        {
            // 플레이어에게 레벨 {_buffLevel}의 축복 운세를 점쳐줍니다.
            switch (_buffLevel)
            {
                case 1:
                    BuffManager.Instance.FortuneConfirm("오늘 선택지는 자그마한 행복입니다.");
                    break;
                case 2:
                    BuffManager.Instance.FortuneConfirm("오늘 당신의 인내는 시련을 이기는 열쇠가 됩니다.");
                    break;
                case 3:
                    BuffManager.Instance.FortuneConfirm("오늘은 비어있지 않지만 그렇다고 채워지지 않습니다.");
                    break;
                case 4:
                    BuffManager.Instance.FortuneConfirm("고요함 속에서 가장 큰 계시가 들릴 것입니다.");
                    break;
                case 5:
                    BuffManager.Instance.FortuneConfirm("새로운 일을 하는건 위험이 큽니다. 오늘같은 날이면 시도해볼만 하군요.");
                    break;
                default:
                    break;
            }
        }
        if (_buffCategory == BuffCategory.Curs)
        {
            // 플레이어에게 레벨 {_buffLevel}의 저주 운세를 점쳐줍니다.
            switch (_buffLevel)
            {
                case 1:
                    BuffManager.Instance.FortuneConfirm("이득이라 생각한 일들이 사실은 손해이니 조심하세요.");
                    break;
                case 2:
                    BuffManager.Instance.FortuneConfirm("오늘은 아니지만 조금만 기다리세요.");
                    break;
                case 3:
                    BuffManager.Instance.FortuneConfirm("오늘 당신이 고른 모든 선택지는 그나마 최악은 아닙니다.");
                    break;
                case 4:
                    BuffManager.Instance.FortuneConfirm("당신이 누군가의 고난이 될 수 있습니다. 조심하세요.");
                    break;
                case 5:
                    BuffManager.Instance.FortuneConfirm("오늘은 행운보다는 행복을 찾는게 좋습니다.");
                    break;
                default:
                    break;
            }
        }
    }
}
