using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class SkyGroup : MonoBehaviour
{
    [Header("Background Layers")]
    [SerializeField] private SpriteRenderer backgroundBase;
    [SerializeField] private SpriteRenderer backgroundOverlay;

    [Header("Sprites For Each Phase")]
    [SerializeField] private Sprite daySprite;
    [SerializeField] private Sprite nightSprite;
    [SerializeField] private Sprite duskSprite;
    [SerializeField] private Sprite eclipseSprite;

    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 4.0f;
    [SerializeField] private Ease fadeEase = Ease.InOutSine;

    [Tooltip("Khi đang chuyển phase mà đổi phase khác, tween cũ sẽ được tăng tốc để hoàn tất trong thời gian này (giây).")]
    [SerializeField] private float fastFinishDuration = 1f;

    private Tween _currentTween;
    private SkyPhase? _queuedPhase = null;

    private void Reset()
    {
        if (backgroundBase == null || backgroundOverlay == null)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            if (renderers.Length >= 2)
            {
                backgroundBase = renderers[0];
                backgroundOverlay = renderers[1];
            }
        }
    }

    private void Awake()
    {
        if (backgroundBase != null)
            backgroundBase.color = Color.white;

        if (backgroundOverlay != null)
        {
            var c = Color.white;
            c.a = 0f;
            backgroundOverlay.color = c;
            backgroundOverlay.sprite = null;
        }
    }

    // --- API public ---

    public void ApplyPhaseInstant(SkyPhase phase)
    {
        _currentTween?.Kill();
        _currentTween = null;
        _queuedPhase = null;

        Sprite sprite = GetSpriteForPhase(phase);
        if (sprite == null || backgroundBase == null || backgroundOverlay == null)
            return;

        backgroundBase.sprite = sprite;
        backgroundBase.color = Color.white;

        var c = backgroundOverlay.color;
        c.a = 0f;
        backgroundOverlay.color = c;
        backgroundOverlay.sprite = null;
    }

    public void FadeToPhase(SkyPhase phase)
    {
        Sprite sprite = GetSpriteForPhase(phase);
        if (sprite == null || backgroundBase == null || backgroundOverlay == null)
            return;

        // Nếu đang tween: tăng tốc tween cũ + queue phase mới
        if (_currentTween != null && _currentTween.IsActive() && _currentTween.IsPlaying())
        {
            _queuedPhase = phase;

            float full = _currentTween.Duration(false);
            float elapsed = _currentTween.Elapsed(false);
            float remaining = Mathf.Max(0f, full - elapsed);

            if (remaining > fastFinishDuration && fastFinishDuration > 0f)
            {
                float factor = remaining / fastFinishDuration;
                _currentTween.timeScale *= factor;
            }
            // Nếu remaining <= fastFinishDuration thì cứ để nó tự chạy tiếp
            return;
        }

        // Không có tween cũ → bắt đầu transition mới
        StartTransition(phase, sprite);
    }

    // --- Helper nội bộ ---

    private void StartTransition(SkyPhase phase, Sprite sprite)
    {
        // Chuẩn bị overlay
        backgroundOverlay.sprite = sprite;
        Color c = backgroundOverlay.color;
        c.a = 0f;
        backgroundOverlay.color = c;

        _currentTween = backgroundOverlay
            .DOFade(1f, fadeDuration)
            .SetEase(fadeEase)
            .OnComplete(() =>
            {
                // Đưa sprite mới xuống base
                backgroundBase.sprite = sprite;

                c = backgroundOverlay.color;
                c.a = 0f;
                backgroundOverlay.color = c;
                backgroundOverlay.sprite = null;

                _currentTween = null;

                // Nếu có phase chờ sẵn thì chạy luôn
                if (_queuedPhase.HasValue)
                {
                    SkyPhase next = _queuedPhase.Value;
                    _queuedPhase = null;
                    FadeToPhase(next);
                }
            });
    }

    private Sprite GetSpriteForPhase(SkyPhase phase)
    {
        switch (phase)
        {
            case SkyPhase.Day: return daySprite;
            case SkyPhase.Night: return nightSprite;
            case SkyPhase.Dusk: return duskSprite;
            case SkyPhase.Eclipse: return eclipseSprite;
            default: return null;
        }
    }
}


