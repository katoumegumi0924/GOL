using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// LifePatternParser：
/// </summary>
public static class LifeTemplateUtil
{
    public static RLEData ParseRLE(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        RLEData result = new RLEData();
        result.rule = "B3/S23";
        List<Vector2Int> activeCells = new List<Vector2Int>();
        string[] lines;

        try
        {
            lines = File.ReadAllLines(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"[ParseRLE] 读取文件时发生错误: {e.Message}");
            return result;
        }

        string data = "";
        Regex headerRegex = new Regex(@"x\s*=\s*(\d+)\s*,\s*y\s*=\s*(\d+)(?:\s*,\s*rule\s*=\s*([a-zA-Z0-9/]+))?", RegexOptions.IgnoreCase);

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("#N"))
            {
                result.name = trimmedLine.Substring(2).Trim();
                continue;
            }

            if (line.StartsWith("#"))
                continue;

            if (line.TrimStart().StartsWith("x", System.StringComparison.OrdinalIgnoreCase))
            {
                Match match = headerRegex.Match(line);
                if (match.Success)
                {
                    result.width = int.Parse(match.Groups[1].Value);
                    result.height = int.Parse(match.Groups[2].Value);

                    if (match.Groups[3].Success)
                    {
                        result.rule = match.Groups[3].Value;
                    }
                }
                continue;
            }

            data += line;
        }

        int curX = 0;
        int curY = 0;
        int count = 0;

        foreach (char c in data)
        {
            if (char.IsDigit(c))
            {
                count = count * 10 + (c - '0');
            }
            else
            {
                int cellCount = count > 0 ? count : 1;
                if (c == 'o')
                {
                    for (int i = 0; i < cellCount; ++i)
                    {
                        activeCells.Add(new Vector2Int(curX + i, curY));
                    }
                    curX += cellCount;
                }
                else if (c == 'b')
                {
                    curX += cellCount;
                }
                else if (c == '$')
                {
                    curY += cellCount;
                    curX = 0;
                }
                else if (c == '!')
                {
                    break;
                }

                count = 0;
            }
        }

        result.cells = TransferToCenter(activeCells);
        return result;
    }

    private static Vector2Int[] TransferToCenter(List<Vector2Int> cells)
    {
        if (cells.Count == 0)
            return new Vector2Int[0];

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (var cell in cells)
        {
            minX = Mathf.Min(minX, cell.x);
            maxX = Mathf.Max(maxX, cell.x);
            minY = Mathf.Min(minY, cell.y);
            maxY = Mathf.Max(maxY, cell.y);
        }

        int centerX = (minX + maxX) / 2;
        int centerY = (minY + maxY) / 2;

        Vector2Int[] relativeCenterPos = new Vector2Int[cells.Count];
        for (int i = 0; i < cells.Count; ++i)
        {
            relativeCenterPos[i] = new Vector2Int(cells[i].x - centerX, cells[i].y - centerY);
        }

        return relativeCenterPos;
    }

    public static RLEData[] LoadAllTemplateFile()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string templatePath = Path.Combine(projectRoot, "Templates");
        DirectoryInfo dir = new DirectoryInfo(templatePath);
        
        if (!dir.Exists)
            dir.Create();

        // 获取以.rle为后缀的存档文件
        FileInfo[] files = dir.GetFiles("*.rle");
        RLEData[] rleDatas = new RLEData[files.Length];
        for (int i = 0; i < files.Length; ++i)
        {
            var rleData = LifeTemplateUtil.ParseRLE(files[i].FullName);

            if (rleData != null)
            {
                rleDatas[i] = rleData;
            }
        }

        return rleDatas;
    }

    public static void RotatePattern(Vector2Int[] cellData, float degree)
    {
        float theta = degree * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);

        for (int i = 0; i < cellData.Length; ++i)
        {
            int x = cellData[i].x;
            int y = cellData[i].y;

            cellData[i].x = Mathf.RoundToInt(x * cosTheta - y * sinTheta);
            cellData[i].y = Mathf.RoundToInt(x * sinTheta + y * cosTheta);
        }
    }
}

public class RLEData
{
    public string name;
    public int width;
    public int height;
    public string rule;
    public Vector2Int[] cells;

    public void Init()
    {

    }

    public void Free()
    {
        name = null;
        width = 0;
        height = 0;
        rule = null;
        cells = null;
    }
}