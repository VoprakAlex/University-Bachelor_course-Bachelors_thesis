using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject secondObjectToHide;

    private void Start()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings - 1");
        Debug.Log(settingsPanel);
        settingsPanel.SetActive(true);
        Debug.Log("OpenSettings - 2");
        secondObjectToHide.SetActive(false);
        Debug.Log("OpenSettings - 3");
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings - 1");
        Debug.Log(settingsPanel);
        settingsPanel.SetActive(false);
        Debug.Log("CloseSettings - 2");

        secondObjectToHide.SetActive(true);
        Debug.Log("CloseSettings - 3");
    }
}