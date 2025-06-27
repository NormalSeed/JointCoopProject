using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public static class BuffCreateFactory
{
    private static System.Random _randomChoice = new System.Random();

    // 버퍼룰렛 (30% : 축복, 20% : 저주, 50% : 꽝)
    public static IBuff BuffRoulette()
    {
        // 0 ~ 99 Choice
        int randomRoll = _randomChoice.Next(0, 100);

        if (randomRoll < 50)    // 꽝!! (50%)
        {
            return new Nothing_Buff();
        }
        else if (randomRoll < 80)  // 축복!! (30%)
        {
            return GenerateBlessBuff();
        }
        else    // 저주!! (20%)
        {
            return GenerateCusBuff();
        }
    }

    // 축복 버퍼 레벨 설정
    public static IBuff GenerateBlessBuff()
    {
        int BlessLevelRoll = _randomChoice.Next(0, 3000);

        if (BlessLevelRoll < 2000)   // Level 1 Bless (20%)
        {
            return GenerateBuffType(BuffCategory.Blessing, 1);
        }
        else if (BlessLevelRoll < 2700)  // Level 2 Bless (7%)
        {
            return GenerateBuffType(BuffCategory.Blessing, 2);
        }
        else if (BlessLevelRoll < 2900)  // Level 3 Bless (2%)
        {
            return GenerateBuffType(BuffCategory.Blessing, 3);
        }
        else if (BlessLevelRoll < 2965)  // Level 4 Bless (0.65%)
        {
            return GenerateBuffType(BuffCategory.Blessing, 4);
        }
        else    // Level 5 Bless (0.35%)
        {
            return GenerateBuffType(BuffCategory.Blessing, 5);
        }
            
    }

    // 저주 버퍼 레벨 설정
    public static IBuff GenerateCusBuff()
    {
        int CusLevelRoll = _randomChoice.Next(0, 2000);

        if (CusLevelRoll < 1000)   // Level 1 Cus (10%)
        {
            return GenerateBuffType(BuffCategory.Curs, 1);
        }
        else if (CusLevelRoll < 1400)  // Level 2 Cus (4%)
        {
            return GenerateBuffType(BuffCategory.Curs, 2);
        }
        else if (CusLevelRoll < 1700)  // Level 3 Cus (3%)
        {
            return GenerateBuffType(BuffCategory.Curs, 3);
        }
        else if (CusLevelRoll < 1900)  // Level 4 Cus (2%)
        {
            return GenerateBuffType(BuffCategory.Curs, 4);
        }
        else    // Level 5 Cus (1%)
        {
            return GenerateBuffType(BuffCategory.Curs, 5);
        }
    }

    // 버퍼 타입 설정
    public static IBuff GenerateBuffType(BuffCategory category, int level)
    {
        int bufferTypeRoll = _randomChoice.Next(0, 20);

        if (bufferTypeRoll < 5) // Attack Power Buff (5%)
        {
            return new AttackPower_Buff(category, level);
        }
        else if (bufferTypeRoll < 10)   // Attack Speed Buff (5%)
        {
            return new AttackSpeed_Buff(category, level);
        }
        else if (bufferTypeRoll < 15)   // Move Speed Buff (5%)
        {
            return new MoveSpeed_Buff(category, level);
        }
        else    // Today Fortune Buff (5%)
        {
            return new TodayFortune_Buff(category, level);
        }

    }
  
}
