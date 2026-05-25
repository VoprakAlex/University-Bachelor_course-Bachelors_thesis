using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetComponent))]
public class TargetComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TargetComponent targetComponent = (TargetComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Clear Main Target"))
        {
            targetComponent.ClearMainTarget();
        }

        if (GUILayout.Button("Clear Sub Targets"))
        {
            targetComponent.ClearSubTargets();
        }

        if (GUILayout.Button("Clear All Targets"))
        {
            targetComponent.ClearTargets();
        }

        if (GUILayout.Button("Remove First Sub Target"))
        {
            targetComponent.RemoveFirstSubTarget();
        }
    }
}