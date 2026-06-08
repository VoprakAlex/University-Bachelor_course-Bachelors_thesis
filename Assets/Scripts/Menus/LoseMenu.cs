using UnityEngine;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject loseBackground;

    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.OnLose.AddListener(ShowLoseScreen);
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.OnLose.RemoveListener(ShowLoseScreen);
    }

    private void Start()
    {
        if (loseMenu != null) loseMenu.SetActive(false);
        if (loseBackground != null) loseBackground.SetActive(false);
    }

    public void ShowLoseScreen()
    {
        Debug.Log("Lose Screen - Show");

        loseMenu.SetActive(true);
        loseBackground.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideLoseScreen()
    {
        loseMenu.SetActive(false);
        loseBackground.SetActive(false);
    }
}
