using UnityEngine;

/// <summary>
/// GameLogic：
/// </summary>
public class GameLogic
{
    public GameData gameData;

    public PlayerController playerController;
    public CameraController cameraController;

    public LifeLogic lifeLogic;
    public TimeLogic timeLogic;

    public void Init(GameData _gameData)
    {
        gameData = _gameData;

        playerController = new PlayerController();
        playerController.Init();

        cameraController = new CameraController();
        cameraController.Init(gameData.lifeData);

        timeLogic = new TimeLogic();
        timeLogic.Init(gameData.gameTime);

        lifeLogic = new LifeLogic();
        lifeLogic.Init(gameData.lifeData, gameData.gameTime, playerController);
    }

    public void Free()
    {
        if (lifeLogic != null)
        {
            lifeLogic.Free();
            lifeLogic = null;
        }

        if (timeLogic != null)
        {
            timeLogic.Free();
            timeLogic = null;
        }

        if (cameraController != null)
        {
            cameraController.Free();
            cameraController = null;
        }

        if (playerController != null)
        {
            playerController.Free();
            playerController = null;
        }

        gameData = null;
    }

    public void SetNew()
    {
        playerController.SetNew();
        cameraController.SetNew();
        timeLogic.SetNew();
        lifeLogic.SetNew();
    }

    public void AfterImport()
    {
        lifeLogic.AfterImport();
    }

    public void Update()
    {
        playerController.Update();
        lifeLogic.Update();
    }

    public void LateUpdate()
    {
        cameraController.Update();
    }

    public void Tick()
    {
        timeLogic.Tick();
        lifeLogic.Tick();
    }
}
