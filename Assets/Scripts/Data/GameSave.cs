using System.IO;
using UnityEngine;

/// <summary>
/// GameSave：
/// </summary>
public static class GameSave
{
    public static readonly string saveExt = ".gol";

    public static void SaveGame(string saveName, GameData gameData)
    {
        saveName = VaildFileName(saveName);

        string path = Configs.builtin.savePath + saveName + saveExt;
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        int[] cellStates = GameMain.instance.data.lifeData.currentCellStates;
        var cellBuffer = GameMain.instance.logic.lifeLogic.outputBuffer;

        cellBuffer.GetData(cellStates);

        if (File.Exists(path))
        {
            UIMessageBox.Show("文件已存在", "该存档名称已存在，是否覆盖？", "确认覆盖", "取消", -1,
                              () => ExecuteSave(path, gameData, cellStates), null);
        }
        else
        {
            ExecuteSave(path, gameData, cellStates);
        }
    }

    public static void LoadGame(string saveName, GameData gameData)
    {
        saveName = VaildFileName(saveName);

        string path = Configs.builtin.savePath + saveName + saveExt;

        // 验证文件是否存在
        if (!File.Exists(path))
        {
            Debug.LogError("存档文件不存在");
            return;
        }

        ExecuteLoad(path, gameData);
    }

    public static Texture2D LoadPreviewImage(string path)
    {
        string imgPath = Path.ChangeExtension(path, ".PNG");

        if (File.Exists(imgPath))
        {
            byte[] bytes = File.ReadAllBytes(imgPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            return tex;
        }

        return null;
    }

    public static void SavePreviewImage(string path, int[] cellStates)
    {
        int resX = GameMain.instance.data.lifeData.resX;
        int resY = GameMain.instance.data.lifeData.resY;
        var cellBuffer = GameMain.instance.logic.lifeLogic.outputBuffer;

        Texture2D tex = new Texture2D(resX, resY, TextureFormat.R8, false);
        byte[] pixels = new byte[resX * resY];
        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = (byte)(cellStates[i] == 0 ? 255 : 0);
        }

        tex.SetPixelData(pixels, 0);
        tex.Apply();

        byte[] pngData = tex.EncodeToPNG();
        string imgPath = Path.ChangeExtension(path, ".PNG");
        File.WriteAllBytes(imgPath, pngData);

        Texture2D.Destroy(tex);
    }

    public static void DeleteSave(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string imgPath = Path.ChangeExtension(path, ".PNG");
        if (File.Exists(path))
        {
            File.Delete(imgPath);
        }

#if UNITY_EDITOR
        Debug.Log($"已删除存档:{path}");
#endif
    }

    // 将输入文件名中的非法字符改为' '
    public static string VaildFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return fileName;

        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            fileName = fileName.Replace(c, ' ');
        }

        return fileName;
    }

    private static void ExecuteSave(string path, GameData gameData, int[] cellStates)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter w = new BinaryWriter(fs))
                {
                    // 存档文件的标识符
                    w.Write('G');
                    w.Write('O');
                    w.Write('L');

                    gameData.Export(w);

                    SavePreviewImage(path, cellStates);

#if UNITY_EDITOR
                    Debug.Log("存档保存成功！");
#endif
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"保存存档时发生异常: {ex.Message}");
        }
    }

    private static void ExecuteLoad(string path, GameData gameData)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    // 验证文件的标识符
                    bool isValid = true;
                    isValid = isValid && (r.ReadChar() == 'G');
                    isValid = isValid && (r.ReadChar() == 'O');
                    isValid = isValid && (r.ReadChar() == 'L');

                    if (!isValid)
                    {
                        Debug.LogError("游戏存档并不合法");
                        return;
                    }

                    gameData.Import(r);
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"保存存档时发生异常: {ex.Message}");
        }
    }
}
