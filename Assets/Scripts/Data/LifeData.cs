using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LifeData：
/// </summary>
public class LifeData
{
    public GameData gameData;

    public int width;
    public int height;

    public int[] currentCellStates;
    public int iterationRuleIndex;

    // 模板数据
    public RLEData[] rleDatas;
    public List<RLEData> ruleRleDatas;

    public void Init(GameData _gameData)
    {
        gameData = _gameData;

        width = 100;
        height = 100;

        currentCellStates = new int[width * height];
        iterationRuleIndex = 0;

        rleDatas = LifeTemplateUtil.LoadAllTemplateFile();
        ruleRleDatas = GetCurRuleRleData(iterationRuleIndex);
    }

    public void Free()
    {
        width = 0;
        height = 0;

        currentCellStates = null;
        iterationRuleIndex = 0;

        if (ruleRleDatas != null)
        {
            for (int i = 0; i < ruleRleDatas.Count; ++i)
            {
                ruleRleDatas[i].Free();
                ruleRleDatas[i] = null;
            }
        }

        if (rleDatas != null)
        {
            for (int i = 0; i < rleDatas.Length; ++i)
            {
                rleDatas[i].Free();
                rleDatas[i] = null;
            }

            rleDatas = null;
        }

        gameData = null;
    }

    public void SetNew()
    {
        width = gameData.gameDesc.resX;
        height = gameData.gameDesc.resY;

        currentCellStates = new int[width * height];
        for (int i = 0; i < currentCellStates.Length; ++i)
        {
            currentCellStates[i] = Random.Range(0, 2);
        }

        iterationRuleIndex = 0;
        ruleRleDatas = GetCurRuleRleData(iterationRuleIndex);
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        w.Write(width);
        w.Write(height);

        int length = currentCellStates.Length;
        w.Write(length);
        for (int i = 0; i < length; ++i)
        {
            w.Write(currentCellStates[i]);
        }

        w.Write(iterationRuleIndex);
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        width = r.ReadInt32();
        height = r.ReadInt32();

        int length = r.ReadInt32();
        currentCellStates = new int[length];
        for (int i = 0; i < length; ++i)
        {
            currentCellStates[i] = r.ReadInt32();
        }

        iterationRuleIndex = r.ReadInt32();

        ruleRleDatas = GetCurRuleRleData(iterationRuleIndex);
    }

    public List<RLEData> GetCurRuleRleData(int iterationRuleIndex)
    {
        List<RLEData> result = new List<RLEData>();
        var lifeRuleSet = Protos.ruleSet;

        for (int i = 0; i < rleDatas.Length; ++i)
        {
            int templateRuleIndex = lifeRuleSet.GetLifeRuleIndex(rleDatas[i].rule);
            if (templateRuleIndex == iterationRuleIndex)
                result.Add(rleDatas[i]);
        }

        return result;
    }
}
