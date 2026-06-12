using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIGameDesc：
/// </summary>
public class UINewGameSetting : ManualBehavior
{
    [SerializeField] public InputField widthInput;
    [SerializeField] public InputField heightInput;

    [SerializeField] public UIButton confirmBtn;
    [SerializeField] public UIButton cancelBtn;

    protected override bool _OnInit()
    {
        return true;
    }

    protected override void _OnFree()
    {

    }

    protected override void _OnRegEvent()
    {
        confirmBtn.onClick += OnConfirmBtnClick;
        cancelBtn.onClick += OnCancelBtnClick;
    }

    protected override void _OnUnregEvent()
    {
        confirmBtn.onClick -= OnConfirmBtnClick;
        cancelBtn.onClick -= OnCancelBtnClick;
    }

    public void OnConfirmBtnClick(int data)
    {
        var gameDesc = Program.instance.gameDesc;

        string widthText = widthInput.text;
        if (string.IsNullOrEmpty(widthText) || string.IsNullOrWhiteSpace(widthText))
        {
            gameDesc.resX = Configs.builtin.resX;
        }
        else
        {
            if (int.TryParse(widthText, out int value))
            {
                gameDesc.resX = value;
            }
        }

        string heightText = heightInput.text;
        if (string.IsNullOrEmpty(heightText) || string.IsNullOrWhiteSpace(heightText))
        {
            gameDesc.resY = Configs.builtin.resY;
        }
        else
        {
            if (int.TryParse(heightText, out int value))
            {
                gameDesc.resY = value;
            }
        }

        Program.instance.NewGame();
        _Close();
    }

    public void OnCancelBtnClick(int data)
    {
        _Close();
    }
}
