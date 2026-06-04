using UnityEngine;

/// <summary>
/// Program：
/// </summary>
public class Program : MonoBehaviour
{
    public static Program instance;

    [SerializeField] public GameMain gameMain;

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;   
    }

    public void NewGame()
    {
        gameMain.gameObject.SetActive(true);
        gameMain.NewGame();

        UIRoot.instance.uiMainMenu._Close();
        UIRoot.instance.uiMainMenu._Free();

        UIRoot.instance.uiGame._Init(gameMain);
        UIRoot.instance.uiGame._Open();
    }

    public void LoadGame(string fileName)
    {
        gameMain.gameObject.SetActive(true);
        gameMain.LoadGame(fileName);

        UIRoot.instance.uiMainMenu._Close();
        UIRoot.instance.uiMainMenu._Free();

        UIRoot.instance.uiGame._Init(gameMain);
        UIRoot.instance.uiGame._Open();
    }

    public void EndGame()
    {
        gameMain.gameObject.SetActive(false);

        UIRoot.instance.uiGame._Close();
        UIRoot.instance.uiGame._Free();

        UIRoot.instance.uiMainMenu._Init(null);
        UIRoot.instance.uiMainMenu._Open();
    }
}
