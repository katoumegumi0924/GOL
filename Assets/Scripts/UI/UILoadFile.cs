using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class UILoadFile : ManualBehavior
{
    public Transform content;
    public UISaveEntry uiSaveEntryPrefab;
    public UIButton closeBtn;
    public RectTransform rect;

    public List<UISaveEntry> saveEntries;

    protected override bool _OnInit()
    {
        saveEntries = new List<UISaveEntry>();

        return true;
    }

    protected override void _OnFree()
    {
        if (saveEntries != null)
        {
            for (int i = 0; i < saveEntries.Count; ++i)
            {
                saveEntries[i]._Free();
            }

            saveEntries.Clear();
            saveEntries = null;
        }
        
    }

    protected override void _OnOpen()
    {
        RefreshList();
    }

    protected override void _OnClose()
    {
        
    }

    protected override void _OnRegEvent()
    {
        closeBtn.onClick += OnCloseBtnClick;
    }

    protected override void _OnUnregEvent()
    {
        closeBtn.onClick -= OnCloseBtnClick;
    }

    protected override void _OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
            {
                _Close();
                return;
            }
        }
    }

    public void RefreshList()
    {
        FileInfo[] files = GetAllSaveFiles();

        for (int i = 0; i < files.Length; ++i)
        {
            if (i >= saveEntries.Count)
            {
                UISaveEntry newFileEntry = UISaveEntry.Instantiate(uiSaveEntryPrefab, content, false);
                newFileEntry._Create();
                newFileEntry._Init(null);
                saveEntries.Add(newFileEntry);
            }

            var entry = saveEntries[i];
            Texture2D preImage = GameSave.LoadPreviewImage(files[i].FullName);
            var saveInfo = new SaveEntryInfo(files[i].FullName, Path.GetFileNameWithoutExtension(files[i].FullName), preImage);
            entry.SetData(saveInfo);
            if (this.active)
                entry._Open();
        }

        for (int i = files.Length; i < saveEntries.Count; ++i)
        {
            saveEntries[i]._Close();
        }
    }

    // 获取存档文件
    private FileInfo[] GetAllSaveFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(GameSave.GetDocumentSavePath());
        if (!dir.Exists)
            dir.Create();

        // 获取以.gol为后缀的存档文件
        FileInfo[] files = dir.GetFiles($"*{GameSave.saveExt}");
        return files;
    }

    public void OnCloseBtnClick(int obj)
    {
        _Close();
    }
}
