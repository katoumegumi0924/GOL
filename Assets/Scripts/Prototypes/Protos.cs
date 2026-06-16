using UnityEngine;

/// <summary>
/// Proto：
/// </summary>
public static class Protos
{
    private static LifeRuleSet _ruleSet;
    public static LifeRuleSet ruleSet
    {
        get
        {
            if (_ruleSet == null)
            {
                _ruleSet = Resources.Load<LifeRuleSet>("Prototypes/Life Rule Set");

                if (_ruleSet == null)
                {
                    Debug.LogError("错误：在 Resources 文件夹下找不到名为 'Life Rule Set' 的配置文件！");
                }
            }

            return _ruleSet;
        }
    }
}
