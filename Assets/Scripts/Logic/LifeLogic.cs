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

    public ComputeShader lifeShader;
    public ComputeBuffer inputBuffer;
    public ComputeBuffer outputBuffer;

    public LifeRuleConfig lifeRule;

    private int updateKernel;
    private int paintKernel;
    private int accumulator;

    private Vector2 _lastFrameUV;

    // 初始状态
    private int[] initialCellStates;

    public void Init(LifeData _lifeData, TimeData _lifeTime, PlayerController _playerController)
    {
        lifeData = _lifeData;
        lifeTime = _lifeTime;
        playerController = _playerController;
        lifeShader = Configs.GPGPU.lifeShader;

        updateKernel = lifeShader.FindKernel("CSUpdate");
        paintKernel = lifeShader.FindKernel("CSPaint");
        inputBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));
        outputBuffer = new ComputeBuffer(lifeData.currentCellStates.Length, sizeof(int));

        inputBuffer.SetData(lifeData.currentCellStates);
        outputBuffer.SetData(lifeData.currentCellStates);

        lifeRule = Configs.ruleSet.GetLifeRule(lifeData.iterationRuleIndex);

        initialCellStates = new int[lifeData.currentCellStates.Length];
        Array.Copy(lifeData.currentCellStates, initialCellStates, initialCellStates.Length);
    }

    public void Free()
    {
        lifeData = null;
        lifeTime = null;
        playerController = null;
        lifeShader = null;

        updateKernel = 0;
        paintKernel = 0;

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

        lifeRule = null;
        initialCellStates = null;
    }

    public void SetNew()
    {

    }

    public void Tick()
    {
        accumulator += lifeTime.tickDelta;
        while (accumulator >= Configs.builtin.singleStepTick)
        {
            LifeTick();
            accumulator -= Configs.builtin.singleStepTick;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Paint();
        }
    }

    public void LifeTick()
    {
        ComputeBuffer temp = inputBuffer;
        inputBuffer = outputBuffer;
        outputBuffer = temp;

        // 将数据发送到GPU执行更新逻辑
        lifeShader.SetBuffer(updateKernel, "_Input", inputBuffer);
        lifeShader.SetBuffer(updateKernel, "_Output", outputBuffer);

        lifeShader.SetInts("_Res", lifeData.resX, lifeData.resY);
        lifeShader.SetInt("_SurvivalMask", lifeRule.survivalMask);
        lifeShader.SetInt("_BirthMask", lifeRule.birthMask);

        int groupsX = Mathf.CeilToInt(lifeData.resX / 8.0f);
        int groupsY = Mathf.CeilToInt(lifeData.resY / 8.0f);
        lifeShader.Dispatch(updateKernel, groupsX, groupsY, 1);  
    }

    public void Paint()
    {
        var paintUV = playerController.paintUV;
        var paintValue = playerController.paintValue;

        if (!float.IsNegativeInfinity(paintUV.x))
        {
            float stepSize = Mathf.Max(lifeData.resX, lifeData.resY);

            if (float.IsNegativeInfinity(_lastFrameUV.x) || Vector2.Distance(_lastFrameUV, paintUV) < 1 / stepSize)
            {
                PaintCell(paintUV, paintValue);
            }
            else
            {
                PaintLine(_lastFrameUV, paintUV, paintValue);
            }

            _lastFrameUV = paintUV;
        }
        else
        {
            _lastFrameUV = Vector2.negativeInfinity;
        }
    }

    public void PaintCell(Vector2 paintUV, int paintValue)
    {
        if (!float.IsNegativeInfinity(paintUV.x))
        {
            lifeShader.SetInt("_PaintValue", paintValue);
            lifeShader.SetFloats("_PaintUV", paintUV.x, paintUV.y);
            lifeShader.SetBuffer(paintKernel, "_Output", outputBuffer);

            lifeShader.Dispatch(paintKernel, 1, 1, 1);
        }
    }

    public void PaintLine(Vector2 start, Vector2 end, int paintValue)
    {
        float dist = Vector2.Distance(start, end);

        float stepSize = Mathf.Max(lifeData.resX, lifeData.resY);
        int step = Mathf.CeilToInt(stepSize * dist);

        for (int i = 0; i < step; ++i)
        {
            float t = (float)i / step;
            Vector2 paintUV = Vector2.Lerp(start, end, t);
            PaintCell(paintUV, paintValue);
        }
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
}
