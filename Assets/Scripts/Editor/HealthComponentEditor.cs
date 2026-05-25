using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthComponent))]
public class HealthComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthComponent health = (HealthComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set To Max"))
        {
            health.SetToMax();
        }

        if (GUILayout.Button("Take 10 Damage"))
        {
            health.DecreaseHP(10);
        }

        if (GUILayout.Button("Heal 10"))
        {
            health.IncreaseHP(10);
        }

        if (GUILayout.Button("Death"))
        {
            health.Death();
        }

        if (GUILayout.Button("Revive 10 HP"))
        {
            health.Revive(10);
        }
    }
}