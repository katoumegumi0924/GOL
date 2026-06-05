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

        lifeLogic = new LifeLogic();
        lifeLogic.Init(gameData.lifeData, gameData.lifeTime, playerController);

        timeLogic = new TimeLogic();
        timeLogic.Init(gameData.lifeTime);
    }

    public void Free()
    {
        if (playerController != null)
        {
            playerController.Free();
            playerController = null;
        }

        if (cameraController != null)
        {
            cameraController.Free();
            cameraController = null;
        }

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

        gameData = null;
    }

    public void SetNew()
    {

    }

    public void Update()
    {
        playerController.Update();
    }

    public void LateUpdate()
    {
        cameraController.Update();
    }

    public void Tick()
    {
        lifeLogic.Tick();
        timeLogic.Tick();
    }
}
