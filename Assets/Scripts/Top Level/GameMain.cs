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
        logic = new GameLogic();
        model = new GameModel();
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
        data.Init();
        data.SetNew();
        logic.Init(data);
        logic.SetNew();
        model.Init(data, logic);
    }

    public void LoadGame(string fileName)
    {
        data.Init();
        GameSave.LoadGame(fileName, data);

        logic.Init(data);
        model.Init(data, logic);
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
