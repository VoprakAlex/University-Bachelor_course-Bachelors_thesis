using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gameManager = (GameManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Win Game"))
        {
            gameManager.WinGame();
        }

        if (GUILayout.Button("Lose Game"))
        {
            gameManager.LoseGame();
        }
    }
}