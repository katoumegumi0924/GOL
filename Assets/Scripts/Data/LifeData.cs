using UnityEngine;

/// <summary>
/// LifeData：
/// </summary>
public class LifeData
{
    public int resX;
    public int resY;

    public int[] currentCellStates;

    public int iterationRuleIndex;

    public void Init()
    {

    }

    public void Free()
    {
        currentCellStates = null;
    }

    public void SetNew()
    {
        resX = GameDesc.resX;
        resY = GameDesc.resY;

        currentCellStates = new int[resX * resY];

        for (int i = 0; i < currentCellStates.Length; ++i)
        {
            currentCellStates[i] = Random.Range(0, 2);
        }
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        w.Write(resX);
        w.Write(resY);

        int length = currentCellStates.Length;
        w.Write(length);
        for (int i = 0; i < length; ++i)
        {
            w.Write(currentCellStates[i]);
        }
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        resX = r.ReadInt32();
        resY = r.ReadInt32();

        int length = r.ReadInt32();
        currentCellStates = new int[length];
        for (int i = 0; i < length; ++i)
        {
            currentCellStates[i] = r.ReadInt32();
        }
    }
}
