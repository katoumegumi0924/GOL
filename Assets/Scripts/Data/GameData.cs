using UnityEngine;

/// <summary>
/// GameData：
/// </summary>
public class GameData
{
    public LifeData lifeData;
    public TimeData gameTime;

    public GameDesc gameDesc;

    public void Init()
    {
        gameTime = new TimeData();
        gameTime.Init();

        lifeData = new LifeData();
        lifeData.Init(this);

        gameDesc = Program.instance.gameDesc;
    }

    public void Free()
    {
        if (gameTime != null)
        {
            gameTime.Free();
            gameTime = null;
        }

        if (lifeData != null)
        {
            lifeData.Free();
            lifeData = null;
        }

        gameDesc = null;
    }

    public void SetNew()
    {
        gameTime.SetNew();
        lifeData.SetNew();
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        lifeData.Export(w);
        gameTime.Export(w);
        gameDesc.Export(w);
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        lifeData.Import(r);
        gameTime.Import(r);
        gameDesc.Import(r);
    }
}
