using UnityEngine;

/// <summary>
/// LifeRuleSet：
/// </summary>
[CreateAssetMenu(fileName = "LifeRuleSet", menuName = "Protos/LifeRule/LifeRuleSet")]
public class LifeRuleSet : ScriptableObject
{
    [SerializeField]
    private LifeRule[] lifeRules;

    public LifeRule GetLifeRule(int index)
    {
        if (index < 0 || index > lifeRules.Length)
            return null;

        return lifeRules[index];
    }

    public int GetLifeRuleIndex(string ruleString)
    {
        if (string.IsNullOrEmpty(ruleString))
            return -1;

        for (int i = 0; i < lifeRules.Length; ++i)
        {
            if (string.Equals(ruleString, lifeRules[i].ruleString, System.StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    public int GetLifeRuleLength()
    {
        return lifeRules.Length;
    }
}
