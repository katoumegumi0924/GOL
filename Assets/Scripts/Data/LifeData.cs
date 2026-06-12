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

    public void Init(GameData _gameData)
    {
        gameData = _gameData;

        width = 100;
        height = 100;

        currentCellStates = new int[width * height];
        iterationRuleIndex = 0;
    }

    public void Free()
    {
        width = 0;
        height = 0;

        currentCellStates = null;
        iterationRuleIndex = 0;

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
    }
}
