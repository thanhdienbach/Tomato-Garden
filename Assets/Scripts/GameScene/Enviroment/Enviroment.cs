// Enviroment.cs
using UnityEngine;

[DisallowMultipleComponent]
public class Enviroment : GameSystem
{
    public static Enviroment Instance { get; private set; }

    [Header("Child Systems")]
    [SerializeField] private SkyGroup skyGroup;
    [SerializeField] private SkyDecorGroup skyDecorGroup;

    [Header("Initial Sky")]
    [SerializeField] private bool applyOnAwake = true;
    [SerializeField] private SkyPhase initialPhase = SkyPhase.Day;

    public SkyPhase CurrentSkyPhase { get; private set; } = SkyPhase.Day;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (skyGroup == null)
            skyGroup = GetComponentInChildren<SkyGroup>();

        if (skyDecorGroup == null)
            skyDecorGroup = GetComponentInChildren<SkyDecorGroup>();
    }

    public override void Init(GameContext _gameContext)
    {
        Debug.Log("Enviroment init");
        if (applyOnAwake)
        {
            SetSkyPhaseInstant(initialPhase);
        }
    }

    public void SetSkyPhase(SkyPhase phase)
    {
        if (phase == CurrentSkyPhase)
            return;

        if (skyGroup != null)
            skyGroup.FadeToPhase(phase);

        if (skyDecorGroup != null)
            skyDecorGroup.FadeToPhase(phase);

        CurrentSkyPhase = phase;
    }

    public void SetSkyPhaseInstant(SkyPhase phase)
    {
        if (skyGroup != null)
            skyGroup.ApplyPhaseInstant(phase);

        if (skyDecorGroup != null)
            skyDecorGroup.ApplyPhaseInstant(phase);

        CurrentSkyPhase = phase;
    }

    #region Shortcuts

    public void SetDay() => SetSkyPhase(SkyPhase.Day);
    public void SetNight() => SetSkyPhase(SkyPhase.Night);
    public void SetDusk() => SetSkyPhase(SkyPhase.Dusk);
    public void SetEclipse() => SetSkyPhase(SkyPhase.Eclipse);

    #endregion
}

