using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResistanceComponent))]
public class ResistanceComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResistanceComponent resistance = (ResistanceComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set Standard Resistances"))
        {
            resistance.SetResistanceToStandard();
        }

        if (GUILayout.Button("Set All Type Resistances To Fatal"))
        {
            resistance.SetDamageTypeResistancesToMax();
        }

        if (GUILayout.Button("Set All Affinity Resistances To Fatal"))
        {
            resistance.SetDamageAffinityResistancesToMax();
        }

        if (GUILayout.Button("Increase All Type Resistances"))
        {
            resistance.IncreaseAllDamageTypeResistances();
        }

        if (GUILayout.Button("Decrease All Type Resistances"))
        {
            resistance.DecreaseAllDamageTypeResistances();
        }

        if (GUILayout.Button("Increase All Affinity Resistances"))
        {
            resistance.IncreaseAllDamageAffinityResistances();
        }

        if (GUILayout.Button("Decrease All Affinity Resistances"))
        {
            resistance.DecreaseAllDamageAffinityResistances();
        }
    }
}