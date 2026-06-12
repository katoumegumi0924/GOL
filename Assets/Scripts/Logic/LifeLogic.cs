using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// LifeLogic：
/// </summary>
public class LifeLogic
{
    public LifeData lifeData;
    public TimeData lifeTime;
    public PlayerController playerController;

    public ComputeShader lifeComputer;
    public ComputeBuffer inputBuffer;
    public ComputeBuffer outputBuffer;
    public ComputeBuffer previewBuffer;
    public ComputeBuffer templateBuffer;

    public LifeRule lifeRule;
    public int templateIndex;

    private int updateKernel;
    private int paintKernel;
    private int templateKernel;
    private int previewKernel;
    private int clearPreviewKernel;
    private int accumulator;

    private Vector2 _lastFramePos;
    private int curretnTemplateBufferSize = -1;

    // 初始状态
    private int[] initialCellStates;

    // 模板数据
    public RLEData[] rleDatas;

    public void Init(LifeData _lifeData, TimeData _lifeTime, PlayerController _playerController)
    {
        lifeData = _lifeData;
        lifeTime = _lifeTime;
        playerController = _playerController;
        lifeComputer = Configs.GPGPU.lifeComputer;

        updateKernel = lifeComputer.FindKernel("CSUpdate");
        paintKernel = lifeComputer.FindKernel("CSPaint");
        templateKernel = lifeComputer.FindKernel("CSDrawTemplate");
        previewKernel = lifeComputer.FindKernel("CSDrawPreview");
        clearPreviewKernel = lifeComputer.FindKernel("CSClearPreview");

        lifeRule = Configs.ruleSet.GetLifeRule(lifeData.iterationRuleIndex);
        templateIndex = 0;

        initialCellStates = new int[lifeData.currentCellStates.Length];
        Array.Copy(lifeData.currentCellStates, initialCellStates, initialCellStates.Length);

        rleDatas = LifeTemplateUtil.LoadAllTemplateFile();
    }

    public void Free()
    {
        lifeData = null;
        lifeTime = null;
        playerController = null;
        lifeComputer = null;

        updateKernel = 0;
        paintKernel = 0;
        templateKernel = 0;
        previewKernel = 0;
        clearPreviewKernel = 0;

        lifeRule = null;
        templateIndex = 0;
        initialCellStates = null;

        if (rleDatas != null)
        {
            for (int i = 0; i < rleDatas.Length; ++i)
            {
                rleDatas[i].Free();
                rleDatas[i] = null;
            }

            rleDatas = null;
        }

        if (inputBuffer != null)
        {
            inputBuffer.Release();
            inputBuffer = null;
        }

        if (outputBuffer != null)
        {
            outputBuffer.Release();
            outputBuffer = null;
        }

        if (previewBuffer != null)
        {
            previewBuffer.Release();
            previewBuffer = null;
        }

        if (templateBuffer != null)
        {
            templateBuffer.Release();
            templateBuffer = null;
        }
    }

