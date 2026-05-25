using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpeedComponent))]
public class SpeedComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpeedComponent speed = (SpeedComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Random Speed"))
        {
            speed.SetRandomSpeed();
        }

        if (GUILayout.Button("Increase Speed"))
        {
            speed.IncreaseSpeed(1);
        }

        if (GUILayout.Button("Decrease Speed"))
        {
            speed.DecreaseSpeed(1);
        }
    }
}