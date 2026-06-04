using System;
using UnityEngine;
using UnityEngine.UI;

public class UISaveEntry : ManualBehavior
{
    public Text saveNameText;
    public RawImage previewImage;
    public UIButton LoadButton;
    public UIButton deleteButton;

    public string path;
    public Action<string> onLoadAction;
    public Action<string> onDeleteAction;
    public SaveEntryInfo currentEntryInfo;

    protected override void _OnRegEvent()
    {
        LoadButton.onClick += OnLoadClick;
        deleteButton.onClick += OnDeleteClick;
    }

    protected override void _OnUnregEvent()
    {
        LoadButton.onClick -= OnLoadClick;
        deleteButton.onClick -= OnDeleteClick;
    }

    private void OnLoadClick(int data)
    {
        Program.instance.LoadGame(currentEntryInfo.saveName); 
    }

    private void OnDeleteClick(int data)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            return;

        GameSave.DeleteSave(path);
        UIRoot.instance.uiMainMenu.uiLoadFile.RefreshList();
    }

    // 用于为已经初始化的对象更新显示数据
    public void SetData(SaveEntryInfo info)
    {
        // 清理旧数据
        if (currentEntryInfo != null && currentEntryInfo.previewTex != null)
        {
            if (currentEntryInfo.previewTex != info.previewTex)
            {
                Texture2D.Destroy(currentEntryInfo.previewTex);
            }
        }

        currentEntryInfo = info;
        path = currentEntryInfo.path;

        if (saveNameText != null)
        {
            saveNameText.text = currentEntryInfo.saveName;
        }

        if (previewImage != null && currentEntryInfo.previewTex != null)
        {
            previewImage.texture = currentEntryInfo.previewTex;
        }
        else if (previewImage != null)
        {
            previewImage.texture = null;
            previewImage.color = Color.white;
        }
    }
}

public class SaveEntryInfo
{
    public string path;
    public string saveName;
    public Texture2D previewTex;

    public SaveEntryInfo(string _path, string _saveName, Texture2D _previewTex)
    {
        path = _path;
        saveName = _saveName;
        previewTex = _previewTex;
    }
}