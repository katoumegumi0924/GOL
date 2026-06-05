using UnityEngine;

/// <summary>
/// GameModel：
/// </summary>
public class GameModel
{
    public GameData gameData;
    public GameLogic gameLogic;

    public LifeRenderer lifeRenderer;

    public void Init(GameData _gameData, GameLogic _gameLogic)
    {
        gameData = _gameData;
        gameLogic = _gameLogic;

        lifeRenderer = new LifeRenderer();
        lifeRenderer.Init(gameData.lifeData, gameLogic.lifeLogic);
    }

    public void Free()
    {
        gameData = null;
        gameLogic = null;

        if (lifeRenderer != null)
        {
            lifeRenderer.Free();
            lifeRenderer = null;
        }
    }

    public void Update()
    {
        lifeRenderer.Update();
    }
}
