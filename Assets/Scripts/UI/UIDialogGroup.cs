using UnityEngine;

/// <summary>
/// UIDialogGroup：
/// </summary>
public class UIDialogGroup : MonoBehaviour
{
    public static UIDialogGroup Instance { get; private set; }

    public static bool HasDialog
    {
        get
        {
            if (Instance == null)
                return false;

            return Instance.transform.childCount > 0;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    protected void Update()
    {
        transform.SetAsLastSibling();
    }
}
