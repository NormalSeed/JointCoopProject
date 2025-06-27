using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public static class BuffCreateFactory
{
    private static System.Random _randomChoice = new System.Random();

    //// 버퍼룰렛 (30% : 축복, 20% : 저주, 50% : 꽝)
    //public static IBuff BuffRoulette()
    //{
    //    // 1 ~ 100 Choice
    //    int randomRoll = _randomChoice.Next(1, 101);

    //    if (randomRoll < 51)    // 꽝!! (50%)
    //    {
    //        return new Nothing_Buff();
    //    }
    //    else if ( randomRoll < 81)  // 축복!! (30%)
    //    {
    //        // 1 ~ 20 Choice
    //        int blessRoll = _randomChoice.Next(1, 21);

    //        if (blessRoll < 16) // 축복의 15%는 버프, 5%는 오늘의 운세
    //        {
                
    //        }
    //        else
    //        {
    //            return new TodayFortune_Buff();
    //        }
    //    }
    //}

    //// 축복 버퍼 생성
    //public static IBuff GenerateBlessBuff(BuffCategory category, int level)
    //{
    //    int bufferTypeRoll = _randomChoice.Next(1, 21);

    //    if (bufferTypeRoll < 6)
    //    {
    //        return new AttackPower_Buff(category, level);
    //    }

    //}

    //// 저주 버퍼 생성
    //public static IBuff GenerateCursBuff(BuffCategory category, int level)
    //{

    //}


}
