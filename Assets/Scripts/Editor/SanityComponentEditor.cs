using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SanityComponent))]
public class SanityComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SanityComponent sanity = (SanityComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set To Max"))
        {
            sanity.SetToMax();
        }

        if (GUILayout.Button("Take 10 SP Damage"))
        {
            sanity.DecreaseSanity(10);
        }

        if (GUILayout.Button("Restore 10 SP"))
        {
            sanity.IncreaseSanity(10);
        }

        if (GUILayout.Button("Madness"))
        {
            sanity.Madness();
        }

        if (GUILayout.Button("Recover 10 SP"))
        {
            sanity.Recover(10);
        }
    }
}