using UnityEngine;

/// <summary>
/// UIRoot：
/// </summary>
public class UIRoot : ManualBehavior
{
    private static UIRoot _instance;
    public static UIRoot instance { get { return _instance; } }

    [SerializeField]public UIGame uiGame;
    [SerializeField]public UIMainMenu uiMainMenu;

    public GameMain gameMain;

    protected override void _OnCreate()
    {
        _instance = this;

        uiGame._Create();
        uiMainMenu._Create();
    }

    protected override void _OnDestroy()
    {
        uiGame._Destroy();
        uiMainMenu._Destroy();

        _instance = null;
    }

    protected override bool _OnInit()
    {
        uiGame._Init(gameMain);
        uiMainMenu._Init(null);

        return true;
    }

    protected override void _OnOpen()
    {
        uiMainMenu._Open();
        uiGame._Close();
    }

    protected override void _OnClose()
    {
        uiMainMenu._Close();
        uiGame._Close();
    }

    protected override void _OnFree()
    {
        uiGame._Free();
        uiMainMenu._Free();
    }

    protected override void _OnUpdate()
    {
        if (uiGame.active)
            uiGame._Update();

        if (uiMainMenu.active)
            uiMainMenu._Update();
    }
}
