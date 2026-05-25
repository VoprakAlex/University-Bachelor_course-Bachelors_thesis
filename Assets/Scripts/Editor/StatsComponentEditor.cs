using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatsComponent))]
public class StatsComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StatsComponent stats = (StatsComponent)target;

        GUILayout.Space(10);

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("Random Speed"))
        {
            stats.RandomSpeed();
        }

        if (GUILayout.Button("Take 10 Raw HP Damage"))
        {
            stats.TakeHPDamage(10);
        }

        if (GUILayout.Button("Take 10 Raw SP Damage"))
        {
            stats.TakeSPDamage(10);
        }

        if (GUILayout.Button("Calculate 10 Slashing Physical Damage"))
        {
            stats.CalculateDamage(
                10,
                DamageType.Slashing,
                DamageAffinity.Physical
            );
        }

        if (GUILayout.Button("Calculate 10 Fire Damage"))
        {
            stats.CalculateDamage(
                10,
                DamageType.Slashing,
                DamageAffinity.Fire
            );
        }

        if (GUILayout.Button("Calculate 10 Mind Damage"))
        {
            stats.CalculateDamage(
                10,
                DamageType.Bludgening,
                DamageAffinity.Mind
            );
        }

        GUI.enabled = true;
    }
}