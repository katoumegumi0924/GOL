using UnityEngine;

/// <summary>
/// UIGame：
/// </summary>
public class UIGame : ManualBehavior
{
    [SerializeField] public UIIterationRules uiIterationRules;
    [SerializeField] public UISettingMenu uiSettingMenu;
    [SerializeField] public UILoadTemplate uiLoadTemplate;

    GameMain gameMain;

    protected override void _OnCreate()
    {
        uiIterationRules._Create();
        uiSettingMenu._Create();
        uiLoadTemplate._Create();
    }

    protected override void _OnDestroy()
    {
        uiIterationRules._Destroy();
        uiSettingMenu._Destroy();
        uiLoadTemplate._Destroy();
    }

    protected override bool _OnInit()
    {
        gameMain = data as GameMain;

        uiIterationRules._Init(gameMain);
        uiSettingMenu._Init(gameMain);
        uiLoadTemplate._Init(gameMain);

        return true;
    }

    protected override void _OnFree()
    {
        uiIterationRules._Free();
        uiSettingMenu._Free();
        uiLoadTemplate._Free();
    }

    protected override void _OnOpen()
    {
        uiIterationRules.SetData(gameMain.data.lifeData.iterationRuleIndex);
        uiIterationRules._Open();
        uiSettingMenu._Open();
        uiLoadTemplate._Open();
        uiLoadTemplate.SetData(gameMain.logic.lifeLogic.templateIndex);
    }

    protected override void _OnClose()
    {
        uiIterationRules._Close();
        uiSettingMenu._Close();
        uiLoadTemplate._Close();
    }

    protected override void _OnUpdate()
    {
        uiSettingMenu._Update();
        uiLoadTemplate._Update();
        uiIterationRules._Update();
    }
}
