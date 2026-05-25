using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StaggerComponent))]
public class StaggerComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StaggerComponent stagger = (StaggerComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Stagger"))
        {
            stagger.Stagger();
        }

        if (GUILayout.Button("Unstagger"))
        {
            stagger.Unstagger();
        }

        if (GUILayout.Button("Increase Threshold"))
        {
            stagger.IncreaseStaggerThreshold(10);
        }

        if (GUILayout.Button("Decrease Threshold"))
        {
            stagger.DecreaseStaggerThreshold(10);
        }
    }
}