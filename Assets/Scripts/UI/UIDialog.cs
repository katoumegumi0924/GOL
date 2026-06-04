using UnityEngine;

/// <summary>
/// UIDialog：
/// </summary>
public class UIDialog : MonoBehaviour
{
    [SerializeField] public CanvasGroup m_Mask;
    [SerializeField] public RectTransform m_Window;

    float _direction = 1;
    float _fadefactor = 0;

    public float FadeTime = 0.16f;
    public AnimationCurve FadeCurve = AnimationCurve.Linear(0, 0, 0, 1);

    public void FadeIn()
    {
        _direction = 1;
    }

    public void FadeOut()
    {
        _direction = -1;
    }

    protected void Start()
    {
        m_Mask.alpha = 0.005f;
        m_Window.localScale = Vector3.one * FadeCurve.Evaluate(0.01f);
    }

    protected void Update()
    {
        if (FadeTime > 0)
        {
            _fadefactor += (_direction / FadeTime) * Time.deltaTime;
        }
        else
            _fadefactor = _direction;

        _fadefactor = Mathf.Clamp01(_fadefactor);

        if (_fadefactor == 0)
            GameObject.Destroy(this.gameObject);

        m_Mask.alpha = _fadefactor;
        m_Window.localScale = Vector3.one * FadeCurve.Evaluate(_fadefactor);
    }

    public static UIDialog CreateDialog(string prefabPath)
    {
        if (UIDialogGroup.Instance == null)
            return null;

        GameObject prefab = Resources.Load(prefabPath) as GameObject;
        if (prefab != null)
        {
            GameObject dialog_go = GameObject.Instantiate(prefab, UIDialogGroup.Instance.transform);
            UIDialog dialog = dialog_go.GetComponent<UIDialog>();
            if (dialog != null)
            {
                RectTransform rect = dialog.transform as RectTransform;
                if (rect != null)
                {
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;

                    dialog_go.transform.SetAsLastSibling();

                    return dialog;
                }
            }
        }

        return null;
    }
}
