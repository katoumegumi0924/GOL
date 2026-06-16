using UnityEngine;

/// <summary>
/// Configs：
/// </summary>
public static class Configs
{
    private static BuiltinConfig _builtin;
    public static BuiltinConfig builtin
    {
        get
        {
            if (_builtin == null)
            {
                _builtin = Resources.Load<BuiltinConfig>("Configs/Builtin Config");

                if (_builtin == null)
                {
                    Debug.LogError("错误：在 Resources 文件夹下找不到名为 'Builtin Config' 的配置文件！");
                }
            }

            return _builtin;
        }
    }

    private static GPGPUConfig _GPGPU;
    public static GPGPUConfig GPGPU
    {
        get
        {
            if (_GPGPU == null)
            {
                _GPGPU = Resources.Load<GPGPUConfig>("Configs/GPGPU Config");

                if (_GPGPU == null)
                {
                    Debug.LogError("错误：在 Resources 文件夹下找不到名为 'GPGPU Config' 的配置文件！");
                }
            }

            return _GPGPU;
        }
    }
}
