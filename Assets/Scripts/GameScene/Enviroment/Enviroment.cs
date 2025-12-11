using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public static Enviroment Instance;

    [Header("Child System")]
    [SerializeField] SkyGroup skyGroup;

    [Header("Initial Sky")]
    [SerializeField] bool applyOnAwake = true;
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

        DontDestroyOnLoad(gameObject);

        if (applyOnAwake && skyGroup != null)
        {
            skyGroup.ApplyPhaseInstant(initialPhase);
            CurrentSkyPhase = initialPhase;
        }
    }

    public void SetSkyPhase(SkyPhase _skyPhase)
    {
        Debug.Log("Set day");
        if (skyGroup == null) return;

        if (_skyPhase == CurrentSkyPhase) return;

        skyGroup.FadeToPhase(_skyPhase);
        CurrentSkyPhase = _skyPhase;
    }

    public void SetSkyPhaseInstance(SkyPhase _skyPhase)
    {
        skyGroup.ApplyPhaseInstant(_skyPhase);
        CurrentSkyPhase = _skyPhase;
    }

    #region Shortcut methods
    public void SetDay() => SetSkyPhase(SkyPhase.Day);

    public void SetNight() => SetSkyPhase(SkyPhase.Night);
    public void SetDusk() => SetSkyPhase(SkyPhase.Dusk);
    public void SetEclipse() => SetSkyPhase(SkyPhase.Eclipse);

    #endregion
}
