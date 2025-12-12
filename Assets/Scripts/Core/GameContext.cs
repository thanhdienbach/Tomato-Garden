using UnityEditor;

public sealed class GameContext
{
    public GameManager GameManager { get; }
    public UIManager UIManager { get; }
    public Enviroment Enviroment { get; }

    public GameContext(GameManager gameManager, UIManager uIManager, Enviroment enviroment)
    {
        GameManager = gameManager;
        UIManager = uIManager;
        Enviroment = enviroment;
    }
}
