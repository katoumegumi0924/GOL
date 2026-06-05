using System;
using UnityEngine;

/// <summary>
/// TimeData：
/// </summary>
public class TimeData
{
    public long tickCounter;

    // 每秒50帧，singleStepTick单次迭代所需的tick设置为50，当tickDelta为1时1秒迭代一次
    private readonly static int[] tickDeltaSteps = { 1, 2, 10, 20, 50 };
    private int tickDeltaIndex;
    public int tickDelta { get { return pausing ? 0 : tickDeltaSteps[tickDeltaIndex]; } }
    public bool pausing { get; private set; }
    public event Action<bool> OnPauseStateChanged;

    public void Init()
    {
        tickCounter = 0L;
        tickDeltaIndex = 1;
    }

    public void Free()
    {
        tickCounter = 0L;
        tickDeltaIndex = 0;
    }

    public void SetNew()
    {
        tickCounter = 0L;
        tickDeltaIndex = 1;
    }

    public void Export(System.IO.BinaryWriter w)
    {
        w.Write(0);

        w.Write(tickCounter);
        w.Write(tickDeltaIndex);
    }

    public void Import(System.IO.BinaryReader r)
    {
        int ver = r.ReadInt32();

        tickCounter = r.ReadInt64();
        tickDeltaIndex = r.ReadInt32();
    }

    public void SpeedUp()
    {
        tickDeltaIndex++;

        if (tickDeltaIndex >= tickDeltaSteps.Length)
            tickDeltaIndex = tickDeltaSteps.Length - 1;
    }

    public void SlowDown()
    {
        tickDeltaIndex--;

        if (tickDeltaIndex < 0)
            tickDeltaIndex = 0;
    }

    public void Pause()
    {
        if (pausing)
            return;
        pausing = true;
        OnPauseStateChanged?.Invoke(pausing);
    }

    public void Resume()
    {
        if (!pausing)
            return;
        pausing = false;
        OnPauseStateChanged?.Invoke(pausing);
    }

    public void TogglePause()
    {
        pausing = !pausing;
    }
}
