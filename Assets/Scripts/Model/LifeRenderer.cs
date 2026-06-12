using UnityEngine;

/// <summary>
/// LifeRenderer：
/// </summary>
public class LifeRenderer
{
    public LifeData lifeData;
    public LifeLogic lifeLogic;

    public GameObject dislayObj;
    public MeshRenderer displayRender;
    public Material displayMaterial;

    public void Init(LifeData _lifeData, LifeLogic _lifeLogic)
    {
        lifeData = _lifeData;
        lifeLogic = _lifeLogic;

        dislayObj = CreateDisplay();
        displayRender = dislayObj.GetComponent<MeshRenderer>();
        displayMaterial = displayRender.material;

        displayMaterial.SetVector("_Res", new Vector2(lifeData.width, lifeData.height));
    } 

    public void Free()
    {
        displayMaterial = null;
        displayRender = null;
        if (dislayObj != null)
        {
            GameObject.Destroy(dislayObj);
            dislayObj = null;
        }

        lifeLogic = null;
        lifeData = null;
    }

    public void Update()
    {
        displayMaterial.SetBuffer("_CellStateBuffer", lifeLogic.outputBuffer);
        displayMaterial.SetBuffer("_PreviewBuffer", lifeLogic.previewBuffer);
    }

    private GameObject CreateDisplay()
    {
        GameObject dislayObj = GameObject.Instantiate(Configs.builtin.displayObj, Vector3.zero, Quaternion.identity);

        float resX = lifeData.width;
        float resY = lifeData.height;

        float scaleY = 10f;
        float scaleX = scaleY * (resX / resY);

        dislayObj.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        return dislayObj;
    }

    public void ShowGrid(bool showGrid)
    {
        displayMaterial.SetFloat("_ShowGrid", showGrid ? 1.0f : 0.0f);
    }
}
