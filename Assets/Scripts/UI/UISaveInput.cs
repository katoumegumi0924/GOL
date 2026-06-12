using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UISaveInput : ManualBehavior
{
    public InputField saveNameInputField;
    public UIButton confirmButton;
    public UIButton cancelButton;

    protected override void _OnOpen()
    {
        GameMain.instance.data.gameTime.Pause();
    }

    protected override void _OnClose()
    {
        GameMain.instance.data.gameTime.Resume();
    }

    protected override void _OnRegEvent()
    {
        confirmButton.onClick += OnConfirmClick;
        cancelButton.onClick += OnCancelClick;
    }

    protected override void _OnUnregEvent()
    {
        confirmButton.onClick -= OnConfirmClick;
        cancelButton.onClick -= OnCancelClick;
    }

    private void OnConfirmClick(int data)
    {
        string inputName = saveNameInputField.text;

        if (string.IsNullOrWhiteSpace(inputName))
        {
            Debug.LogWarning("存档名称不能为空");
            return;
        }

        GameSave.SaveGame(inputName, GameMain.instance.data);
        _Close();
    }

    private void OnCancelClick(int data)
    {
        _Close();
    }
}
