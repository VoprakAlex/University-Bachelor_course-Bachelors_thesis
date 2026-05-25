using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombatManager))]
public class CombatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CombatManager combat = (CombatManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Clear Attacker"))
        {
            combat.ClearAttacker();
        }

        if (GUILayout.Button("Execute Combat"))
        {
            combat.ExecuteCombat();
        }
    }
}