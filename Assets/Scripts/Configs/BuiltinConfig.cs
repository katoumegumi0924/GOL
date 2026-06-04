using UnityEngine;

/// <summary>
/// BuiltinConfig：
/// </summary>
[CreateAssetMenu(fileName = "BuiltinConfig", menuName = "Configs/BuiltinConfig")]
public class BuiltinConfig : ScriptableObject
{
    [Header("单次迭代间隔Tick")]
    public int singleStepTick = 50;

    [Header("用于显示的Quad预制体")]
    public GameObject displayObj;

    [Header("存档路径")]
    public string savePath = "F:\\Cases\\GOL\\Save\\";

    [Header("默认分辨率")]
    public int resX = 100;
    public int resY = 100;
}
