using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine;

public class UISettingMenu : ManualBehavior
{
    [Header("Window")]
    public UISaveInput saveinputWindow;

    [Header("UIButton")]
    public UIButton gridButton;
    public UIButton playButton;
    public UIButton resetButton;
    public UIButton stepForwardButton;
    public UIButton speedUpButton;
    public UIButton slowDownButton;
    public UIButton clearButton;
    public UIButton saveButton;
    public UIButton menuButton;
    public UIButton exitButton;

    public Text playText;

    private bool gridToggle;
    private bool playIconToggle;

    private Image playButtonImg;
    private Sprite playSprite;
    private Sprite pauseSprite;

    // 引用
    [NonSerialized]
    public GameMain gameMain;
    public TimeData lifeTime;
    public LifeLogic lifeLogic;
    public LifeRenderer lifeRenderer;

    protected override void _OnCreate()
    {
        saveinputWindow._Create();
    }

    protected override void _OnDestroy()
    {
        saveinputWindow._Destroy();
    }

    protected override bool _OnInit()
    {
        gameMain = data as GameMain;

        saveinputWindow._Init(null);

        playButtonImg = playButton.GetComponent<Image>();
        playSprite = Resources.Load<Sprite>("Icons/play-icon");
        pauseSprite = Resources.Load<Sprite>("Icons/pause-icon");

        return true;
    }

    protected override void _OnFree()
    {
        saveinputWindow._Free();

        playButtonImg = null;
        playSprite = null;
        pauseSprite = null;
    }

    protected override void _OnRegEvent()
    {
        gridButton.onClick += OnClickGridButton;
        playButton.onClick += OnClickPlayButton;
        stepForwardButton.onClick += OnClickStepForwardButton;
        speedUpButton.onClick += OnClickSpeedUpButton;
        slowDownButton.onClick += OnClickSlowDownButton;
        resetButton.onClick += OnClickResetButton;
        clearButton.onClick += OnClickClearButton;
        saveButton.onClick += OnClickSaveButton;
        menuButton.onClick += OnClickMenuButton;
        exitButton.onClick += OnClickExitButton;
    }

    protected override void _OnUnregEvent()
    {
        gridButton.onClick -= OnClickGridButton;
        playButton.onClick -= OnClickPlayButton;
        stepForwardButton.onClick -= OnClickStepForwardButton;
        speedUpButton.onClick -= OnClickSpeedUpButton;
        slowDownButton.onClick -= OnClickSlowDownButton;
        resetButton.onClick -= OnClickResetButton;
        clearButton.onClick -= OnClickClearButton;
        saveButton.onClick -= OnClickSaveButton;
        menuButton.onClick -= OnClickMenuButton;
        exitButton.onClick -= OnClickExitButton;
    }

    protected override void _OnOpen()
    {
        lifeTime = gameMain.data.lifeTime;
        lifeLogic = gameMain.logic.lifeLogic;
        lifeRenderer = gameMain.model.lifeRenderer;

        // 重置playButton显示状态
        playButtonImg.sprite = pauseSprite;
        playText.text = "pause";
    }

    protected override void _OnClose()
    {
        lifeTime = null;
        lifeLogic = null;
        lifeRenderer = null;
    }

    private void OnClickGridButton(int data)
    {
        gridToggle = !gridToggle;
        lifeRenderer.ShowGrid(gridToggle);
    }

    private void OnClickPlayButton(int data)
    {
        lifeTime.TogglePause();
        TogglePlayBtnIcon();
    }

    private void TogglePlayBtnIcon()
    {
        if (!playIconToggle)
        {
            playButtonImg.sprite = playSprite;
            playText.text = "play";
        }
        else
        {
            playButtonImg.sprite = pauseSprite;
            playText.text = "pause";
        }

        playIconToggle = !playIconToggle;
    }

    private void OnClickStepForwardButton(int data)
    {
        lifeTime.Pause();
        lifeLogic.LifeTick();
    }

    private  void OnClickResetButton(int data)
    {
        lifeLogic.ResetState();
    }

    private void OnClickSpeedUpButton(int data)
    {
        lifeTime.Resume();
        lifeTime.SpeedUp();
    }

    private void OnClickSlowDownButton(int data)
    {
        lifeTime.Resume();
        lifeTime.SlowDown();
    }

    private void OnClickClearButton(int data)
    {
        lifeLogic.ClearLife();
    }

    private void OnClickSaveButton(int data)
    {
        string fileName = $"save_{System.DateTime.Now:yyyyMMdd_HHmmss}";
        saveinputWindow.saveNameInputField.text = fileName;
        saveinputWindow._Open();
    }

    private void OnClickMenuButton(int data)
    {
        Program.instance.EndGame();
    }

    private void OnClickExitButton(int data)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