    public void SetNew()
    {
        inputBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));
        outputBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));
        previewBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));

        inputBuffer.SetData(lifeData.currentCellStates);
        outputBuffer.SetData(lifeData.currentCellStates);

        lifeRule = Configs.ruleSet.GetLifeRule(lifeData.iterationRuleIndex);
        templateIndex = 0;

        initialCellStates = new int[lifeData.currentCellStates.Length];
        Array.Copy(lifeData.currentCellStates, initialCellStates, initialCellStates.Length);

        SetDisplaySize();
    }

    public void AfterImport()
    {
        inputBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));
        outputBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));
        previewBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));

        inputBuffer.SetData(lifeData.currentCellStates);
        outputBuffer.SetData(lifeData.currentCellStates);

        lifeRule = Configs.ruleSet.GetLifeRule(lifeData.iterationRuleIndex);
        templateIndex = 0;

        initialCellStates = new int[lifeData.currentCellStates.Length];
        Array.Copy(lifeData.currentCellStates, initialCellStates, initialCellStates.Length);

        SetDisplaySize();
    }

    public void SetDisplaySize()
    {
        var lifeRenderer = GameMain.instance.model.lifeRenderer;

        float resX = lifeData.width;
        float resY = lifeData.height;

        float scaleY = 10f;
        float scaleX = scaleY * (resX / resY);

        lifeRenderer.dislayObj.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        lifeRenderer.displayMaterial.SetVector("_Res", new Vector2(lifeData.width, lifeData.height));
    }

    public void Tick()
    {
        accumulator += lifeTime.tickDelta;
        while (accumulator >= Configs.builtin.singleStepTick)
        {
            LifeTick();
            accumulator -= Configs.builtin.singleStepTick;
        }
    }

    public void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (templateIndex == 0)
            {
                Paint();
                ClearPreview();
            }
            else
            {
                LoadTemplate();
            }     
        }
    }

    public void LifeTick()
    {
        ComputeBuffer temp = inputBuffer;
        inputBuffer = outputBuffer;
        outputBuffer = temp;

        lifeComputer.SetBuffer(updateKernel, "_Input", inputBuffer);
        lifeComputer.SetBuffer(updateKernel, "_Output", outputBuffer);

        lifeComputer.SetInt("_ResX", lifeData.width);
        lifeComputer.SetInt("_ResY", lifeData.height);
        lifeComputer.SetInt("_SurvivalMask", lifeRule.survivalMask);
        lifeComputer.SetInt("_BirthMask", lifeRule.birthMask);

        int groupsX = Mathf.CeilToInt(lifeData.width / 8.0f);
        int groupsY = Mathf.CeilToInt(lifeData.height / 8.0f);
        lifeComputer.Dispatch(updateKernel, groupsX, groupsY, 1);  
    }

    public void Paint()
    {
        var paintPos = playerController.paintPos;
        var paintValue = playerController.paintValue;

        if (!float.IsNegativeInfinity(paintPos.x))
        {
            float stepSize = Mathf.Max(lifeData.width, lifeData.height);

            if (float.IsNegativeInfinity(_lastFramePos.x) || Vector2.Distance(_lastFramePos, paintPos) < 1 / stepSize)
            {
                PaintCell(paintPos, paintValue);
            }
            else
            {
                PaintLine(_lastFramePos, paintPos, paintValue);
            }

            _lastFramePos = paintPos;
        }
        else
        {
            _lastFramePos = Vector2.negativeInfinity;
        }
    }

    public void PaintCell(Vector2 paintPos, int paintValue)
    {
        if (!float.IsNegativeInfinity(paintPos.x))
        {
            lifeComputer.SetInt("_PaintValue", paintValue);
            lifeComputer.SetFloat("_PaintPosX", paintPos.x);
            lifeComputer.SetFloat("_PaintPosY", paintPos.y);
            lifeComputer.SetBuffer(paintKernel, "_Output", outputBuffer);

            lifeComputer.Dispatch(paintKernel, 1, 1, 1);
        }
    }

    public void PaintLine(Vector2 start, Vector2 end, int paintValue)
    {
        float dist = Vector2.Distance(start, end);

        float stepSize = Mathf.Max(lifeData.width, lifeData.height);
        int step = Mathf.CeilToInt(stepSize * dist);

        for (int i = 0; i < step; ++i)
        {
            float t = (float)i / step;
            Vector2 paintUV = Vector2.Lerp(start, end, t);
            PaintCell(paintUV, paintValue);
        }
    }

    public void LoadTemplate()
    {
        var paintPos = playerController.GetPaintPos();

        if (!float.IsNegativeInfinity(paintPos.x) && templateIndex > 0)
        {
            var rleData = rleDatas[templateIndex - 1];
            var templateData = rleData.cells;

            // 根据模板设置迭代规则
            var templateRule = Configs.ruleSet.GetLifeRuleIndex(rleData.rule);
            if (templateRule != -1)
            {
                lifeData.iterationRuleIndex = templateRule;
                lifeRule = Configs.ruleSet.GetLifeRule(templateRule);
            }

            PaintPreview(templateData, paintPos);

            if (Input.GetMouseButtonDown(0))
            {
                ClearPreview();
                PaintTemplate(templateData, paintPos);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ClearPreview();
                templateIndex = 0;
            }
        }
    }

    public void PaintTemplate(Vector2Int[] templateData, Vector2 paintPos)
    {
        if (templateBuffer == null || curretnTemplateBufferSize != templateData.Length)
        {
            if (templateBuffer != null)
                templateBuffer.Release();

            templateBuffer = new ComputeBuffer(templateData.Length, sizeof(int) * 2);
            curretnTemplateBufferSize = templateData.Length;
        }
        templateBuffer.SetData(templateData);

        lifeComputer.SetFloat("_PaintPosX", paintPos.x);
        lifeComputer.SetFloat("_PaintPosY", paintPos.y);
        lifeComputer.SetInt("_TemplateLength", templateData.Length);
        lifeComputer.SetBuffer(templateKernel, "_Template", templateBuffer);
        lifeComputer.SetBuffer(templateKernel, "_Output", outputBuffer);

        int threadGroups = Mathf.CeilToInt(templateData.Length / 64.0f);
        lifeComputer.Dispatch(templateKernel, threadGroups, 1, 1);
    }

    public void PaintPreview(Vector2Int[] templateData, Vector2 paintPos)
    {
        ClearPreview();

        if (templateBuffer == null || curretnTemplateBufferSize != templateData.Length)
        {
            if (templateBuffer != null)
                templateBuffer.Release();

            templateBuffer = new ComputeBuffer(templateData.Length, sizeof(int) * 2);
            curretnTemplateBufferSize = templateData.Length;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            LifeTemplateUtil.RotatePattern(templateData, 90f);
        if (Input.GetKeyDown(KeyCode.E))
            LifeTemplateUtil.RotatePattern(templateData, -90f);

        templateBuffer.SetData(templateData);

        lifeComputer.SetFloat("_PaintPosX", paintPos.x);
        lifeComputer.SetFloat("_PaintPosY", paintPos.y);
        lifeComputer.SetInt("_TemplateLength", templateData.Length);
        lifeComputer.SetBuffer(previewKernel, "_Template", templateBuffer);
        lifeComputer.SetBuffer(previewKernel, "_Preview", previewBuffer);

        int threadGroups = Mathf.CeilToInt(templateData.Length / 64.0f);
        lifeComputer.Dispatch(previewKernel, threadGroups, 1, 1);
    }

    public void ResetState()
    {
        outputBuffer.SetData(initialCellStates);
    }

    public void ClearLife()
    {
        Array.Clear(lifeData.currentCellStates, 0, lifeData.currentCellStates.Length);
        outputBuffer.SetData(lifeData.currentCellStates);
    }

    public void ClearPreview()
    {
        lifeComputer.SetBuffer(clearPreviewKernel, "_Preview", previewBuffer);

        int threadGroups = Mathf.CeilToInt(lifeData.currentCellStates.Length / 64.0f);
        lifeComputer.Dispatch(clearPreviewKernel, threadGroups, 1, 1);
    }
}
