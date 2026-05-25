using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillComponent))]
public class SkillComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SkillComponent skill = (SkillComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set Clashing"))
        {
            skill.SetClashing();
        }

        if (GUILayout.Button("Set Not Clashing"))
        {
            skill.SetNotClashing();
        }

        if (GUILayout.Button("Clear Current Skill"))
        {
            skill.ClearCurrentSkill();
        }

        if (GUILayout.Button("Add Current Skill To Used"))
        {
            skill.AddCurrentSkillToUsed();
        }

        if (GUILayout.Button("Clear Used Skills"))
        {
            skill.ClearUsedSkills();
        }
    }
}