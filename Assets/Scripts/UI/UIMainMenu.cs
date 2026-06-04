using UnityEngine;

/// <summary>
/// UIMainMenu：
/// </summary>
public class UIMainMenu : ManualBehavior
{
    [SerializeField] public UIButton newGameBtn;
    [SerializeField] public UIButton continueBtn;
    [SerializeField] public UIButton exitBtn;

    [SerializeField] public UILoadFile uiLoadFile;
    [SerializeField] public UINewGameSetting uiNewGameSetting;

    protected override void _OnCreate()
    {
        uiLoadFile._Create();
        uiNewGameSetting._Create();
    }

    protected override void _OnDestroy()
    {
        uiLoadFile._Destroy();
        uiNewGameSetting._Destroy();
    }

    protected override bool _OnInit()
    {
        uiLoadFile._Init(null);
        uiNewGameSetting._Init(null);

        return true;
    }

    protected override void _OnFree()
    {
        uiLoadFile._Free();
        uiNewGameSetting._Free();
    }

    protected override void _OnRegEvent()
    {
        newGameBtn.onClick += OnNewGameBtnClick;
        continueBtn.onClick += OnContinueBtnClick;
    }

    protected override void _OnUnregEvent()
    {
        newGameBtn.onClick -= OnNewGameBtnClick;
        continueBtn.onClick -= OnContinueBtnClick;
    }

    protected override void _OnOpen()
    {

    }

    protected override void _OnClose()
    {
        uiLoadFile._Close();
    }

    protected override void _OnUpdate()
    {
        if (uiLoadFile.active)
            uiLoadFile._Update();
    }

    private void OnNewGameBtnClick(int data)
    {
        uiNewGameSetting._Open();
    }

    private void OnContinueBtnClick(int data)
    {
        uiLoadFile._Open();
    }

    private void OnExitBtnClick(int data)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
