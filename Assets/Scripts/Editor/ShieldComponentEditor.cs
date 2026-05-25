using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShieldComponent))]
public class ShieldComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShieldComponent shield = (ShieldComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set To Starting"))
        {
            shield.SetToStarting();
        }

        if (GUILayout.Button("Add 10 Shield"))
        {
            shield.IncreaseShield(10);
        }

        if (GUILayout.Button("Remove 10 Shield"))
        {
            shield.DecreaseShield(10);
        }
    }
}