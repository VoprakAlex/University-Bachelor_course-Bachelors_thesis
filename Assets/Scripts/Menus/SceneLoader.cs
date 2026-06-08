using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        
        if (currentScene == "MainMenu")
        {
            SaveSystem.DeleteSave();
        }


        if (sceneName != "MainMenu")
        {
            SaveSystem.SaveLastScene(sceneName);
        }

        SceneManager.LoadScene(sceneName);
    }
}