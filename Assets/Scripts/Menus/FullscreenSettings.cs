using UnityEngine;
using TMPro;

public class FullscreenSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown fullscreenDropdown;

    private void Start()
    {
        fullscreenDropdown.value = PlayerPrefs.GetInt("FullscreenMode", 0);
        SetFullscreenMode(fullscreenDropdown.value);
    }

    public void SetFullscreenMode(int mode)
    {
        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }

        PlayerPrefs.SetInt("FullscreenMode", mode);
        PlayerPrefs.Save();
    }
}