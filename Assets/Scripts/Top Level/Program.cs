using UnityEngine;

/// <summary>
/// Program：
/// </summary>
public class Program : MonoBehaviour
{
    public static Program instance;
    public GameDesc gameDesc;

    [SerializeField] public GameMain gameMain;

    private void OnEnable()
    {
        instance = this;

        gameDesc = new GameDesc();
        gameDesc.Init();
    }

    private void OnDisable()
    {
        if (gameDesc != null)
        {
            gameDesc.Free();
            gameDesc = null;
        }

        instance = null;
    }

    public void NewGame()
    {
        gameMain.gameObject.SetActive(true);
        gameMain.NewGame();

        UIRoot.instance.OnStartGame();
    }

    public void LoadGame(string fileName)
    {
        gameMain.gameObject.SetActive(true);
        gameMain.LoadGame(fileName);

        UIRoot.instance.OnStartGame();
    }

    public void EndGame()
    {
        gameMain.gameObject.SetActive(false);

        UIRoot.instance.OnEndGame();
    }
}
