using UnityEngine;

/// <summary>
/// TemplateData：
/// </summary>
public class TemplateData
{
    public string name;
    public Vector2Int[] cells;
    public int width;
    public int height;
    public int ruleIndex;
    

    public void Init()
    {
        name = "";
        cells = new Vector2Int[0];
        width = 0;
        height = 0;
        ruleIndex = 0;
    }

    public void Free()
    {
        name = null;
        cells = null;
        width = 0;
        height = 0;
        ruleIndex = 0;
    }
}
