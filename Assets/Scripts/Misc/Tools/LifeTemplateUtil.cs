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
    public static TemplateData ParseTxt(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        TemplateData result = new TemplateData();
        result.ruleIndex = 0;
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

        Regex ruleRegex = new Regex(@"!\s*rule\s*[:=]?\s*([a-zA-Z0-9/]+)", RegexOptions.IgnoreCase);

        int currentY = 0;
        int maxWidth = 0;
        Vector2Int? hotspot = null;
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("!"))
            {
                Match ruleMatch = ruleRegex.Match(trimmedLine);
                if (ruleMatch.Success)
                {
                    result.ruleIndex = Protos.ruleSet.GetLifeRuleIndex(ruleMatch.Groups[1].Value);
                    continue;
                }

                if (trimmedLine.StartsWith("!name:", System.StringComparison.OrdinalIgnoreCase))
                {
                    result.name = trimmedLine.Substring(6).Trim();
                }

                continue;
            }

            maxWidth = Mathf.Max(maxWidth, trimmedLine.Length);

            for (int x = 0; x < trimmedLine.Length; ++x)
            {
                char c = trimmedLine[x];

                if (c == '#')
                {
                    hotspot = new Vector2Int(x, currentY);
                    activeCells.Add(new Vector2Int(x, currentY));
                }
                else if (c == 'O' || c == 'o' || c == '*')
                {
                    activeCells.Add(new Vector2Int(x, currentY));
                }
            }

            currentY++;
        }

        if (string.IsNullOrEmpty(result.name))
            result.name = "未命名模板";

        result.width = maxWidth;
        result.height = currentY;
        result.cells = TransferCenterToHotspot(activeCells, hotspot);

        return result;
    }

    private static Vector2Int[] TransferCenterToHotspot(List<Vector2Int> cells, Vector2Int? hotspot)
    {
        if (cells.Count == 0)
            return new Vector2Int[0];

        int centerX = 0;
        int centerY = 0;

        if (hotspot.HasValue)
        {
            centerX = hotspot.Value.x;
            centerY = hotspot.Value.y;
        }
        else
        {
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;
            foreach (var cell in cells)
            {
                minX = Mathf.Min(minX, cell.x);
                maxX = Mathf.Max(maxX, cell.x);
                minY = Mathf.Min(minY, cell.y);
                maxY = Mathf.Max(maxY, cell.y);
            }
            centerX = (minX + maxX) / 2;
            centerY = (minY + maxY) / 2;
        }

        Vector2Int[] relativeCenterPos = new Vector2Int[cells.Count];
        for (int i = 0; i < cells.Count; ++i)
        {
            relativeCenterPos[i] = new Vector2Int(cells[i].x - centerX, centerY - cells[i].y);
        }

        return relativeCenterPos;
    }

    public static TemplateData[] LoadAllTemplateFile()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string templatePath = Path.Combine(projectRoot, "Templates");
        DirectoryInfo dir = new DirectoryInfo(templatePath);

        if (!dir.Exists)
            dir.Create();

        FileInfo[] files = dir.GetFiles("*.txt");
        TemplateData[] templateDatas = new TemplateData[files.Length];
        for (int i = 0; i < files.Length; ++i)
        {
            var templateData = LifeTemplateUtil.ParseTxt(files[i].FullName);

            if (templateData != null)
            {
                templateDatas[i] = templateData;
            }
        }

        return templateDatas;
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

    public static List<TemplateData> GetCurRuleRleData(int iterationRuleIndex, TemplateData[] templateDatas)
    {
        List<TemplateData> result = new List<TemplateData>();
        var lifeRuleSet = Protos.ruleSet;

        for (int i = 0; i < templateDatas.Length; ++i)
        {
            if (templateDatas[i].ruleIndex == iterationRuleIndex)
                result.Add(templateDatas[i]);
        }

        return result;
    }
}