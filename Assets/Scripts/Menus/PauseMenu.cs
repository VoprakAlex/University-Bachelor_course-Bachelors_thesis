using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject objectToHide;
    [SerializeField] private GameObject pauseBackground;

    private bool isPaused;

    private void Start()
    {
        if (pauseMenu != null && pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }

        if (pauseBackground != null && pauseBackground.activeSelf)
        {
            pauseBackground.SetActive(false);
        }
    }

    public void Pause()
    {

        Debug.Log("Pause - 1");
        pauseMenu.SetActive(true);
        pauseBackground.SetActive(true);
        Debug.Log("Pause - 2");

        objectToHide.SetActive(false);

        Debug.Log("Pause - 3");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Debug.Log("Resume - 1");
        pauseMenu.SetActive(false);
        pauseBackground.SetActive(false);
        Debug.Log("Resume - 2");
        objectToHide.SetActive(true);

        Debug.Log("Resume - 3");
        Time.timeScale = 1f;
    }
}