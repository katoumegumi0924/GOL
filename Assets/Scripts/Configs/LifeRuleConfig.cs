using UnityEngine;

/// <summary>
/// LifeRuleConfig：
/// </summary>
[CreateAssetMenu(fileName = "LifeRuleConfig", menuName = "Configs/LifeRule/LifeRuleConfig")]
public class LifeRuleConfig : ScriptableObject
{
    public string ruleName;
    [TextArea] public string ruleDesc;

    public bool[] birth = new bool[9];
    public bool[] survival = new bool[9];

    public int survivalMask { get { return BoolArrayToMask(survival); } }
    public int birthMask { get { return BoolArrayToMask(birth); } }

    private int BoolArrayToMask(bool[] conditions)
    {
        int mask = 0;
        for (int i = 0; i < conditions.Length; ++i)
        {
            if (conditions[i])
            {
                mask = mask | (1 << i);
            }
        }

        return mask;
    }
}
