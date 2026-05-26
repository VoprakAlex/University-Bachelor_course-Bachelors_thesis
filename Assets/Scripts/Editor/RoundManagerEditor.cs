using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoundManager))]
public class RoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoundManager roundManager = (RoundManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Prepare Round"))
        {
            roundManager.PrepareRound();
        }
    }
}