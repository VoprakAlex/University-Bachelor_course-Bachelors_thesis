using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject objectToHide;

    private bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);

        if (objectToHide != null)
            objectToHide.SetActive(false);

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        if (objectToHide != null)
            objectToHide.SetActive(true);

        Time.timeScale = 1f;
    }
}