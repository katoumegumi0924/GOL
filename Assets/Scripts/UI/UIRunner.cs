using UnityEngine;

/// <summary>
/// UIRunner：
/// </summary>
public class UIRunner : MonoBehaviour
{
    public UIRoot uiRoot;

    private void OnEnable()
    {
        uiRoot._Create();
        uiRoot._Init(null);
        uiRoot._Open();
    }

    private void OnDestroy()
    {
        uiRoot._Close();
        uiRoot._Free();
        uiRoot._Destroy();
    }

    private void Update()
    {
        uiRoot._Update();
    }

    private void LateUpdate()
    {
        uiRoot._LateUpdate();
    }
}
