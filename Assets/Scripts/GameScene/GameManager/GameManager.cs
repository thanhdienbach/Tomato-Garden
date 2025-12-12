using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    #region Instance
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    [Header("Core systen")]
    [SerializeField] private UIManager uIManager;
    [SerializeField] private Enviroment enviroment;

    [Header("Other system")]
    [SerializeField] private GameSystem[] extraSystem;

    private GameContext gameContext;
    private bool initialized;

    private void Start()
    {
        InitAll();
    }
    private void InitAll()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;

        gameContext = new GameContext(this, uIManager, enviroment);

        var systems = BuildSystemsArray();
        Array.Sort(systems, (a,b) => a.InitOrder.CompareTo(b.InitOrder));

        for (int i = 0; i < systems.Length; i++)
        {
            if (systems[i] == null) continue;
            systems[i].Init(gameContext);
        }
        
    }

    private GameSystem[] BuildSystemsArray()
    {
        int extraLen = extraSystem != null ? extraSystem.Length : 0;

        var arr = new GameSystem[2 + extraLen];
        arr[0] = uIManager;
        arr[1] = enviroment;

        for (int i = 0; i < extraLen; i++)
        {
            arr[2 + i] = extraSystem[i];
        }

        return arr;
    }

    private void OnApplicationQuit()
    {
        // Shutdown theo thứ tự ngược
    }
}
