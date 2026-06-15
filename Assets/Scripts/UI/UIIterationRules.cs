using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIIterationRules : ManualBehavior
{
    public Dropdown rulesDropdown;
    public Text textDesc;

    GameMain gameMain;

    protected override bool _OnInit()
    {
        gameMain = data as GameMain;
        if (gameMain == null)
            return false;

        rulesDropdown.ClearOptions();

        var ruleSet = Configs.ruleSet;
        if (ruleSet == null)
            return false;

        for (int i = 0; i < ruleSet.GetLifeRuleLength(); ++i)
        {
            rulesDropdown.options.Add(new Dropdown.OptionData(ruleSet.GetLifeRule(i).ruleName));
        }

        return true;
    }

    protected override void _OnFree()
    {
        rulesDropdown.ClearOptions();
        gameMain = null;
    }

    protected override void _OnRegEvent()
    {
        rulesDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    protected override void _OnUnregEvent()
    {
        rulesDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    protected override void _OnOpen()
    {
        
    }

    protected override void _OnClose()
    {
        
    }

    protected override void _OnUpdate()
    {
        if (gameMain?.data?.lifeData == null)
            return;

        rulesDropdown.value = gameMain.data.lifeData.iterationRuleIndex;
    }

    public void SetData(int ruleIndex)
    {
        // 先设置为-1，避免DropDown默认值与ruleIndex相同时DropDown不刷新的问题
        rulesDropdown.value = -1;

        rulesDropdown.value = ruleIndex;
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
        lifeLogic.ruleRleDatas = lifeLogic.GetCurRuleRleData(lifeData.iterationRuleIndex);

        var uiLoadTemplate = UIRoot.instance.uiGame.uiLoadTemplate;
        uiLoadTemplate.SetData(0);
        uiLoadTemplate.RefreshTemplateList(lifeLogic.ruleRleDatas, lifeData.iterationRuleIndex);

        textDesc.text = ruleSet.GetLifeRule(index).ruleDesc;
    }
}
