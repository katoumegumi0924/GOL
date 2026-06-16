using UnityEngine;

/// <summary>
/// GameData：
/// </summary>
public class GameData
{
    public TimeData gameTime;
    public LifeData lifeData;
    
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
        gameDesc = null;

        if (lifeData != null)
        {
            lifeData.Free();
            lifeData = null;
        }

        if (gameTime != null)
        {
            gameTime.Free();
            gameTime = null;
        }   
    }

    public void SetNew()
    {
        gameTime.SetNew();
        lifeData.SetNew();
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        gameTime.Export(w);
        lifeData.Export(w);
        gameDesc.Export(w);
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        gameTime.Import(r);
        lifeData.Import(r);
        gameDesc.Import(r);
    }
}
