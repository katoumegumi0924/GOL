using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

/// <summary>
/// RleTransferTxt：
/// </summary>
public static class RleTransferTxt
{
    /// <summary>
    /// 读取 RLE 文件并直接在同级目录下生成同名的 .cells 文件
    /// </summary>
    public static void ConvertRleFileToCells(string rleFilePath)
    {
        if (!File.Exists(rleFilePath))
        {
            Debug.LogError($"[FormatConverter] 找不到文件: {rleFilePath}");
            return;
        }

        string rleText = File.ReadAllText(rleFilePath);
        string plaintextData = ConvertRleToPlaintext(rleText);

        // 替换后缀名
        string outputPath = Path.ChangeExtension(rleFilePath, ".txt");

        File.WriteAllText(outputPath, plaintextData);
        Debug.Log($"[FormatConverter] 转换成功！文件已保存至: {outputPath}");
    }

    /// <summary>
    /// 核心翻译逻辑：RLE 字符串 -> Plaintext 字符串
    /// </summary>
    public static string ConvertRleToPlaintext(string rleText)
    {
        StringBuilder plaintext = new StringBuilder();
        string name = "Converted Pattern";
        string rule = "B3/S23";
        string data = "";

        // 1. 提取元数据和数据体
        string[] lines = rleText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Regex headerRegex = new Regex(@"x\s*=\s*\d+.*rule\s*=\s*([a-zA-Z0-9/]+)", RegexOptions.IgnoreCase);

        foreach (string line in lines)
        {
            string trimmed = line.Trim();

            if (trimmed.StartsWith("#N", System.StringComparison.OrdinalIgnoreCase))
            {
                name = trimmed.Substring(2).Trim();
            }
            else if (trimmed.StartsWith("#"))
            {
                // 其他类型的注释（如作者、描述），可以选择忽略或转换为 ! 注释
                continue;
            }
            else if (trimmed.StartsWith("x", System.StringComparison.OrdinalIgnoreCase))
            {
                Match match = headerRegex.Match(trimmed);
                if (match.Success)
                {
                    rule = match.Groups[1].Value;
                }
            }
            else
            {
                // 拼接 RLE 的字符体
                data += trimmed;
            }
        }

        // 2. 写入 Plaintext 头部规范
        plaintext.AppendLine($"!Name: {name}");
        plaintext.AppendLine($"!Rule: {rule}");
        plaintext.AppendLine("!"); // 空注释行作为头部和图形的分割线

        // 3. 逐字解码 RLE 游程压缩
        int count = 0;
        foreach (char c in data)
        {
            if (char.IsDigit(c))
            {
                // 累加前面的数字 (例如 "12" -> 1*10+2 = 12)
                count = count * 10 + (c - '0');
            }
            else
            {
                int runLength = count > 0 ? count : 1;

                if (c == 'b')
                {
                    // 生成 runLength 个点
                    plaintext.Append(new string('.', runLength));
                }
                else if (c == 'o')
                {
                    // 生成 runLength 个 O
                    plaintext.Append(new string('O', runLength));
                }
                else if (c == '$')
                {
                    // RLE 中可能会出现 3$ 这种连续空多行的情况
                    for (int i = 0; i < runLength; i++)
                    {
                        plaintext.AppendLine();
                    }
                }
                else if (c == '!')
                {
                    // 解析结束符
                    break;
                }

                // 字符处理完毕，数字归零
                count = 0;
            }
        }

        return plaintext.ToString();
    }
}
