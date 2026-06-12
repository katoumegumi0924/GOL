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

    private Image playButtonImg;
    private Sprite playSprite;
    private Sprite pauseSprite;

    // 引用
    [NonSerialized]
    public GameMain gameMain;
    public TimeData gameTime;
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
        gameMain = null;

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
        gameTime = gameMain.data.gameTime;
        lifeLogic = gameMain.logic.lifeLogic;
        lifeRenderer = gameMain.model.lifeRenderer;
    }

    protected override void _OnClose()
    {
        gameTime = null;
        lifeLogic = null;
        lifeRenderer = null;
    }

    protected override void _OnUpdate()
    {
        UpdatePlayBtnIcon(gameTime.pausing);
    }

    private void OnClickGridButton(int data)
    {
        gridToggle = !gridToggle;
        lifeRenderer.ShowGrid(gridToggle);
    }

    private void OnClickPlayButton(int data)
    {
        gameTime.TogglePause();
    }

    public void UpdatePlayBtnIcon(bool isPaused)
    {
        if (isPaused)
        {
            playButtonImg.sprite = playSprite;
            playText.text = "play";
        }
        else
        {
            playButtonImg.sprite = pauseSprite;
            playText.text = "pause";
        }
    }

    private void OnClickStepForwardButton(int data)
    {
        gameTime.Pause();
        lifeLogic.LifeTick();
    }

    private  void OnClickResetButton(int data)
    {
        lifeLogic.ResetState();
    }

    private void OnClickSpeedUpButton(int data)
    {
        gameTime.Resume();
        gameTime.SpeedUp();
    }

    private void OnClickSlowDownButton(int data)
    {
        gameTime.Resume();
        gameTime.SlowDown();
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
