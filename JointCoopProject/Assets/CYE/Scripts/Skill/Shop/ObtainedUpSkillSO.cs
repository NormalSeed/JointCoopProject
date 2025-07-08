using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shop/Obtained Up")]
public class ObtainedUpSkillSO : SkillDataSO
{
    [System.Serializable]
    private class ValueProbabilityPair
    {
        public int increaseValue;
        public int probability;
        [HideInInspector] public int realProbability;
        public void SetRealProbability(int value)
        {
            realProbability = value;
        }
    }
    [Tooltip("결과값과 해당 결과값의 확률을 기입합니다.")]
    [SerializeField] private List<ValueProbabilityPair> _valueProbabilities;
    private System.Random _randomInstance = new System.Random();

    public override void UseSkill(Transform caster, out bool useResult)
    {
        PlayerStatManager.Instance._additionalDropGold += GetRandomResultValue();
        useResult = true;
    }
    private int GetRandomResultValue()
    {
        int returnValue = 0;
        int randomResult = _randomInstance.Next(0, 100);
        
        SetProbablitySteps();
        
        foreach (ValueProbabilityPair valueProbability in _valueProbabilities)
        {
            if (randomResult < valueProbability.realProbability)
            {
                returnValue = valueProbability.increaseValue;
                break;
            }
        }
        return returnValue;
    }
    
    private void SetProbablitySteps()
    { 
        int probabilityStep = 0;
        foreach (ValueProbabilityPair valueProbability in _valueProbabilities)
        {
            probabilityStep += valueProbability.probability;
            valueProbability.SetRealProbability(probabilityStep);
        }
    }
}
