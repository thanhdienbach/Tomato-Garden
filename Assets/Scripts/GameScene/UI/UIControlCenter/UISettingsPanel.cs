using UnityEngine;
using UnityEngine.UI;

public class UISettingsPanel : MonoBehaviour
{

    public Button closeSettingsButton;
    public GameObject gameSettingsPanle;

    public void SetUISetingsPanleActiveTrue()
    {
        gameObject.SetActive(false);
    }
    public void SetUISetingsPanleActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
