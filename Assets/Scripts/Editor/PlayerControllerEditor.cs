using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController controller = (PlayerController)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Draw Hand"))
        {
            controller.DrawAllCards();
        }

        if (GUILayout.Button("Clear Hand"))
        {
            controller.ClearAllCards();
        }

        if (GUILayout.Button("Refresh Hand"))
        {
            controller.RefreshHandView();
        }

        if (GUILayout.Button("Fill Components"))
        {
            controller.FillComponents();
        }
    }
}