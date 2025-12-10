using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    [Tooltip("Thời gian tối thiểu hiển thị Splash (giây)")]
    [SerializeField] private float minSplashTime = 2.0f;

    private void Start()
    {
        StartCoroutine(SplashRoutine());
    }

    private IEnumerator SplashRoutine()
    {
        // Lấy index của scene hiện tại (Splash = 0)
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1; // Scene tiếp theo trong Build Settings

        // Nếu lỡ cấu hình sai (không có scene tiếp theo) thì thoát
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("[SplashManager] Không tìm thấy scene tiếp theo trong Build Settings!");
            yield break;
        }

        // Bắt đầu load scene tiếp theo ngầm
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(nextIndex);
        loadOp.allowSceneActivation = false;

        float timer = 0f;

        // Đợi đủ thời gian tối thiểu + scene load xong (progress ~0.9f)
        while (timer < minSplashTime || loadOp.progress < 0.9f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Cho phép chuyển sang scene tiếp theo (MainMenuScene)
        loadOp.allowSceneActivation = true;
    }
}

