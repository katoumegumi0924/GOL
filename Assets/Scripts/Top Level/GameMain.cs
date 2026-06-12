using UnityEngine;

/// <summary>
/// GameMain：
/// </summary>
public class GameMain : MonoBehaviour
{
    public static GameMain instance { get; private set; }

    public GameData data;
    public GameLogic logic;
    public GameModel model;

    public void Init()
    {
        instance = this;

        data = new GameData();
        data.Init();
        logic = new GameLogic();
        logic.Init(data);
        model = new GameModel();
        model.Init(data, logic);
    }

    public void Free()
    {
        if (data != null)
        {
            data.Free();
            data = null;
        }

        if (logic != null)
        {
            logic.Free();
            logic = null;
        }

        if (model != null)
        {
            model.Free();
            model = null;
        }

        instance = null;
    }

    public void NewGame()
    {
        data.SetNew();
        logic.SetNew();
    }

    public void LoadGame(string fileName)
    {
        GameSave.LoadGame(fileName, data);
        logic.AfterImport();
    }

    private void Update()
    {
        logic.Update();
        model.Update();
    }

    private void FixedUpdate()
    {
        logic.Tick();
    }

    private void LateUpdate()
    {
        logic.LateUpdate();
    }

    private void OnEnable()
    {
        Init();        
    }

    private void OnDisable()
    {
        Free();
    }
}
