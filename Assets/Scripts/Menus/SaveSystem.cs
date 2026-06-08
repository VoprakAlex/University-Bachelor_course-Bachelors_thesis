using UnityEngine;

public static class SaveSystem
{
    private const string LastSceneKey = "LastScene";

    public static void SaveLastScene(string sceneName)
    {
        PlayerPrefs.SetString(LastSceneKey, sceneName);
        PlayerPrefs.Save();
        Debug.Log(LastSceneKey);
        Debug.Log(sceneName);
    }

    public static string GetLastScene()
    {
        Debug.Log(PlayerPrefs.GetString(LastSceneKey, ""));
        return PlayerPrefs.GetString(LastSceneKey, "");
    }

    public static bool HasSavedScene()
    {
        Debug.Log(PlayerPrefs.GetString(LastSceneKey, ""));
        return PlayerPrefs.HasKey(LastSceneKey);
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(LastSceneKey);
        PlayerPrefs.Save();
    }
}