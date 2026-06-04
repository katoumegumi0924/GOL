using UnityEngine;

/// <summary>
/// LifeRuleSet：
/// </summary>
[CreateAssetMenu(fileName = "LifeRuleSet", menuName = "Configs/LifeRule/LifeRuleSet")]
public class LifeRuleSet : ScriptableObject
{
    [SerializeField]
    private LifeRuleConfig[] lifeaRules;

    public LifeRuleConfig GetLifeRule(int index)
    {
        if (index < 0 || index > lifeaRules.Length)
            return null;

        return lifeaRules[index];
    }

    public int GetLifeRuleLength()
    {
        return lifeaRules.Length;
    }
}
