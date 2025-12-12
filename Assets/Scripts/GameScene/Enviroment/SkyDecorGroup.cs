// SkyDecorGroup.cs
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class SkyDecorGroup : MonoBehaviour
{
    [Header("Roots")]
    [SerializeField] private Transform dayCloudsRoot;
    [SerializeField] private Transform nightStarsRoot;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Ease fadeEase = Ease.InOutSine;

    private SpriteRenderer[] _dayClouds;
    private SpriteRenderer[] _nightStars;

    private void Reset()
    {
        // Tự bắt con tên DayClouds / NightStars nếu có
        if (dayCloudsRoot == null)
        {
            var child = transform.Find("DayClouds");
            if (child != null) dayCloudsRoot = child;
        }

        if (nightStarsRoot == null)
        {
            var child = transform.Find("NightStars");
            if (child != null) nightStarsRoot = child;
        }
    }

    private void Awake()
    {
        if (dayCloudsRoot != null)
            _dayClouds = dayCloudsRoot.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);

        if (nightStarsRoot != null)
            _nightStars = nightStarsRoot.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
    }

    // ----- API public -----

    public void ApplyPhaseInstant(SkyPhase phase)
    {
        GetTargetAlphas(phase, out float cloudsAlpha, out float starsAlpha);
        SetGroupAlphaInstant(_dayClouds, cloudsAlpha);
        SetGroupAlphaInstant(_nightStars, starsAlpha);
    }

    public void FadeToPhase(SkyPhase phase)
    {
        GetTargetAlphas(phase, out float cloudsAlpha, out float starsAlpha);
        FadeGroupToAlpha(_dayClouds, cloudsAlpha);
        FadeGroupToAlpha(_nightStars, starsAlpha);
    }

    // ----- Helpers nội bộ -----

    private void GetTargetAlphas(SkyPhase phase, out float cloudsAlpha, out float starsAlpha)
    {
        // Bạn có thể tinh chỉnh mapping này thêm sau
        switch (phase)
        {
            case SkyPhase.Day:
                cloudsAlpha = 1f;   // mây rõ
                starsAlpha = 0f;    // không thấy sao
                break;

            case SkyPhase.Night:
                cloudsAlpha = 0f;
                starsAlpha = 1f;    // sao rõ
                break;

            case SkyPhase.Dusk:
                cloudsAlpha = 0.3f; // mờ mờ
                starsAlpha = 0.7f;  // sao đã khá thấy
                break;

            case SkyPhase.Eclipse:
                cloudsAlpha = 0f;
                starsAlpha = 1f;    // cho giống mode sức mạnh
                break;

            default:
                cloudsAlpha = 0f;
                starsAlpha = 0f;
                break;
        }
    }

    private void SetGroupAlphaInstant(SpriteRenderer[] group, float alpha)
    {
        if (group == null) return;
        foreach (var sr in group)
        {
            if (sr == null) continue;
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }

    private void FadeGroupToAlpha(SpriteRenderer[] group, float targetAlpha)
    {
        if (group == null) return;

        foreach (var sr in group)
        {
            if (sr == null) continue;

            // Kill tween cũ trên renderer này (nếu có)
            sr.DOKill();

            sr.DOFade(targetAlpha, fadeDuration)
              .SetEase(fadeEase);
        }
    }
}

