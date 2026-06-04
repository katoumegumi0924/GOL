using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIIterationRules : ManualBehavior
{
    public Dropdown rulesDropDown;
    public Text textDesc;

    GameMain gameMain;

    protected override bool _OnInit()
    {
        gameMain = data as GameMain;
        if (gameMain == null)
            return false;

        rulesDropDown.ClearOptions();

        var ruleSet = Configs.ruleSet;
        if (ruleSet == null)
            return false;

        for (int i = 0; i < ruleSet.GetLifeRuleLength(); ++i)
        {
            rulesDropDown.options.Add(new Dropdown.OptionData(ruleSet.GetLifeRule(i).ruleName));
        }

        return true;
    }

    protected override void _OnFree()
    {
        rulesDropDown.ClearOptions();
    }

    protected override void _OnRegEvent()
    {
        rulesDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    protected override void _OnUnregEvent()
    {
        rulesDropDown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    protected override void _OnOpen()
    {
        
    }

    protected override void _OnClose()
    {
        
    }

    public void SetData(int ruleIndex)
    {
        // 先设置为-1，避免DropDown默认值与ruleIndex相同时DropDown不刷新的问题
        rulesDropDown.value = -1;

        rulesDropDown.value = ruleIndex;
    }

    private void OnDropdownValueChanged(int index)
    {
        var ruleSet = Configs.ruleSet;

        if (ruleSet == null || index < 0 || index >= ruleSet.GetLifeRuleLength())
            return;

        var lifeData = gameMain.data.lifeData;
        lifeData.iterationRuleIndex = index;
        var lifeLogic = gameMain.logic.lifeLogic;
        lifeLogic.lifeRule = ruleSet.GetLifeRule(lifeData.iterationRuleIndex);

        textDesc.text = ruleSet.GetLifeRule(index).ruleDesc;
    }
}
