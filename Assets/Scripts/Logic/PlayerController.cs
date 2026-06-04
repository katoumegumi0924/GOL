using UnityEngine;

/// <summary>
/// PlayerController：
/// </summary>
public class PlayerController
{
    public Vector2 paintUV;
    public int paintValue;

    public void Init()
    {
        // 这种Init,Free,SetNew完全一样的的变量有必要显式的写出来吗
        paintUV = Vector2.zero;
        paintValue = 0;
    }

    public void Free()
    {
        paintUV = Vector2.zero;
        paintValue = 0;
    }

    public void SetNew()
    {
        paintUV = Vector2.zero;
        paintValue = 0;
    }

    public void Update()
    {
        HandlePaint();
    }

    public void HandlePaint()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButton(1);

        if (leftClick || rightClick)
        {
            paintUV = GetPaintUV();
            paintValue = leftClick ? 1 : 0;
        }
        else
        {
            paintUV = Vector2.negativeInfinity;
        }
    }

    public Vector2 GetPaintUV()
    {
        var displayObj = GameMain.instance.model.lifeRenderer.dislayObj;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == displayObj)
            {
                return hit.textureCoord;
            }
        }

        return Vector2.negativeInfinity;
    }
}
