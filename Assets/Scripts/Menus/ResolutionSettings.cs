using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        resolutionDropdown.value = 0;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;

            case 1:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;

            case 2:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                break;

            case 3:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;

            case 4:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;

            case 5:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
        }
    }
}