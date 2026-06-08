using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueGameButton : MonoBehaviour
{
    private void Start()
    {
        if (!SaveSystem.HasSavedScene())
        {
            
            GetComponent<Button>().interactable = false;
        }
    }

    public void ContinueGame()
    {
        if (!SaveSystem.HasSavedScene())
            return;

        string lastScene = SaveSystem.GetLastScene();

        if (!string.IsNullOrEmpty(lastScene))
        {
            SceneManager.LoadScene(lastScene);
        }
    }
}