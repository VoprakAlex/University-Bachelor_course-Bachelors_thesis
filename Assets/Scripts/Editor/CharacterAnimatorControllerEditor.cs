using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterAnimatorController))]
public class CharacterAnimatorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterAnimatorController controller =
            (CharacterAnimatorController)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Play Attack"))
        {
            controller.PlayAttack();
        }

        if (GUILayout.Button("Play Hit"))
        {
            controller.PlayHit();
        }

        if (GUILayout.Button("Play Death"))
        {
            controller.PlayDeath();
        }
    }
}