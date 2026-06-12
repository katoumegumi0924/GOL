using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UILoadTemplate：
/// </summary>
public class UILoadTemplate : ManualBehavior
{
    [SerializeField] public Dropdown templateDropdown;
    [SerializeField] public Text tipInfo;

    [HideInInspector]
    public GameMain gameMain;

    protected override bool _OnInit()
    {
        gameMain = data as GameMain;
        if (gameMain == null)
            return false;

        return true;
    }

    protected override void _OnFree()
    {
        gameMain = null;
    }

    protected override void _OnRegEvent()
    {
        templateDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    protected override void _OnUnregEvent()
    {
        templateDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    protected override void _OnOpen()
    {
        templateDropdown.ClearOptions();

        var rleDatas = gameMain.logic.lifeLogic.rleDatas;
        templateDropdown.options.Add(new Dropdown.OptionData("选择模板"));
        for (int i = 0; i < rleDatas.Length; ++i)
        {
            templateDropdown.options.Add(new Dropdown.OptionData(rleDatas[i].name));
        }
    }

    protected override void _OnClose()
    {
        templateDropdown.ClearOptions();
    }

    protected override void _OnUpdate()
    {
        if (gameMain?.logic?.lifeLogic == null || gameMain?.data?.lifeData == null) 
            return;

        var lifeLogic = gameMain.logic.lifeLogic;
        var lifeData = gameMain.data.lifeData;
        templateDropdown.value = lifeLogic.templateIndex;


        if (lifeLogic.templateIndex > 0)
        {
            var rleData = lifeLogic.rleDatas[lifeLogic.templateIndex - 1];

            if (rleData.width > lifeData.width || rleData.height > lifeData.height)
            {
                tipInfo.gameObject.SetActive(true);
                tipInfo.text = $"当前模板的尺寸为{rleData.width}x{rleData.height}，已超过画布尺寸";
            }
        }
        else
        {
            tipInfo.gameObject.SetActive(false);
        }
    }

    public void SetData(int templateIndex)
    {
        templateDropdown.value = -1;
        templateDropdown.value = templateIndex;
    }

    public void OnDropdownValueChanged(int index)
    {
        var rleDatas = gameMain.logic.lifeLogic.rleDatas;

        if (rleDatas == null || index < 0 || index > rleDatas.Length)
            return;

        var lifeLogic = gameMain.logic.lifeLogic;
        lifeLogic.templateIndex = index;
    }
}
