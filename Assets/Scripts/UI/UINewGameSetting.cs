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
        string widthText = widthInput.text;
        if (string.IsNullOrEmpty(widthText) || string.IsNullOrWhiteSpace(widthText))
        {
            GameDesc.resX = Configs.builtin.resX;
        }
        else
        {
            if (int.TryParse(widthText, out int value))
            {
                GameDesc.resX = value;
            }
        }

        string heightText = heightInput.text;
        if (string.IsNullOrEmpty(heightText) || string.IsNullOrWhiteSpace(heightText))
        {
            GameDesc.resY = Configs.builtin.resY;
        }
        else
        {
            if (int.TryParse(heightText, out int value))
            {
                GameDesc.resY = value;
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
