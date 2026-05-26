using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject secondObjectToHide;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        if (secondObjectToHide != null)
            secondObjectToHide.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        if (secondObjectToHide != null)
            secondObjectToHide.SetActive(true);
    }
}