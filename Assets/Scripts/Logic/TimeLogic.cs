using UnityEngine;

/// <summary>
/// TimeLogic：
/// </summary>
public class TimeLogic
{
    public TimeData lifeTime;

    public void Init(TimeData _lifeTime)
    {
        lifeTime = _lifeTime;
    }

    public void Free()
    {
        lifeTime = null;
    }

    public void Tick()
    {
        lifeTime.tickCounter += lifeTime.tickDelta;
    }
}
