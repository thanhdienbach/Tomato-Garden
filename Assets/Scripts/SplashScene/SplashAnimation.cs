using UnityEngine;
using DG.Tweening;
using TMPro;

public class SplashAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI studioText;
    [SerializeField] private TextMeshProUGUI taglineText;

    [Header("Timings")]
    [SerializeField] private float titleFadeDuration = 0.6f;
    [SerializeField] private float studioFadeDelay = 0.2f;
    [SerializeField] private float studioFadeDuration = 0.6f;
    [SerializeField] private float taglineFadeDelay = 0.4f;
    [SerializeField] private float taglineFadeDuration = 0.6f;

    [Header("Fade-out Canvas")]
    [Tooltip("Thời điểm bắt đầu mờ dần (giây, tính từ lúc scene start)")]
    [SerializeField] private float canvasFadeOutDelay = 1.6f;
    [Tooltip("Thời gian mờ dần (giây)")]
    [SerializeField] private float canvasFadeOutDuration = 0.4f;

    private void Awake()
    {
        // Nền hiện sẵn, chỉ cho text trong suốt
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        if (titleText != null)
            titleText.alpha = 0f;

        if (studioText != null)
            studioText.alpha = 0f;

        if (taglineText != null)
            taglineText.alpha = 0f;
    }

    private void Start()
    {
        PlayIntroAnimation();
    }

    private void PlayIntroAnimation()
    {
        Sequence seq = DOTween.Sequence();

        // 1) Title fade-in trước
        if (titleText != null)
        {
            seq.Append(
                titleText.DOFade(0.8f, titleFadeDuration)
                         .SetEase(Ease.OutQuad)
            );
        }

        // 2) Studio hiện theo sau một chút
        if (studioText != null)
        {
            seq.Insert(
                studioFadeDelay,
                studioText.DOFade(1f, studioFadeDuration)
                          .SetEase(Ease.OutQuad)
            );
        }

        // 3) Tagline hiện muộn hơn và mờ hơn
        if (taglineText != null)
        {
            seq.Insert(
                taglineFadeDelay,
                taglineText.DOFade(0.6f, taglineFadeDuration) // 0.7 để mờ mờ
                          .SetEase(Ease.OutQuad)
            );
        }

        // 4) Cả Canvas mờ dần rồi biến mất sau 1.6s
        if (canvasGroup != null)
        {
            seq.Insert(
                canvasFadeOutDelay,
                canvasGroup.DOFade(0f, canvasFadeOutDuration)
                           .SetEase(Ease.InQuad)
            );
        }
    }
}

