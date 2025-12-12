using UnityEngine;
using UnityEngine.UI;

public class UIControlCenter : MonoBehaviour
{
    [Header("Header element")]
    [SerializeField] private Button enterGardenButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button leaveStationButton;

    [Header("Stage root (optional)")]
    [Tooltip("Game farm root")]
    [SerializeField] private GameObject farmRoot;
    [Tooltip("Control Centor roor")]
    [SerializeField] private GameObject controlCentorRoot;
    [SerializeField] GameObject settingPanle;

    public void OnEnterGarden()
    {
        if (farmRoot != null)
            farmRoot.SetActive(true);

        if (controlCentorRoot != null)
            controlCentorRoot.SetActive(false);

        Debug.Log("Enter tomato garden");
    }

    public void OnOpenSetting()
    {
        settingPanle.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnOpenThis()
    {
        settingPanle.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void OnLeaveStation()
    {
#if UNITY_EDITOR
        Debug.Log("Exit App");
#else
        Application.Quit();
#endif
    }

}
