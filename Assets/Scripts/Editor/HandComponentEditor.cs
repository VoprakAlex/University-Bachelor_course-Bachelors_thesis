using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HandComponent))]
public class HandComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HandComponent hand = (HandComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Rebuild Deck"))
        {
            hand.RebuildDeck();
        }

        if (GUILayout.Button("Draw Skill"))
        {
            hand.DrawSkill();
        }
    }
}