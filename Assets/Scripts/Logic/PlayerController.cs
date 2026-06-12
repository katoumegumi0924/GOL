using UnityEngine;

/// <summary>
/// PlayerController：
/// </summary>
public class PlayerController
{
    public Vector2 paintPos;
    public int paintValue;

    public void Init()
    {

    }

    public void Free()
    {
        paintPos = Vector2.zero;
        paintValue = 0;
    }

    public void SetNew()
    {

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
            paintPos = GetPaintPos();
            paintValue = leftClick ? 1 : 0;
        }
        else
        {
            paintPos = Vector2.negativeInfinity;
        }
    }

    public Vector2 GetPaintPos()
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
