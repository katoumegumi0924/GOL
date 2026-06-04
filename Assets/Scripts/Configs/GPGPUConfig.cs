using UnityEngine;

/// <summary>
/// GPGPUConfig：
/// </summary>
[CreateAssetMenu(fileName = "GPGPUConfig", menuName = "Configs/GPGPUConfig")]
public class GPGPUConfig : ScriptableObject
{
    public ComputeShader lifeShader;
}
