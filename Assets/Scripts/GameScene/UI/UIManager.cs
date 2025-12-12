using UnityEngine;

[DisallowMultipleComponent]
public class UIManager : GameSystem
{
    public static UIManager Instance { get; private set; }

    [Header("Views (drag & drop)")]
    [SerializeField] private UIControlCenter controlCenter;
    [SerializeField] private UISettingsPanel settingsPanel;

    public override void Init(GameContext _gameContext)
    {
        Debug.Log("Init UIManager");
        if (controlCenter != null)
        {
            controlCenter.gameObject.SetActive(true);
        }
        if (settingsPanel != null)
        {
            settingsPanel.gameObject.SetActive(false);
        }
    }

}

