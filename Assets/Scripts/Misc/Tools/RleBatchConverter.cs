using System.IO;
using UnityEngine;

/// <summary>
/// Rle：
/// </summary>
public class RleBatchConverter : MonoBehaviour
{
    private void Start()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string rleTemplatesPath = Path.Combine(projectRoot, "Rle Templates");

        if (Directory.Exists(rleTemplatesPath))
        {
            string[] rleFiles = Directory.GetFiles(rleTemplatesPath, "*.rle", SearchOption.AllDirectories);

            foreach (string file in rleFiles)
            {
                RleTransferTxt.ConvertRleFileToCells(file);
            }

            Debug.Log($"一键转换完毕，共处理 {rleFiles.Length} 个文件！");
        }
    }
}
