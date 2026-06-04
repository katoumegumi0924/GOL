using UnityEngine;

/// <summary>
/// GameData：
/// </summary>
public class GameData
{
    public LifeData lifeData;
    public TimeData lifeTime;

    public void Init()
    {
        lifeData = new LifeData();
        lifeData.Init();

        lifeTime = new TimeData();
        lifeTime.Init();
    }

    public void Free()
    {
        if (lifeData != null)
        {
            lifeData.Free();
            lifeData = null;
        }

        if (lifeTime != null)
        {
            lifeTime.Free();
            lifeTime = null;
        }
    }

    public void SetNew()
    {
        lifeData.SetNew();
        lifeTime.SetNew();
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        lifeData.Export(w);
        lifeTime.Export(w);
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        lifeData.Import(r);
        lifeTime.Import(r);
    }
}
