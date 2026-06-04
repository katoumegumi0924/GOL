using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MessageBox：
/// </summary>
public class UIMessageBox : UIDialog
{
    [SerializeField] public RectTransform m_WindowTrans;
    [SerializeField] public Image m_Body;
    [SerializeField] public Text m_TitleText;
    [SerializeField] public Text m_MessageText;
    [SerializeField] public Image m_IconImage;
    [SerializeField] public Text m_Button1;
    [SerializeField] public Text m_Button2;
    [SerializeField] public Text m_Button3;

    [SerializeField] public int minWidth;
	[SerializeField] public int minHeight;
	[SerializeField] public int paddingX;
	[SerializeField] public int paddingY;

    public Sprite[] SpriteSet;
    public delegate void Response();
    Response Resp1;
    Response Resp2;
    Response Resp3;

    public const int INFO = 0;
    public const int WARNING = 1;
    public const int QUESTION = 2;
    public const int ERROR = 3;

    public static List<UIMessageBox> messageStack;
    public static int activeCount { get { return messageStack == null ? 0 : messageStack.Count; } }

    static void PushMessage(UIMessageBox messageBox)
    {
        if (messageStack == null)
            messageStack = new List<UIMessageBox>();

        messageStack.Add(messageBox);
    }

    static void PopMessage(UIMessageBox messageBox)
    {
        if (messageStack != null && messageStack.Count > 0)
        {
            messageStack.Remove(messageBox);
        }
    }

    public static bool CloseTopMessage()
    {
        if (messageStack != null && messageStack.Count > 0)
        {
            var mb = messageStack[messageStack.Count - 1];
            if (mb != null)
            {
                mb.OnButton1Click();
                return true;
            }
            else
            {
                messageStack.RemoveAt(messageStack.Count - 1);
                return false;
            }
        }

        return false;
    }

    public static void CloseAllMessage()
    {
        if (messageStack != null)
        {
            while (messageStack.Count > 0)
            {
                CloseTopMessage();
            }
        }
    }

    public static UIMessageBox TopMessage()
    {
        if (messageStack != null && messageStack.Count > 0)
            return messageStack[messageStack.Count - 1];
        else
            return null;

    }

    public static UIMessageBox Show(string title, string message, string btn1,
                                    string btn2, string btn3, int icon,
                                    Response resp1, Response resp2, Response resp3)
    {
        return Show("Prefabs/message-box", title, message, btn1, btn2, btn3, icon, resp1, resp2, resp3);
    }

    public static UIMessageBox Show(string title, string message, string btn1,
                                     string btn2, int icon, Response resp1, Response resp2)
    {
        return Show("Prefabs/message-box", title, message, btn1, btn2, null, icon, resp1, resp2, null);
    }

    public static UIMessageBox Show(string title, string message, string btn1,
                                     int icon, Response resp1)
    {
        return Show("Prefabs/message-box", title, message, btn1, null, null, icon, resp1, null, null);
    }

    public static UIMessageBox Show(string title, string message, string btn1, int icon)
    {
        return Show("Prefabs/message-box", title, message, btn1, null, null, icon, null, null, null);
    }

    public static UIMessageBox Show(string prefab, string title, string message, string btn1,
                                 string btn2, int icon, Response resp1, Response resp2)
    {
        return Show(prefab, title, message, btn1, btn2, null, icon, resp1, resp2, null);
    }

    public static UIMessageBox Show(string prefab, string title, string message,
                                     string btn1, int icon, Response resp1)
    {
        return Show(prefab, title, message, btn1, null, null, icon, resp1, null, null);
    }

    public static UIMessageBox Show(string prefab, string title, string message, string btn1, int icon)
    {
        return Show(prefab, title, message, btn1, null, null, icon, null, null, null);
    }

    public static UIMessageBox Show(string prefabPath, string title, string message,
                                    string btn1, string btn2, string btn3, int icon,
                                    Response resp1, Response resp2, Response resp3)
    {
        UIMessageBox msgbox = UIDialog.CreateDialog(prefabPath) as UIMessageBox;
        msgbox.m_TitleText.text = title;
        msgbox.m_MessageText.text = message;
        if (btn1 != null)
            msgbox.m_Button1.text = btn1;
        else
            msgbox.m_Button1.transform.parent.gameObject.SetActive(false);
        if (btn2 != null)
            msgbox.m_Button2.text = btn2;
        else
            msgbox.m_Button2.transform.parent.gameObject.SetActive(false);
        if (btn3 != null)
            msgbox.m_Button3.text = btn3;
        else
            msgbox.m_Button3.transform.parent.gameObject.SetActive(false);
        if (icon >= 0 && icon < msgbox.SpriteSet.Length)
            msgbox.m_IconImage.sprite = msgbox.SpriteSet[icon];
        else
            msgbox.m_IconImage.gameObject.SetActive(false);
        msgbox.Resp1 = resp1;
        msgbox.Resp2 = resp2;
        msgbox.Resp3 = resp3;

        Vector2 textPreferSize = new Vector2(msgbox.m_MessageText.preferredWidth, 
                                             msgbox.m_MessageText.preferredHeight);
        textPreferSize.x = (int)((textPreferSize.x + 2) / 2) * 2;
        textPreferSize.y = (int)((textPreferSize.y + 2) / 2) * 2;

        msgbox.m_MessageText.rectTransform.sizeDelta = textPreferSize;

        textPreferSize = new Vector2(msgbox.m_MessageText.preferredWidth,
                                             msgbox.m_MessageText.preferredHeight);
        textPreferSize.x = (int)((textPreferSize.x + 2) / 2) * 2;
        textPreferSize.y = (int)((textPreferSize.y + 2) / 2) * 2;

        float textOfs = Mathf.Min(0, (int)((textPreferSize.y - 56) / 2));
        msgbox.m_MessageText.rectTransform.anchoredPosition = new Vector2(msgbox.m_MessageText.rectTransform.anchoredPosition.x,
            textOfs + 20);

        if (msgbox.m_IconImage.gameObject.activeSelf)
        {
            msgbox.m_IconImage.rectTransform.anchoredPosition = new Vector2(
                msgbox.m_IconImage.rectTransform.anchoredPosition.x,
                msgbox.m_MessageText.rectTransform.anchoredPosition.y
            );
        }

        msgbox.m_WindowTrans.sizeDelta = new Vector2(
            Mathf.Max(msgbox.minWidth, textPreferSize.x + msgbox.paddingX),
            Mathf.Max(msgbox.minHeight, textPreferSize.y + msgbox.paddingY));

        PushMessage(msgbox);
        return msgbox;
    }


    public void OnButton1Click()
    {
        if (Resp1 != null)
            Resp1();
        PopMessage(this);
        FadeOut();
        Resp1 = null;
    }
    public void OnButton2Click()
    {
        if (Resp2 != null)
            Resp2();
        PopMessage(this);
        FadeOut();
        Resp2 = null;
    }
    public void OnButton3Click()
    {
        if (Resp3 != null)
            Resp3();
        PopMessage(this);
        FadeOut();
        Resp3 = null;
    }

    private void OnDestroy()
    {
        PopMessage(this);
    }
}
