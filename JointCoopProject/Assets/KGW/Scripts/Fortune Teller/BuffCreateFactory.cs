using Unity.VisualScripting;
using UnityEngine;

public static class BuffCreateFactory
{
    static System.Random _randomChoice = new System.Random();

    // 버퍼룰렛 (30% : 축복, 20% : 저주, 50% : 꽝)
    public static IBuff BuffRoulette()
    {
        int playerLuckSetting = PlayerStatManager.Instance._playerLuck * 5; // 캐릭터 운1 -> 확률 5% 
        
        // 0 ~ 99 Choice
        int randomRoll = _randomChoice.Next(0, 100);

        if (randomRoll < 50 - playerLuckSetting)    // 꽝!! (50%) , 캐릭터 운1 -> 꽝 확률 5%감소
        {
            return new Nothing_Buff();
        }
        else if (randomRoll < 80)  // 축복!! (30%) 캐릭터 운1 -> 축복 확률 5%증가
        {
            return GenerateBlessBuff();
        }
        else    // 저주!! (20%)
        {
            return GenerateCusDeBuff();
        }
    }

    // 축복 버퍼 레벨 설정
    public static IBuff GenerateBlessBuff()
    {
        int playerLuckSetting = PlayerStatManager.Instance._playerLuck;
        int baseRollRange = 3000;   // 축복 버퍼 초기 30%
        int currentRollRange = baseRollRange + (playerLuckSetting * 500);   // 캐릭터 운 1증가 -> 전체 확률 5%증가

        int level1_Range = 2000 + (playerLuckSetting * 200);    // Level 1 Buff 구간 세팅
        int level2_Range = level1_Range + 700 + (playerLuckSetting * 100);  // Level 2 Buff 구간 세팅
        int level3_Range = level2_Range + 200 + (playerLuckSetting * 100);  // Level 3 Buff 구간 세팅
        int level4_Range = level3_Range + 65 + (playerLuckSetting * 70);    // Level 4 Buff 구간 세팅, Level 5 구간은 나머지 

        // 소수점 위 두자리, 아래 두자리의 확률로 인하여 천다위로 세팅
        int BlessLevelRoll = _randomChoice.Next(0, currentRollRange);

        if (BlessLevelRoll < level1_Range)   // Level 1 Bless (20%) 운1 -> Level1 확률 2%증가
        {
            return GenerateBuffType(BuffCategory.Blessing, 1);
        }
        else if (BlessLevelRoll < level2_Range)  // Level 2 Bless (7%) 운1 -> Level1 확률 1%증가
        {
            return GenerateBuffType(BuffCategory.Blessing, 2);
        }
        else if (BlessLevelRoll < level3_Range)  // Level 3 Bless (2%) 운1 -> Level1 확률 1%증가
        {
            return GenerateBuffType(BuffCategory.Blessing, 3);
        }
        else if (BlessLevelRoll < level4_Range)  // Level 4 Bless (0.65%) 운1 -> Level1 확률 0.7%증가
        {
            return GenerateBuffType(BuffCategory.Blessing, 4);
        }
        else  // Level 5 Bless (0.35%) 운1 -> Level1 확률 0.3%증가
        {
            return GenerateBuffType(BuffCategory.Blessing, 5);
        }
    }

    // 저주 디버퍼 레벨 설정
    public static IBuff GenerateCusDeBuff()
    {
        int baseRollRange = 2000;   // 저주 디버퍼 초기 20%

        // 소수점 위 두자리, 아래 두자리의 확률로 인하여 천다위로 세팅
        int CusLevelRoll = _randomChoice.Next(0, baseRollRange);

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
