using UnityEngine;

/// <summary>
/// GameDesc：
/// </summary>
public class GameDesc
{
    public int resX;
    public int resY;

    public void Init()
    {
        // 默认分辨率为100x100
        resX = 100;
        resY = 100;
    }

    public void Free()
    {
        resX = 0;
        resY = 0;
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        w.Write(resX);
        w.Write(resY);
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        resX = r.ReadInt32();
        resY = r.ReadInt32();
    }
}
