using UnityEngine;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject winBackground;

    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.OnWin.AddListener(ShowWinScreen);
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.OnWin.RemoveListener(ShowWinScreen);
    }

    private void Start()
    {
        if (winMenu != null) winMenu.SetActive(false);
        if (winBackground != null) winBackground.SetActive(false);
    }

    public void ShowWinScreen()
    {
        Debug.Log("Win Screen - Show");

        winMenu.SetActive(true);
        winBackground.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideWinScreen()
    {
        winMenu.SetActive(false);
        winBackground.SetActive(false);
    }
}
